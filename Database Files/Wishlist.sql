
use BookStoreDB

create table Wishlists(
WishlistId int primary key identity,
UserId int foreign key references Users(UserId),
BookId int foreign key references Books(BookId),
Title nvarchar(max) not null,
Author nvarchar(max) not null,
Image nvarchar(max) not null,
OriginalBookPrice int not null,
FinalBookPrice int not null,
)


select * from Wishlists


-----------------------------------------------  AddBookToWishlist  ---------------------------------------

create or alter proc usp_AddBookToWishlist(
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
				
				if not exists (select 1 from Wishlists where UserId = @UserId and BookId = @BookId)
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
			
					insert into Wishlists(UserId, BookId, Title, Author, Image, OriginalBookPrice, FinalBookPrice)
						values (@UserId, @BookId, @Title, @Author, @Image, @OriginalBookPrice, @FinalBookPrice)

					declare @WishlistId int = SCOPE_IDENTITY();
					select * from Wishlists where WishlistId = @WishlistId;
					return;
				end
				else
				begin
					set @ErrorMessage = 'Book already added to wishlist, Please go to wishlist';
				end
			end
			else
			begin
				set @ErrorMessage = 'Book does not exist';
			end
		end
		else
		begin
			set @ErrorMessage = 'User does not exist';
		end

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		
	end try
	begin catch
		throw;
	end catch
end;

select * from Wishlists

exec usp_AddBookToWishlist @UserId = 2, @BookId = 5


--------------------------------------  ViewAllWhishlists  ------------------------------------

create or alter proc usp_ViewAllWhishlists
as
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	

		select * from Wishlists;
		
	end try
	begin catch
		SET @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		throw;
	end catch
end;

exec usp_ViewAllWhishlists


--------------------------------------  ViewCartByUser  ------------------------------------

create or alter proc usp_ViewWhishlistByUser(
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
			
			select * from Wishlists where UserId = @UserId;

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

exec usp_ViewWhishlistByUser @UserId = 3

select * from Wishlists

select * from Users


--------------------------------------  RemoveBookFromWhishlist  ------------------------------------

create or alter proc usp_RemoveBookFromWhishlist(
@WhishlistId int
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

		if (@WhishlistId is null)
		begin
			set @ErrorMessage = 'WhishlistId cannot be null'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		if exists (select 1 from Wishlists where WishlistId = @WhishlistId)
		begin
			
			delete from Wishlists where WishlistId = @WhishlistId;

			if (@@ROWCOUNT != 1)
			begin
				set @ErrorMessage = 'Whishlist did not deleted';
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else
		begin
			set @ErrorMessage = 'Whishlist does not exist for wishlistId: ' + CAST(@WhishlistId as nvarchar(50));
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		
	end try
	begin catch
		throw;
	end catch
end;

select * from Wishlists

exec usp_RemoveBookFromWhishlist @WhishlistId = 3











