
use BookStoreDB

create table Orders(
	OrderId int primary key identity,
	UserId int foreign key references Users(UserId),
	--CartId int foreign key references Carts(CartId),
	BookId int foreign key references Books(BookId),
	Title nvarchar(max) not null,
	Author nvarchar(max) not null,
	Image nvarchar(max) not null,
	Quantity int check(Quantity >= 1),  
	TotalOriginalBookPrice int not null,
	TotalFinalBookPrice int not null,
	OrderDateTime datetime default getdate(),
	IsDeleted bit default 0
)

--drop table Orders

select * from Orders

------------------------------------  PlaceOrder  --------------------------------

create or alter proc usp_PlaceOrder (
@UserId int,
@CartId int
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

		-- validate inputs
		if (@UserId is null or @UserId  = 0 or @CartId is null or @CartId = 0)
		begin
			set @ErrorMessage = 'UserId or CartId cannot be null or zero'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end
		-- validate user
		if not exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin
			set @ErrorMessage = FORMATMESSAGE('User does not exist for id: %d', @UserId);
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end
		-- validate cart
		if not exists (select 1 from Carts where UserId = @UserId and CartId = @CartId)
		begin
			set @ErrorMessage = FORMATMESSAGE('Cart does not exist for user id: %d with cart id: %d', @UserId, @CartId);
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end

		declare @BookId int,
				@Title nvarchar(max),
				@Author nvarchar(max),
				@Image nvarchar(max),
				@Quantity int,
				@TotalOriginalBookPrice int,
				@TotalFinalBookPrice int
		
		select @BookId = BookId,
				@Title = Title,
				@Author = Author,
				@Image = Image,
				@Quantity = Quantity,
				@TotalOriginalBookPrice = OriginalBookPrice,
				@TotalFinalBookPrice = FinalBookPrice
				from Carts where CartId = @CartId and UserId = @UserId;

		-- validate book quantity 
		if not exists (select 1 from Books where BookId = @BookId and Quantity >= @Quantity)
		begin
			set @ErrorMessage = 'Books are out of stock';
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end
		else
		begin
			-- insert into Orders 
			insert into Orders(UserId, BookId, Title, Author, Image, Quantity, TotalOriginalBookPrice, TotalFinalBookPrice)
				values (@UserId, @BookId, @Title, @Author, @Image, @Quantity, @TotalOriginalBookPrice, @TotalFinalBookPrice);
				

			-- Reduce book quantity in original Books table
			update Books set Quantity = Quantity - @Quantity 
				where BookId = @BookId;

			-- Before deleting cart, Remove cart foreign key from Order
			--update Orders set CatId = null where CartId = @CartId;

			-- remove order placed item from cart
			delete from Carts where CartId = @CartId;
			
			declare @OrderId int = SCOPE_IDENTITY();
			select * from Orders where OrderId = @OrderId;
		end
		
	end try
	begin catch
		set @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
	end catch
end;	

select * from Users
select * from Carts
select* from Books

select * from Orders

exec usp_PlaceOrder @UserId = 2, @CartId = 2


------------------------  ViewAllOrders  ---------------------------------

create or alter proc usp_ViewAllOrders
as
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	

		select * from Orders;
		
	end try
	begin catch
		SET @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		throw;
	end catch
end;

exec usp_ViewAllOrders


--------------------------------------  ViewOrdersByUser  ------------------------------------

create or alter proc usp_ViewOrdersByUser(
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

		if (@UserId is null or @UserId = 0)
		begin
			set @ErrorMessage = 'UserId cannot be null or zero'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		if exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin
			select * from Orders where UserId = @UserId and IsDeleted = 0;
			if (@@ROWCOUNT = 0)
			begin
				set @ErrorMessage = FORMATMESSAGE('Orders list is empty for user id: %d', @UserId);
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else
		begin
			set @ErrorMessage = FORMATMESSAGE('User does not exist for user id: %d', @UserId);
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		
	end try
	begin catch
		throw;
	end catch
end;

exec usp_ViewOrdersByUser @UserId = 2

select * from Orders
select * from Carts

--delete from Orders where OrderId = 1

--------------------------------------  ViewOrderById  ------------------------------------

create or alter proc usp_ViewOrderById(
@OrderId int
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

		if (@OrderId is null or @OrderId = 0)
		begin
			set @ErrorMessage = 'OrderId cannot be null or zero'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		if exists (select 1 from Orders where OrderId = @OrderId and IsDeleted = 0)
		begin

			select * from Orders where OrderId = @OrderId;

		end
		else
		begin
			set @ErrorMessage = FORMATMESSAGE('Order is not found for order id: %d', @OrderId);
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		
	end try
	begin catch
		throw;
	end catch
end;

select * from Orders

exec usp_ViewOrderById @OrderId = 3

--------------------------------------  CancelOrder  ------------------------------------

create or alter proc usp_CancelOrder(
@UserId int,
@OrderId int
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

		if (@OrderId is null or @OrderId = 0 or @OrderId is null or @OrderId = 0)
		begin
			set @ErrorMessage = 'UserId or OrderId cannot be null or zero'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		-- validate user
		if not exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin
			set @ErrorMessage = FORMATMESSAGE('User does not exist for id: %d', @UserId);
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end

		-- check order exists or not
		if exists (select 1 from Orders where UserId = @UserId and OrderId = @OrderId and IsDeleted = 0)
		begin

			-- declare & store BookId & Quantity from canceling order
			declare @BookId int,
					@Quantity int
			select @BookId = BookId, @Quantity = Quantity from Orders
				where OrderId = @OrderId;

			-- delete from orders
			update Orders set IsDeleted = 1
				where OrderId = @OrderId;

			-- Restore book quantities back to original book
			update Books set Quantity = @Quantity
				where BookId = @BookId;

			select * from Orders where OrderId = @OrderId;
		end
		else
		begin
			set @ErrorMessage = FORMATMESSAGE('Order is not found for order id: %d', @OrderId);
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		
	end try
	begin catch
		throw;
	end catch
end;

select * from Orders

exec usp_CancelOrder @UserId = 2, @OrderId = 4








