
create database BookStoreDB

use BookStoreDB

create table Users(
UserId int primary key identity,
FullName nvarchar(100) not null,
Email nvarchar(100) not null unique,
Password nvarchar(max) not null,
Mobile bigint not null,
IsDeleted bit default 0,
constraint chk_email check(Email like '%@gmail.com'),
constraint chk_mobile check(Mobile >=6000000000 and Mobile<= 9999999999),
CreatedAt datetime default getdate(),
UpdatedAt datetime default getdate()
)

--drop table Users

select * from Users

----- Insert

alter procedure usp_InsertUser(
@FullName nvarchar(100),
@Email nvarchar(100),
@Password nvarchar(max),
@Mobile bigint
)
as 
begin
	set nocount on
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try
		-- Validate FullName
     --   IF @FullName IS NULL OR LEN(@FullName) < 3 OR LEN(@FullName) > 50 OR
     --      @FullName COLLATE Latin1_General_BIN NOT LIKE '[A-Z][a-z0-9]%' OR 
		   --(CHARINDEX(' ', @FullName) > 0 AND @FullName COLLATE Latin1_General_BIN NOT LIKE '[A-Z][a-z0-9]* [A-Z][a-z0-9]')
     --   BEGIN
     --       SET @ErrorMessage = 'Invalid FullName. FullName must start with an uppercase letter followed by at least two lowercase letters or digits. An optional second word must start with an uppercase letter and can have lowercase letters or digits, and the length must be between 3 and 50 characters.';
     --       RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
     --       RETURN;
     --   END
		 -- Validate email
        IF @Email IS NULL OR LEN(@Email) < 13 OR LEN(@Email) > 50 OR 
           @Email COLLATE Latin1_General_BIN NOT LIKE '[a-z][a-z0-9]%@gmail.com' OR CHARINDEX('@', @Email) <= 3
        BEGIN
            SET @ErrorMessage = 'Invalid email format. Email must be in lower case, start with an alphabet, 
				and end with @gmail.com, and be between 13 and 50 characters.';
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
            RETURN;
        END
		-- Validate password
        --IF @Password IS NULL OR
        --   @Password COLLATE Latin1_General_BIN NOT LIKE '%[A-Z]%' OR @Password COLLATE Latin1_General_BIN NOT LIKE '%[a-z]%' OR
        --   @Password COLLATE Latin1_General_BIN NOT LIKE '%[0-9]%' OR @Password COLLATE Latin1_General_BIN NOT LIKE '%[^a-zA-Z0-9]%' OR
        --   LEN(@Password) < 8
        --BEGIN
        --    SET @ErrorMessage = 'Invalid password. Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.';
        --    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
        --    RETURN;
        --END
		-- Validate Mobile
        IF @Mobile IS NULL OR @Mobile NOT BETWEEN 6000000000 AND 9999999999
        BEGIN
            SET @ErrorMessage = 'Invalid mobile number. Mobile number must start with a digit between 6 and 9 and contain exactly 10 digits.';
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
            RETURN;
        END

		if not exists(select 1 from Users where Email = @Email)
		begin
			insert into Users(FullName, Email, Password, Mobile) 
				values (@FullName, @Email, @Password, @Mobile);

			select * from Users where Email = @Email;
			--if (@@ROWCOUNT = 0)
			--begin
			--set @ErrorMessage = 'Failed to insert user';
			--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			--end
		end
		else if exists (select 1 from Users where Email = @Email and IsDeleted = 1)
		begin
			declare @Update datetime = getdate();
			update Users set FullName = @FullName, Password = @Password, Mobile = @Mobile, IsDeleted = 0, UpdatedAt = @Update 
				where Email = @Email;

			select * from Users where Email = @Email
		end
		else
		begin
			set @ErrorMessage = 'User already exists for email id: ' + @Email;
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
	end try
	begin catch
		throw;
	end catch
end;

exec usp_InsertUser 'Scott', 'scott123@gmail.com', 'Scott@123', 7678123456;


exec usp_InsertUser 'Scott', 'scott123@gmail.com', 'Scott@123', 7654321234


select * from Users

truncate table Users

------ GetAllUsers

alter proc usp_GetAllUsers
AS
BEGIN
	set nocount on
    PRINT 'Executing usp_GetAllUsers';

    BEGIN TRY
        BEGIN TRANSACTION;
        
        --SELECT * FROM Users where IsDeleted = 0;
        SELECT * FROM Users;

        -- Commit transaction if no errors
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback transaction in case of error
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

    PRINT 'usp_GetAllUsers execution completed';
