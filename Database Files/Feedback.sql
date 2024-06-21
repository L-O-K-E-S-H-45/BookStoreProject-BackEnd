
use BookStoreDB

create table Feedback(
FeedbackId int primary key identity,
UserId int foreign key references Users(UserId),
UserName nvarchar(100) not null,
BookId int foreign key references Books(BookId),
Rating int not null,
constraint chk_feedback_rating check(Rating between 1 and 5),
Review nvarchar(max) not null,
CreatedAt datetime default getdate(),
UpdatedAt datetime default getdate()
)
 
--drop table Feedback
select * from Feedback

---------------------------------------  GiveFeedback  -----------------------------

create or alter proc usp_GiveFeedback(
@UserId int,
@BookId int,
@Rating int,
@Review nvarchar(max)
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

		if (@UserId is null or @UserId = 0 or
			@BookId is null or @BookId = 0 or
			@Rating is null or 
			@Review is null or @Review = ''
			)
		begin
			set @ErrorMessage = 'Parameters cannot be null or zero or empty'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end

		--IF TRY_CAST(@Rating AS INT) IS NULL OR NOT (@Rating BETWEEN 1 AND 5)
		if not (@Rating between 1 and 5)
		begin
			set @ErrorMessage = 'Rating is either not an integer or not within the range 1 to 5';
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end 

		if exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin

			if exists (select 1 from Books where BookId = @BookId and IsDeleted = 0)
			begin
				
				if exists (select 1 from Orders where UserId = @UserId and BookId = @BookId and IsDeleted = 0)
				begin

					if exists (select 1 from Feedback where UserId = @UserId and BookId = @BookId)
					begin
						set @ErrorMessage = FORMATMESSAGE('You have already given rating for this book id: %d', @BookId);
						RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
						return;
					end 

					declare @UserName nvarchar(100);

					select @UserName = FullName
							from Users where UserId = @UserId;
			
					insert into Feedback(UserId, UserName, BookId, Rating, Review)
						values (@UserId, @UserName, @BookId, @Rating, @Review)

					declare @FeedbackId int = SCOPE_IDENTITY();

					-- update ratings to original Book
					update Books set Rating = ((Rating*RatingCount)+@Rating)/(RatingCount+1),
							RatingCount=RatingCount+1
							where BookId = @BookId;

					select * from Feedback where FeedbackId = @FeedbackId;
					return;
				end
				else
				begin
					set @ErrorMessage = FORMATMESSAGE('Sorry! You are not allowed to review this book id: %d, since you have not purchaged it on BookStore', @BookId);
					--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
				end
			end
			else
			begin
				set @ErrorMessage = FORMATMESSAGE('Book does not exist for id: %d', @BookId);
				--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else
		begin
			set @ErrorMessage = FORMATMESSAGE('User does not exist for id: %d', @UserId);
			--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		
	end try
	begin catch
		throw;
	end catch
end;

select * from Feedback
select * from Books
select * from Users
select * from Orders

exec usp_GiveFeedback @UserId = 3, @BookId = 6, @Rating = 5, @Review = 'This is good book'

--truncate table Feedback


------------------------- Triger for update or delete Feedback to update Rating to Book ----------------------

alter TRIGGER trg_UpdateRatingInBooks
ON Feedback
AFTER UPDATE, DELETE
AS
BEGIN
    DECLARE @BookId INT;
    -- For DELETE operation, the BookId comes from the DELETED table
    -- For UPDATE operation, handle both old and new values (assuming BookId doesn't change during update)
    SELECT TOP 1 @BookId = BookId FROM DELETED;
    
    IF @BookId IS NULL
    BEGIN
        -- If no rows were deleted, get the BookId from the INSERTED table (update operation case)
        SELECT TOP 1 @BookId = BookId FROM INSERTED;
    END
    
    IF @BookId IS NOT NULL
    BEGIN
        DECLARE @Rating DECIMAL(2,1),
                @RatingCount INT;
        
        -- Calculate the new average rating and rating count for the book
        SELECT @Rating = AVG(Rating), @RatingCount = COUNT(*)
        FROM Feedback
        WHERE BookId = @BookId;

		if @Rating is null
		set @Rating = 0.0

        -- Update the Books table with the new rating and rating count
        UPDATE Books 
        SET Rating = @Rating, RatingCount = @RatingCount
        WHERE BookId = @BookId;
    END
	else
	Raiserror('Rating did not updated in book', 16, 1);

END;

select * from Books
select * from Feedback


----------------------------------------  EditReview  --------------------------------

create or alter proc usp_EditReview(
@UserId int,
@FeedbackId int,
@Rating int,
@Review nvarchar(max)
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

		if (@UserId is null or @UserId = 0 or
			@FeedbackId is null or @UserId = 0 or
			@Rating is null or 
			@Review is null or @Review = ''
			)
		begin
			set @ErrorMessage = 'Parameters cannot be null or zero or empty'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end

		if not (@Rating between 1 and 5)
		begin
			set @ErrorMessage = 'Rating is not within the range 1 to 5';
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end 

		if exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin

			if exists (select 1 from Feedback where UserId = @UserId and FeedbackId = @FeedbackId)
			begin

				update Feedback set Rating = @Rating, Review = @Review, UpdatedAt = GETDATE()
					where FeedbackId = @FeedbackId

				select * from Feedback where FeedbackId = @FeedbackId;
				return;
			end
			else
			begin
				set @ErrorMessage = FORMATMESSAGE('Feedback does not exist for user id: %d with feedback id: %d',@UserId, @FeedbackId);
				--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else
		begin
			set @ErrorMessage = FORMATMESSAGE('User does not exist for id: %d', @UserId);
			--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		
	end try
	begin catch
		throw;
	end catch
end;

select * from Feedback
select * from Books
select * from Carts
select * from Orders

exec usp_EditReview @UserId = 3, @FeedbackId = 1, @Rating = 4,  @Review = 'Good book'


------------------------------------ DeleteReview  ----------------------------------

create or alter proc usp_DeleteReview(
@UserId int,
@FeedbackId int
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

		if (@UserId is null or @UserId = 0 or
			@FeedbackId is null or @UserId = 0 
			)
		begin
			set @ErrorMessage = 'Parameters cannot be null or zero or empty'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			return;
		end

		if exists (select 1 from Users where UserId = @UserId and IsDeleted = 0)
		begin

			if exists (select 1 from Feedback where UserId = @UserId and FeedbackId = @FeedbackId)
			begin

				delete from Feedback
					where FeedbackId = @FeedbackId

				return;
			end
			else
			begin
				set @ErrorMessage = FORMATMESSAGE('Feedback does not exist for user id: %d with feedback id: %d',@UserId, @FeedbackId);
				--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
			end
		end
		else
		begin
			set @ErrorMessage = FORMATMESSAGE('User does not exist for id: %d', @UserId);
			--RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		
	end try
	begin catch
		throw;
	end catch
end;

select * from Feedback
select * from Books
select * from Carts
select * from Orders

exec usp_DeleteReview @UserId = 3, @FeedbackId = 1


-----------------------------  ViewAllFeedbacks  --------------------------

create or alter proc usp_ViewAllFeedbacks
as
begin
	set nocount on;
	declare @ErrorMessage nvarchar(max);
	declare @ErrorStatus int;
	declare @ErrorSeverity int;
	set @ErrorSeverity = 16;
	set @ErrorStatus = 1;
	begin try	

		select * from Feedback;
		
	end try
	begin catch
		SET @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		throw;
	end catch
end;

exec usp_ViewAllFeedbacks


------------------------------  ViewAllFeedbacksOfBook   ---------------------------------

create or alter proc usp_ViewAllFeedbacksOfBook(
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

		if (@BookId is null)
		begin
			set @ErrorMessage = 'BookId cannot be null'
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end

		if exists (select 1 from Feedback where BookId = @BookId)
		begin
			
			select * from Feedback where BookId = @BookId;

		end
		else
		begin
			set @ErrorMessage = 'Book does not exist';
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorStatus);
		end
		
	end try
	begin catch
		throw;
	end catch
end;

exec usp_ViewAllFeedbacksOfBook @BookId = 5

select * from Feedback
select * from Books
select * from Carts
select * from Orders



