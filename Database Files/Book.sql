
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

