END;


exec usp_GetAllUsers


------ UserLogin

alter proc usp_UserLogin(
@Email nvarchar(100),
@Password nvarchar(max)
)
as
begin
	set nocount on
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try
		 -- Check if user exists
        IF EXISTS (SELECT 1 FROM Users WHERE Email COLLATE Latin1_General_BIN = @Email and IsDeleted = 0)
        BEGIN
            -- Validate user credentials
            IF EXISTS (SELECT 1 FROM Users WHERE Email COLLATE Latin1_General_BIN = @Email AND Password COLLATE Latin1_General_BIN = @Password)
            BEGIN
                SELECT * from Users where Email COLLATE Latin1_General_BIN = @Email AND Password COLLATE Latin1_General_BIN = @Password
            END
            ELSE
            BEGIN
                SET @ErrorMessage = 'Failed to login user because of invalid credentials';
                RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
            END
        END
        ELSE
        BEGIN
            SET @ErrorMessage = 'User does not exist for email id: ' + @Email;
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
        END
	end try
	begin catch
		throw;
	end catch
end;

select * from Users
select * from Users where Email COLLATE Latin1_General_BIN = 'smith123@gmail.com' and Password COLLATE Latin1_General_BIN = 'smith@123'
select * from Users where Email = 'smith123@gmail.com' and Password = 'smith@123'

exec usp_UserLogin 'smith123@gmail.com', 'smith@123'
exec usp_UserLogin 'smith123@gmail.com', 'Smith@123'
exec usp_UserLogin 'Smith123@gmail.com', 'Smith@123'


----- GetUserByEmail

