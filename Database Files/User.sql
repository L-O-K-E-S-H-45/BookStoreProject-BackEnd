
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

update Users set IsDeleted = 0 where UserId = 3
update Books set IsDeleted = 0 where BookId = 4

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
			set @ErrorMessage = 'User already exists for email id: ' + o@Email;
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
	end try
	begin catch
		throw;
	end catch
end;

exec usp_InsertUser 'Allen', 'allen123@gmail.com', 'Allen@123', 7678123456;


exec usp_InsertUser 'Miller', 'miller123@gmail.com', 'Miller@123', 7654321234
exec usp_InsertUser 'Jerry', 'jerry123@gmail.com', 'Jerry@123', 7654321234


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

		if @Email is null or @Email = '' or
			@Password is null or @Password = ''
		BEGIN
            SET @ErrorMessage = 'Please provide Email and Password';
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
        END

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
















