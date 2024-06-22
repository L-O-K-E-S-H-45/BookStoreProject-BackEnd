
use BookStoreDB

create table Carts(
CartId int primary key identity,
UserId int foreign key references Users(UserId),
BookId int foreign key references Books(BookId),
Title nvarchar(max) not null,
Author nvarchar(max) not null,
Image nvarchar(max) not null,
Quantity int default 1 check(Quantity between 1 and 5),  -- because in real world example(flipkart), while adding product to cart we don't have option for quantity
OriginalBookPrice int not null,
FinalBookPrice int not null,
--IsDeleted bit default 0
)

--drop table Carts

select * from Carts

---------------------------------
To get last insetred record id
In SQL Server, we can use the OUTPUT clause or the SCOPE_IDENTITY() function.

-- Using OUTPUT clause
INSERT INTO your_table (column1, column2)
OUTPUT inserted.id
VALUES (value1, value2);

-- Using SCOPE_IDENTITY() function
INSERT INTO your_table (column1, column2) VALUES (value1, value2);
SELECT SCOPE_IDENTITY();
---------------------------------------

-----------------------------------  AddBookToCart  ----------------------------------------

create or alter proc usp_AddBookToCart(
@UserId int,
@BookId int
)
as
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	

		if (@UserId is null or @BookId is null)
		begin
			set @ErrorMessage = 'UserId & BookId cannot be null'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		if exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin

			if exists (select 1 from Books where BookId = @BookId and IsDeleted = 0)
			begin
				
				if not exists (select 1 from Carts where UserId = @UserId and BookId = @BookId)
				begin

					declare
					@Title nvarchar(max),
					@Author nvarchar(max),
					@Image nvarchar(max),
					@OriginalBookPrice int,
					@FinalBookPrice int;

					select @Title = Title, 
							@Author = Author,
							@Image = Image,
							@OriginalBookPrice = OriginalPrice,
							@FinalBookPrice = Price
							from Books where BookId = @BookId
			
					insert into Carts(UserId, BookId, Title, Author, Image, OriginalBookPrice, FinalBookPrice)
						values (@UserId, @BookId, @Title, @Author, @Image, @OriginalBookPrice, @FinalBookPrice)

					declare @CartId int = SCOPE_IDENTITY();
					select * from Carts where CartId = @CartId;
					return;
				end

				--else if exists (select 1 from Carts where UserId = @UserId and BookId = @BookId where IsDeleted = 1)
				--begin
				--	update Carts set IsDeleted = 0 where UserId = @UserId and BookId = @BookId where IsDeleted = 1;
				--	select * from Carts where CartId = @CartId;
				--	return;
				--end

				else
				begin
					set @ErrorMessage = 'Book already added to the cart, Please go to cart';
					--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
				end
			end
			else
			begin
				set @ErrorMessage = 'Book does not exist';
				--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else
		begin
			set @ErrorMessage = 'User does not exist';
			--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		
	end try
	begin catch
		throw;
	end catch
end;

select * from Carts

select * from Users
select * from Books

exec usp_AddBookToCart @UserId = 6, @BookId = 8


--------------------------------------  ViewAllCarts  ------------------------------------

create or alter proc usp_ViewAllCarts
as
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	

		select * from Carts;
		
	end try
	begin catch
		SET @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		throw;
	end catch
end;

exec usp_ViewAllCarts

--------------------------------------  ViewCartByUser  ------------------------------------

create or alter proc usp_ViewCartByUser(
@UserId int
)
as
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	

		if (@UserId is null)
		begin
			set @ErrorMessage = 'UserId cannot be null'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		if exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin
			
			select * from Carts where UserId = @UserId;

		end
		else
		begin
			set @ErrorMessage = 'User does not exist';
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		
	end try
	begin catch
		throw;
	end catch
end;

exec usp_ViewCartByUser @UserId = 3

select * from Carts

select * from Users


--------------------------------------  UpdateCart  ------------------------------------

create or alter proc usp_UpdateCart(
@CartId int,
@Quantity int
)
as
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	
		if exists (select 1 from Carts where CartId = @CartId)
		begin
			if (@Quantity between 1 and 5)
			begin
				update Carts set Quantity = @Quantity, OriginalBookPrice = (OriginalBookPrice/Quantity * @Quantity), FinalBookPrice = FinalBookPrice/Quantity * @Quantity
					where CartId = @CartId;

					select * from Carts where CartId = @CartId;
			end
			else
			begin
				SET @ErrorMessage = 'Quantity must be between 1 and 5';
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else
		begin
			SET @ErrorMessage = 'Cart does not exist for id: ' + @CartId;
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
	end try
	begin catch
		SET @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
	end catch
end;

select * from Carts

exec usp_UpdateCart @CartId =12 , @Quantity = 2


--------------------------------------  RemoveBookFromcart  ------------------------------------

create or alter proc usp_RemoveBookFromcart(
@CartId int
)
as
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	

		if (@CartId is null)
		begin
			set @ErrorMessage = 'CartId cannot be null'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		if exists (select 1 from Carts where CartId = @CartId)
		begin
			
			delete from Carts where CartId = @CartId;

			if (@@ROWCOUNT != 1)
			begin
				set @ErrorMessage = 'Cart did not deleted';
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else
		begin
			set @ErrorMessage = 'Cart does not exist for cartId: ' + CAST(@CartId as nvarchar(50));
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		
	end try
	begin catch
		throw;
	end catch
end;

select * from Carts

exec usp_RemoveBookFromcart @CartId = 5


------------------------- CountBooksInUserCart ---------------

create or alter proc usp_CountBooksInUserCart(
@UserId int
)
as
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	

		if (@UserId is null)
		begin
			set @ErrorMessage = 'UserId cannot be null'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		if exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin
			
			declare @NoOfBooks int;
			select @NoOfBooks = COUNT(*) from Carts where UserId = @UserId;
			print @NoOfBooks;

			--select @NoOfBooks as NoOfBooks;
			--return;

			return @NoOfBooks ;
		end
		else
		begin
			set @ErrorMessage = 'User does not exist';
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		
	end try
	begin catch
		throw;
	end catch
end;

exec usp_CountBooksInUserCart @UserId = 2

select * from Carts

select * from Users

select * from Books













