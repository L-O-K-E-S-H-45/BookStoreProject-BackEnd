
use BookStoreDB

create table Books(
BookId int primary key identity,
Title nvarchar(max) not null,
Author nvarchar(max) not null,
Description nvarchar(max) not null,
Rating decimal(2,1) default 0,
RatingCount int default 0,
OriginalPrice int not null,
DiscountPercentage int not null,
Price as cast(OriginalPrice*(1-DiscountPercentage/100.0) as int) persisted not null, 
Quantity int not null,
Image nvarchar(max),
IsDeleted bit default 0,
CreatedAt datetime default getdate(),
UpdatedAt datetime default getdate(),
constraint chk_rating check(Rating between 0 and 5),
constraint chk_discount check(DiscountPercentage between 0 and 100),
constraint chk_price check(Price <= OriginalPrice),
)

select * from Books

drop table Books

--------- AddBook

alter proc usp_AddBook(
@Title nvarchar(max),
@Author nvarchar(max),
@Description nvarchar(max),
@OriginalPrice int,
@DiscountPercentage int,
@Quantity int,
@Image nvarchar(max)
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
		if not exists (select 1 from Books where Title = @Title and Author = @Author)
		begin
			insert into Books(Title, Author, Description, OriginalPrice, DiscountPercentage, Quantity, Image)
				values (@Title, @Author, @Description, @OriginalPrice, @DiscountPercentage, @Quantity, @Image)
			
			select * from Books where Title = @Title and Author = @Author;
		end 
		else if exists(select 1 from Books where Title = @Title and Author = @Author and IsDeleted = 1)
		begin
			declare @Update datetime = getdate();
			update Books set Description = @Description, OriginalPrice = @OriginalPrice, DiscountPercentage = @DiscountPercentage,
				Quantity = @Quantity, Image = @Image, IsDeleted = 0, UpdatedAt = @Update where Title = @Title and Author = @Author

			select * from Books where Title = @Title and Author = @Author;
		end
		else
		begin
			set @ErrorMessage = 'Book already exist with Title: ' + @Title + ' and Author: ' + @Author;
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
	end try
	begin catch
		throw;
	end catch
end;

select * from Books

exec usp_AddBook 'Book1', 'Author1', 'Desc for book1', 1800, 15, 12, 'Book1-image'
exec usp_AddBook 'Book2', 'Author2', 'Desc for book2', 2500, 20, 36, 'Book2-image'
exec usp_AddBook 'Book3', 'Author3', 'Desc for book3', 950, 8, 22, 'Book3-image'
exec usp_AddBook 'Book7', 'Author6', 'Desc for book7', 1500, 26, 10, 'Book7-image'


------ GetAllBooks

alter proc usp_GetAllBooks
AS
BEGIN
	set nocount on

    BEGIN TRY
        BEGIN TRANSACTION;
        
        --SELECT * FROM Books where ISDeleted = 0;
        SELECT * FROM Books;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;

exec usp_GetAllBooks


----- GetBookById

alter proc usp_GetBookById(
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
		if exists (select 1 from Books where BookId = @BookId and IsDeleted = 0)
		begin
			select * from Books where BookId = @BookId;
		end
		else
		begin
		set @ErrorMessage = 'Book does not exist for bookId: ' + CAST(@BookId as nvarchar(50));
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
	end try
	begin catch
		throw;
	end catch
end;

select * from Books

exec usp_GetBookById 1


---=========================================  GetBookByNames =====================================================

alter proc usp_GetBookByNames(
@Title nvarchar(max) = null,
@Author nvarchar(max) = null
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
		
		if ( (@Title is null or @Title = '') and (@Author is null or @Author = '') )
		begin
			set @ErrorMessage = 'Both Title & Author cannot be null or empty'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		else if (@Title is null or @Title = '')
		begin
			if exists (select 1 from Books where Author = @Author and IsDeleted = 0)
			begin
				select * from Books where Author = @Author;
			end
			else
			begin
			set @ErrorMessage = 'Book does not found for Author: ' + @Author;
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else if (@Author is null or @Author = '')
		begin
			if exists (select 1 from Books where Title = @Title and IsDeleted = 0)
			begin
				select * from Books where Title = @Title;
			end
			else
			begin
			set @ErrorMessage = 'Book does not found for Title: ' + @Title;
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else if exists (select 1 from Books where Title = @Title and Author = @Author and IsDeleted = 0)
		begin
			select * from Books where Title = @Title and Author = @Author;
		end
		else
		begin
		set @ErrorMessage = 'Book does not exist for Title: ' + @Title + ' and/or Author: ' + @Author;
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

	end try
	begin catch
		throw;
	end catch
end;

select * from Books

exec usp_GetBookByNames @Title = 'Book1', @Author = 'Author1'






------ UpdateBook


alter proc usp_UpdateBook(
@BookId int,
@Title nvarchar(max),
@Author nvarchar(max),
@Description nvarchar(max),
@OriginalPrice int,
@DiscountPercentage int,
@Quantity int,
@Image nvarchar(max)
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
		if exists (select 1 from Books where BookId = @BookId and IsDeleted = 0)
		begin
			declare @Update datetime = getdate();
			update Books set Title = @Title, Author	= @Author, Description = @Description, OriginalPrice = @OriginalPrice,
				DiscountPercentage = @DiscountPercentage, Quantity = @Quantity, Image = @Image, UpdatedAt = @Update
				where BookId = @BookId;
			
			select * from Books where BookId = @BookId;
		end 
		else
		begin
			set @ErrorMessage = 'Book does not exist for book id: ' + CAST(@BookId as nvarchar(50));
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
	end try
	begin catch
		throw;
	end catch
end;

select * from Books

exec usp_UpdateBook 2, 'Book-2', 'Author-2', 'Desc for book2', 1650, 12, 10, 'Book2-image'


----- DeleteBook

alter proc usp_DeleteBook(
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
		if exists (select 1 from Books where BookId = @BookId and IsDeleted = 0)
		begin
			declare @Update datetime = getdate();
			update Books set Quantity = 0, IsDeleted = 1, UpdatedAt = @Update
				where BookId = @BookId;
		end 
		else
		begin
			set @ErrorMessage = 'Book does not exist for book id: ' + CAST(@BookId as nvarchar(50));
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
	end try
	begin catch
		throw;
	end catch
end;

select * from Books

exec usp_DeleteBook 2


------------------------------- Review Task  ----------------------------

-- 1) Find the book using any two columns of table.

create or alter proc usp_Book_ByTitleAndPrice(
@Title nvarchar(max),
@Price int
)
as
begin
	--set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	

		if @Title is null or
			@Price is null 
		begin
			set @ErrorMessage = 'Please provide all the parameters';
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end

		if exists (select 1 from Books where Title = @Title and Price <= @Price)
		begin
			select * from Books where Title = @Title and Price <= @Price
		end
		else
		--if (@@ROWCOUNT = 0) 
		begin
			set @ErrorMessage = 'Books did not found for Title: ' + @Title + ' & with price less than or equals to: ' + CAST(@Price as nvarchar(50));
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end

		end try
	begin catch
		throw;
	end catch
end;

exec usp_Book_ByTitleAndPrice @Title = 'Book5', @Price = 1500
exec usp_Book_ByTitleAndPrice @Title = 'Book5', @Price = 1200
exec usp_Book_ByTitleAndPrice @Title = 'Book5', @Price = 100


--------------------------

--2)Find the data using bookid, if it exst update the data else insert the new book record.

create or alter proc usp_Insert_Update_Book(
@BookId int,
@Title nvarchar(max),
@Author nvarchar(max),
@Description nvarchar(max),
@OriginalPrice int,
@DiscountPercentage int,
@Quantity int,
@Image nvarchar(max)
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

		if @BookId is null or @BookId = 0 or
			@Title is null or
			@Author is null or
			@Description is null or
			@OriginalPrice is null or
			@DiscountPercentage is null or
			@Quantity is null or
			@Image is null 
		begin
			set @ErrorMessage = 'Please provide all the parameters';
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end

		if exists (select 1 from Books where Title = @Title and Author = @Author and IsDeleted = 0)
		begin
			set @ErrorMessage = 'Book already exist with Title: ' + @Title + ' and Author: ' + @Author;
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end
		else
		begin

			if not exists(select 1 from Books where BookId = @BookId)
			begin
				insert into Books(Title, Author, Description, OriginalPrice, DiscountPercentage, Quantity, Image)
					values (@Title, @Author, @Description, @OriginalPrice, @DiscountPercentage, @Quantity, @Image)
				
				set @BookId = SCOPE_IDENTITY();
				select * from Books where BookId = @BookId;
				return;
			end 
			else 
			begin
				declare @Update datetime = getdate();
				update Books set Title = @Title, Author	= @Author, Description = @Description, OriginalPrice = @OriginalPrice,
					DiscountPercentage = @DiscountPercentage, Quantity = @Quantity, Image = @Image, UpdatedAt = @Update, IsDeleted = 0
					where BookId = @BookId;
			
				select * from Books where BookId = @BookId;
			end 

		end

	end try
	begin catch
		throw;
	end catch
end;

select * from Books
update Books set Rating = 0 where BookId = 1

exec usp_Insert_Update_Book @BookId	= 1, @Title = 'Book-1' , @Author = 'Author1', @Description = 'Desc for book', 
		@OriginalPrice = 2000, @DiscountPercentage = 10, @Quantity = 20, @Image = 'Book1-image'

------------------

-- 3) Display wishlist or cart details alongwith the user who has added it

create or alter proc usp_CartDetilsWithUser
as
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try
		
		select c.CartId, c.BookId, c.Title, c.Author, c.Image, c.Quantity,
				c.OriginalBookPrice, c.FinalBookPrice, 
				u.UserId, u.FullName, u.Email, u.Mobile
				from Carts c join Users u on c.UserId = u.UserId

	end try
	begin catch
		set @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		--throw;
	end catch
end;

select * from Users
select * from Carts

exec usp_CartDetilsWithUser






