
use BookStoreDB

create table Admin(
AdminId int primary key identity,
FullName nvarchar(100) not null,
Email nvarchar(100) not null unique,
Password nvarchar(max) not null,
Mobile bigint not null,
IsDeleted bit default 0,
constraint chk_admin_email check(Email like '%@gmail.com'),
constraint chk_admin_mobile check(Mobile >=6000000000 and Mobile<= 9999999999),
CreatedAt datetime default getdate(),
UpdatedAt datetime default getdate()
)


select * from Admin

insert into Admin(FullName, Email, Password, Mobile)
	values ('Admin', 'admin123@gmail.com', 'Admin@123', 9988776655);


------------------  AdminLogin  --------------------------

create or alter proc usp_AdminLogin(
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

		 -- Check if admin exists
        IF EXISTS (SELECT 1 FROM Admin WHERE Email COLLATE Latin1_General_BIN = @Email and IsDeleted = 0)
        BEGIN
            -- Validate user credentials
            IF EXISTS (SELECT 1 FROM Admin WHERE Email COLLATE Latin1_General_BIN = @Email AND Password COLLATE Latin1_General_BIN = @Password)
            BEGIN
                SELECT AdminId, FullName, Email, Mobile from Admin where Email COLLATE Latin1_General_BIN = @Email AND Password COLLATE Latin1_General_BIN = @Password
            END
            ELSE
            BEGIN
                SET @ErrorMessage = 'Failed to login admin because of invalid credentials';
                RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
            END
        END
        ELSE
        BEGIN
            SET @ErrorMessage = 'Admin does not exist for email id: ' + @Email;
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
        END
	end try
	begin catch
		throw;
	end catch
end;


select * from Admin

exec usp_AdminLogin @Email = 'admin123@gmail.com', @Password = Admin@123






