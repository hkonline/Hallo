USE [master]

declare	@pmoId int, @mobile varchar(255), @antal int, @email varchar(255), @guardian int, @age int, @birthdate date
declare @lastname varchar(50), @firstname varchar(80), @mother int, @father int

declare userCursor 
	CURSOR FOR 
		select [Lastname], [Firstname]+' '+[Middlename], [Age], [Person ID], [Cell Phone], [E-Mail], CAST(LEFT(RIGHT([Guardian],6),5) as INT), Birthdate from kobenhavn.dbo.PMO$ 
		where [Firstname] Like 'Claire'

open userCursor	FETCH next from userCursor INTO @lastname, @firstname, @age, @pmoId, @mobile, @email, @guardian, @birthdate

while @@FETCH_STATUS=0
begin	
	select @antal = count(*) from kobenhavn.kobenhavn.hk_user_test where pmopersonid = @pmoId
	print 'Hej'
	update kobenhavn.kobenhavn.hk_user_test set mobil = @mobile, email = @email where pmopersonid = @pmoID

	if @antal = 0 
	begin
		insert into kobenhavn.kobenhavn.hk_user_test (Efternavn, Fornavn, Mobil, Email, Foedselsdato, LoginApproved, Godkendt, LiveInCPH, AttendsMeetings, PMOPersonId, SMSInfoGenerelt, SendGenerelInfoMail, ShowOnList)
		values ( @lastname, @firstname, @mobile, @email, @birthdate, 1, 1, 1, 1, @pmoId, 1, 1, 1 )
		
		if @pmoId <> @guardian 
		begin
			if @age > 20 
			begin -- Spouse
				update kobenhavn.kobenhavn.hk_user_test set spouse = @pmoId where UserID = @guardian
				
				update kobenhavn.kobenhavn.hk_user_test set spouse = @guardian where UserID = @pmoId
			end			
				else
					begin
						select @father = userId from kobenhavn.kobenhavn.hk_user_test where spouse = @pmoId
						select @mother = userId from kobenhavn.kobenhavn.hk_user_test where spouse = @father
						
						print 'father=' + cast(@father as char)
						print 'mother=' + cast(@mother as char)

						update kobenhavn.kobenhavn.hk_user_test set far = @father, mor = @mother where pmopersonID = @pmoId
					end
		end
	end  

	FETCH next from userCursor INTO @lastname, @firstname, @age, @pmoId, @mobile, @email, @guardian, @birthdate
end

close userCursor
deallocate userCursor
