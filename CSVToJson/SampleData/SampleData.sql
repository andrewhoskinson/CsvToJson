-- Creates sample data for the demo
declare @peopleId uniqueidentifier

select @peopleId = NEWID()

INSERT INTO
	People
	(
		Id,
		[Name]
	)
	values
	(
		@peopleId,
		'Dave'
	)

INSERT INTO 
	Addresses
	(
		Id,
		PeopleId,
		Line1,
		Line2
	)
	values
	(
		NEWID(),
		@peopleId,
		'Street',
		'Town'
	)