alter proc usp_GetUserByEmail(
@Email nvarchar(100)
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
		IF EXISTS (SELECT 1 FROM Users WHERE Email COLLATE Latin1_General_BIN = @Email and IsDeleted = 0)
			select * from Users where  Email COLLATE Latin1_General_BIN = @Email;
			else
			begin
                SET @ErrorMessage = 'User does not exist for email id: ' + @Email;
                RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
            end
	end try
	begin catch
		throw;
	end catch
end;

select * from Users

exec usp_GetUserByEmail 'smith123@gmail.com'


--------- ResetPassword

alter  proc usp_ResetPassword(
@Email nvarchar(100),
@Password nvarchar(max)
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
		if  exists (select 1 from Users where Email COLLATE Latin1_General_BIN = @Email and IsDeleted = 0)
		begin
			declare @Update datetime = getdate();
			update Users set Password = @Password, UpdatedAt = @Update 
				where Email COLLATE Latin1_General_BIN = @Email;
		end
		else
		begin
			SET @ErrorMessage = 'User does not exist for email id: ' + @Email;
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
	end try
	begin catch
		set @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		--throw;
	end catch
end;

select * from Users

exec usp_ResetPassword 'smith123@gmail.com', 'Smith@123'


-------- GetUserByUserId

create proc usp_GetUserByUserId(
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
		IF EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId and IsDeleted = 0)
			select * from Users where  UserId = @UserId;
		else
		begin
            SET @ErrorMessage = 'User does not exist for user id: ' + CAST(@UserId as nvarchar(50));
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
        end
	end try
	begin catch
		throw;
	end catch
end;

exec usp_GetUserByUserId 4


----- UpdateUser

alter proc usp_UpdateUser(
@UserId int,
@FullName nvarchar(100),
@Email nvarchar(100),
@Password nvarchar(max),
@Mobile bigint
)
as 
begin
	set nocount on
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try
		-- Validate FullName
     --   IF @FullName IS NULL OR LEN(@FullName) < 3 OR LEN(@FullName) > 50 OR
     --      @FullName COLLATE Latin1_General_BIN NOT LIKE '[A-Z][a-z0-9]%' OR 
		   --(CHARINDEX(' ', @FullName) > 0 AND @FullName COLLATE Latin1_General_BIN NOT LIKE '[A-Z][a-z0-9]* [A-Z][a-z0-9]')
     --   BEGIN
     --       SET @ErrorMessage = 'Invalid FullName. FullName must start with an uppercase letter followed by at least two lowercase letters or digits. An optional second word must start with an uppercase letter and can have lowercase letters or digits, and the length must be between 3 and 50 characters.';
     --       RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
     --       RETURN;
     --   END
		 -- Validate email
        IF @Email IS NULL OR LEN(@Email) < 13 OR LEN(@Email) > 50 OR 
           @Email COLLATE Latin1_General_BIN NOT LIKE '[a-z][a-z0-9]%@gmail.com' OR CHARINDEX('@', @Email) <= 3
        BEGIN
            SET @ErrorMessage = 'Invalid email format. Email must be in lower case, start with an alphabet, 
				and end with @gmail.com, and be between 13 and 50 characters.';
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
            RETURN;
        END
		-- Validate password
    --    IF @Password IS NULL OR
    --       @Password COLLATE Latin1_General_BIN NOT LIKE '%[A-Z]%' OR @Password COLLATE Latin1_General_BIN NOT LIKE '%[a-z]%' OR
    --       @Password COLLATE Latin1_General_BIN NOT LIKE '%[0-9]%' OR @Password COLLATE Latin1_General_BIN NOT LIKE '%[^a-zA-Z0-9]%' OR
    --       LEN(@Password) < 8
    --    BEGIN
    --        SET @ErrorMessage = 'Invalid password. Password must be at least 8 characters long and contain at least 
				--one uppercase letter, one lowercase letter, one digit, and one special character.';
    --        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
    --        RETURN;
    --    END
		-- Validate Mobile
        IF @Mobile IS NULL OR @Mobile NOT BETWEEN 6000000000 AND 9999999999
        BEGIN
            SET @ErrorMessage = 'Invalid mobile number. Mobile number must start with a digit between 6 and 9 and contain exactly 10 digits.';
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
            RETURN;
        END

		if exists(select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin
			declare @Update datetime = getdate();
			update Users set FullName = @FullName, Email = @Email, Password = @Password, Mobile = @Mobile, UpdatedAt = @Update
			where UserId = @UserId;

			select * from Users where UserId = @UserId;
		end
		else
		begin
			set @ErrorMessage = 'User does not exists for user id: ' + CAST(@UserId as nvarchar(50));
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
	end try
	begin catch
		throw;
	end catch
end;

select * from Users

exec usp_UpdateUser 2 , 'Turner', 'turner123@gmail.com', 'Turner@123', 8765432112


----- DeleteUser

alter proc usp_DeleteUser(
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
		IF EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId and IsDeleted = 0)
		begin
			declare @Update datetime = getdate();
			update Users set IsDeleted = 1, UpdatedAt = @Update where UserId = @UserId;
		end
		else if exists (SELECT 1 FROM Users WHERE UserId = @UserId and IsDeleted = 1)
		begin
			SET @ErrorMessage = 'User laready deleted for user id: ' + CAST(@UserId as nvarchar(50));
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		else
		begin
            SET @ErrorMessage = 'User does not exist for user id: ' + CAST(@UserId as nvarchar(50));
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
        end
	end try
	begin catch
		throw;
	end catch
end;

select * from Users

exec usp_DeleteUser 1

'---------------------------------------- Book --------------------------------------------'


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



----------------------------------------------------------------------

--In SQL Server, the RAISERROR statement allows you to generate error messages and control their severity and status. The parameters ErrorSeverity and ErrorStatus used in the RAISERROR function have specific meanings:

--ErrorSeverity:

--The ErrorSeverity parameter indicates the severity level of the error.
--SQL Server uses severity levels from 0 to 25 to categorize the type and severity of errors.
--Severity levels 0-10 are informational and are not typically used to indicate errors.
--Severity levels 11-16 indicate errors that can be corrected by the user.
--Level 16 is used for general errors that can be corrected by the user, such as syntax errors or permission issues.
--Severity levels 17-19 indicate more serious errors that may not be easily corrected by the user, such as resource or configuration issues.
--Severity levels 20-25 indicate system-level errors and are more serious, often requiring database or server administrator intervention.
--In the context of your stored procedure, ErrorSeverity = 16 indicates that the errors raised are general errors that the user might be able to correct (e.g., by providing correct email or password).

--ErrorStatus:

--The ErrorStatus parameter (also known as the State) is an arbitrary integer that can be used to indicate a specific error condition or state.
--It can be any value from 0 to 255.
--This parameter does not affect the behavior of the RAISERROR statement but can be used by developers to provide additional information about the error condition.
--For example, different error conditions in the same stored procedure could be assigned different ErrorStatus values to help identify the exact nature of the error when it is logged or monitored.
--In your stored procedure, ErrorStatus = 1 is simply an arbitrary value chosen to indicate a specific error condition. It could be used for logging or debugging purposes to distinguish between different errors.




