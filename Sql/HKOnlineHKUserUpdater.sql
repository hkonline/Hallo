USE [master]

declare	@pmoId int, @mobile varchar(255), @antal int, @email varchar(255), @guardian int, @age int, @birthdate date
declare @lastname varchar(50), @firstname varchar(80), @mother int, @father int

declare userCursor 
	CURSOR FOR 
		select [Lastname], [Firstname]+' '+[Middlename], [Age], [Person ID], [Cell Phone], [E-Mail], CAST(LEFT(RIGHT([Guardian],6),5) as INT), Birthdate from kobenhavn.dbo.PMO$ 
		
open userCursor	FETCH next from userCursor INTO @lastname, @firstname, @age, @pmoId, @mobile, @email, @guardian, @birthdate

while @@FETCH_STATUS=0
begin	
	select @antal = count(*) from kobenhavn.kobenhavn.hk_user where pmopersonid = @pmoId
	print 'Hej'
	update kobenhavn.kobenhavn.hk_user set mobil = @mobile, email = @email where pmopersonid = @pmoID

	if @antal = 0 
	begin
		print 'New record!'
		
		insert into kobenhavn.kobenhavn.hk_user (Efternavn, Fornavn, Mobil, Email, Foedselsdato, LoginApproved, Godkendt, LiveInCPH, AttendsMeetings, PMOPersonId, SMSInfoGenerelt, SendGenerelInfoMail, ShowOnList)
		values ( @lastname, @firstname, @mobile, @email, @birthdate, 1, 1, 1, 1, @pmoId, 1, 1, 1 )
		
		if @pmoId <> @guardian 
		begin
			if @age > 20 
			begin -- Spouse
				Print 'Voksen'
				update kobenhavn.kobenhavn.hk_user set spouse = @pmoId where UserID = @guardian
				
				update kobenhavn.kobenhavn.hk_user set spouse = @guardian where UserID = @pmoId
			end			
				else
					begin
						Print 'Barn'
						select @father = userId, @mother = spouse from kobenhavn.kobenhavn.hk_user where pmopersonid = @guardian
						--select @mother = userId from kobenhavn.kobenhavn.hk_user where spouse = @father
						
						print @father
						print @mother

						--print 'father=' + cast(@father as char)
						--print 'mother=' + cast(@mother as char)

						update kobenhavn.kobenhavn.hk_user set far = @father, mor = @mother where pmopersonID = @pmoId
					end
		end
	end  

	FETCH next from userCursor INTO @lastname, @firstname, @age, @pmoId, @mobile, @email, @guardian, @birthdate
end

close userCursor
deallocate userCursor
