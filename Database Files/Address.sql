
use BookStoreDB

create table Addresses(
AddressId int primary key identity,
UserId int foreign key references Users(UserId),
FullName nvarchar(100) not null,
Mobile bigint not null,
Address nvarchar(max) not null,
City nvarchar(100) not null,
State nvarchar(100) not null,
Type nvarchar(50) not null,
constraint chk_addressmobile check(Mobile >=6000000000 and Mobile<= 9999999999),
constraint chk_addresstype check(Type in ('Home', 'Work', 'Other'))
);

select * from Addresses



-----------------------------------------------  AddAddress  ---------------------------------------------


create or alter procedure usp_AddAddress(
@UserId int,
@FullName nvarchar(100),
@Mobile bigint,
@Address nvarchar(max),
@City nvarchar(100),
@State nvarchar(100),
@Type nvarchar(50)
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
		if @UserId is null or
			@FullName is null or
			@Mobile is null or
			@Address is null or
			@City is null or
			@State is null or
			@Type is null
			begin
				set @ErrorMessage = 'Please provide all the parameters';
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
			if (@Type COLLATE Latin1_General_BIN not in ('Home', 'Work', 'Other'))
			begin
				set @ErrorMessage = 'Please provide correct adddress type';
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
			if exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
			begin
				if exists (select 1 from Addresses where UserId = @UserId and Type = @Type)
				begin
					set @ErrorMessage = 'Address already exist for user id: ' + CAST(@UserId as nvarchar(50)) + ' with address type: ' + @Type;
					RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
				end

				insert into Addresses (UserId, FullName, Mobile, Address, City, State, Type)
					values (@UserId, @FullName, @Mobile, @Address, @City, @State, @Type COLLATE Latin1_General_BIN);

				declare @AddressId int = SCOPE_IDENTITY();
				select * from Addresses where AddressId = @AddressId;
			end
			else
			begin
				set @ErrorMessage = 'User does not exist for user id: ' + CAST(@UserId as nvarchar(50));
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
	end try
	begin catch
		throw;
	end catch
end;

select * from Addresses

--truncate table Address

exec usp_AddAddress @UserId = 2, @FullName = 'Jack j', @Mobile = 7654321123,
		@Address = '12A/4th Main, 3rd Phase Banashankari', @City = 'Bangalore', @State = 'Karnataka', @Type = 'Work' 


----------------------------------------------  GetAllAddress  ---------------------------------------

create or alter procedure usp_GetAllAddress
as 
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	

		select * from Addresses;
		
	end try
	begin catch
		SET @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		throw;
	end catch
end;

select * from Addresses

exec usp_GetAllAddress


--------------------------------------  GetAddressByUser  ------------------------------------

create or alter proc usp_GetAddressByUser(
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
			
			select * from Addresses where UserId = @UserId;

		end
		else
		begin
			set @ErrorMessage = 'User does not exist for user id: ' + CAST(@UserId as nvarchar(50));
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		
	end try
	begin catch
		throw;
	end catch
end;

exec usp_GetAddressByUser @UserId = 2

select * from Addresses


---------------------------------------  GetAddressById  ---------------------------------------

create or alter proc usp_GetAddressById(
@UserId int,
@AddressId int
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

		if (@UserId is null or @AddressId is null)
		begin
			set @ErrorMessage = 'UserId or AddressId cannot be null'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		if not exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin
			set @ErrorMessage = 'User does not exist for user id: ' + CAST(@UserId as nvarchar(50));
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		if exists (select 1 from Addresses where UserId = @UserId and AddressId = @AddressId)
		begin
			
			select * from Addresses where UserId = @UserId and AddressId = @AddressId;

		end
		else
		begin
			set @ErrorMessage = 'Address does not found for user id: '+ CAST(@UserId as nvarchar(50)) + ' with address id: ' + CAST(@AddressId as nvarchar(50));
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		
	end try
	begin catch
		throw;
	end catch
end;

exec usp_GetAddressById @UserId = 2, @AddressId = 1

select * from Addresses
select * from Users

----------------------------------------------  UpdateAddress  -------------------------------------

create or alter proc usp_UpdateAddress(
@UserId int,
@AddressId int,
@FullName nvarchar(100),
@Mobile bigint,
@Address nvarchar(max),
@City nvarchar(100),
@State nvarchar(100),
@Type nvarchar(50)
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
		if @UserId is null or
			@AddressId is null or
			@FullName is null or
			@Mobile is null or
			@Address is null or
			@City is null or
			@State is null or
			@Type is null
			begin
				set @ErrorMessage = 'Please provide all the parameters';
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end

			if (@Type COLLATE latin1_general_bin not in ('Home', 'Work', 'Other'))
			begin
				set @ErrorMessage = 'Please provide correct adddress type';
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end

			IF not EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId and IsDeleted = 0)
			begin
				SET @ErrorMessage = 'User does not exist for user id: ' + CAST(@UserId as nvarchar(50));
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end

			if exists (select 1 from Addresses where UserId = @UserId and AddressId = @AddressId)
			begin
				
				if exists (select 1 from Addresses where UserId = @UserId and Type = @Type and AddressId != @AddressId)
				begin 
					SET @ErrorMessage = 'Address already exists for user id: ' + CAST(@UserId as nvarchar(50)) + ' with address type: ' + @Type;
					RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
				end

				update Addresses
				set FullName = @FullName, 
				    Mobile = @Mobile, 
					Address = @Address, 
					City = @City, 
					State = @State, 
					Type = @Type
				where AddressId = @AddressId;

				select * from Addresses where AddressId = @AddressId;
			end
			else
			begin
				set @ErrorMessage = 'Address does not found for user id: '+ CAST(@UserId as nvarchar(50)) + ' with address id: ' + CAST(@AddressId as nvarchar(50));
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
	end try
	begin catch
		set @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
	end catch
end;

select * from Addresses

exec usp_UpdateAddress @UserId = 2, @AddressId = 4, @FullName = 'Jack', @Mobile = 8765432112, 
		@Address = '5B/4th Main, 1st Phase Koramangala', @City = 'Bangalore', @State = 'Karnataka', @Type = 'Home';


---------------------------------------------  DeleteAddress  --------------------------------------

create or alter proc usp_DeleteAddress(
@UserId int,
@AddressId int
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

		if (@UserId is null or @AddressId is null)
		begin
			set @ErrorMessage = 'UserId or AddressId cannot be null'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		IF not EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId and IsDeleted = 0)
			begin
				SET @ErrorMessage = 'User does not exist for user id: ' + CAST(@UserId as nvarchar(50));
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		if exists (select 1 from Addresses where UserId = @UserId and AddressId = @AddressId)
		begin
			 
			delete from Addresses where UserId = @UserId and AddressId = @AddressId;

			if (@@ROWCOUNT != 1)
			begin
				set @ErrorMessage = 'Address did not deleted';
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else
		begin
			set @ErrorMessage = 'Address does not found for user id: '+ CAST(@UserId as nvarchar(50)) + ' with address id: ' + CAST(@AddressId as nvarchar(50));
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
	end try
	begin catch
		throw;
	end catch
end;

select * from Addresses

exec usp_DeleteAddress @UserId = 2, @AddressId = 4














