CREATE DATABASE TripService
GO
USE TripService
GO
--P01. Create

CREATE TABLE Cities
	(
		Id INT PRIMARY KEY IDENTITY,
		[Name] NVARCHAR(20) NOT NULL,
		CountryCode CHAR(2) NOT NULL,
	);

CREATE TABLE Hotels
	(
		Id INT PRIMARY KEY IDENTITY,
		[Name] NVARCHAR(30) NOT NULL,
		CityId INT FOREIGN KEY(CityId) REFERENCES Cities(Id) NOT NULL,
		EmployeeCount INT NOT NULL,
		BaseRate DECIMAL(16, 2)	
	);

CREATE TABLE Rooms
	(
		Id INT PRIMARY KEY IDENTITY,
		Price DECIMAL(16, 2) NOT NULL,
		[Type] NVARCHAR(20) NOT NULL,
		Beds INT NOT NULL,
		HotelId INT FOREIGN KEY(HotelId) REFERENCES Hotels(Id) NOT NULL,
	);

CREATE TABLE Trips
	(
		Id INT PRIMARY KEY IDENTITY,
		RoomId INT FOREIGN KEY(RoomId) REFERENCES Rooms(Id) NOT NULL,
		BookDate DATE NOT NULL,
		ArrivalDate DATE NOT NULL,
		ReturnDate DATE NOT NULL,
		CancelDate DATE		

		CHECK(BookDate < ArrivalDate),
		CHECK(ArrivalDate < ReturnDate)
		
		

	);

CREATE TABLE Accounts
	(
		Id INT PRIMARY KEY IDENTITY,
		FirstName NVARCHAR(50) NOT NULL,
		MiddleName NVARCHAR(20),
		LastName NVARCHAR(50) NOT NULL,
		CityId INT FOREIGN KEY(CityId) REFERENCES Cities(Id) NOT NULL,
		BirthDate DATE NOT NULL,
		Email VARCHAR(100) UNIQUE NOT NULL
	);

CREATE TABLE AccountsTrips
	(
		AccountId INT FOREIGN KEY(AccountId) REFERENCES Accounts(Id) NOT NULL,
		TripId  INT FOREIGN KEY(TripId) REFERENCES Trips(Id) NOT NULL,
		Luggage INT NOT NULL CHECK(Luggage >= 0)
	);

--P02. INSERT 

INSERT INTO Accounts
(FirstName, MiddleName, LastName, CityId, BirthDate, Email)
VALUES
	('John', 'Smith', 'Smith', 34, '1975-07-21', 'j_smith@gmail.com'),
	('Gosho', NULL, 'Petrov', 11, '1978-05-16', 'g_petrov@gmail.com'),
	('Ivan', 'Petrovich', 'Pavlov', 59, '1849-09-26', 'i_pavlov@softuni.bg'),
	('Friedrich', 'Wilhelm', 'Nietzsche', 2, '1844-10-15',	'f_nietzsche@softuni.bg')


INSERT INTO Trips 
(RoomId, BookDate, ArrivalDate, ReturnDate,	CancelDate)
VALUES
	(101, '2015-04-12', '2015-04-14', '2015-04-20', '2015-02-02'),
	(102, '2015-07-07', '2015-07-15', '2015-07-22', '2015-04-29'),
	(103, '2013-07-17', '2013-07-23', '2013-07-24', NULL),
	(104, '2012-03-17', '2012-03-31', '2012-04-01', '2012-01-10'),
	(109, '2017-08-07', '2017-08-28', '2017-08-29', NULL)

--P03. UPDATE

UPDATE Rooms 
SET Price *= 1.14 
WHERE HotelId = 5 OR HotelId = 7 OR HotelId = 9

GO
SELECT SUM(Price)
FROM Rooms
WHERE Id = 5 OR Id = 7 OR Id = 9


-- P04. DELETE
DELETE
 FROM AccountsTrips
WHERE Id = 47

-- P05. Bulgarian Cities 

SELECT *
FROM Cities

SELECT c.Id, c.[Name]
FROM Cities AS c
WHERE c.CountryCode = 'BG'
ORDER BY c.[Name] 

-- P06. People Born After 1991 
SELECT *
FROM (
SELECT CONCAT(a.FirstName, ' ' + (NULLIF(a.MiddleName,'')), ' ',a.LastName) AS [Full Name], YEAR(a.BirthDate) AS [BirthYear]
FROM Accounts AS a
WHERE YEAR(a.BirthDate) > 1991

) AS h
ORDER BY  h.BirthYear DESC, h.[Full Name] ASC


		

-- P07. EEE-Mails 

SELECT a.FirstName, a.LastName, CONVERT(VARCHAR(10), a.BirthDate, 110) AS [BirthDate] , c.[Name], a.Email
FROM Accounts AS a
JOIN Cities AS c ON c.Id = a.CityId
WHERE a.Email LIKE 'e%'
ORDER BY c.[Name] DESC

-- P08. City Statistics

SELECT c.[Name] AS [City], COUNT(h.Id) AS [Hotels]
FROM Cities AS c
LEFT JOIN Hotels AS h ON h.CityId = c.Id
GROUP BY c.[Name]
ORDER BY COUNT(h.Id) DESC,
		 c.[Name]

-- P09. Expensive First Class Rooms

SELECT r.Id, r.Price, h.[Name], c.[Name]
FROM Rooms AS r
JOIN Hotels AS h ON h.Id = r.HotelId
JOIN Cities AS c ON c.Id = h.CityId
WHERE r.Type = 'First Class'
ORDER BY r.Price DESC,
		 r.Id ASC

-- P10. Longest and Shortest Trips

SELECT a.Id , a.FirstName + ' ' + a.LastName AS [Full Name], COUNT(act.AccountId)
FROM Accounts  AS a
JOIN AccountsTrips AS act ON act.AccountId = a.Id
JOIN Trips AS t ON t.Id = act.TripId
GROUP BY a.Id,  a.FirstName + ' ' + a.LastName, t.CancelDate
HAVING t.CancelDate IS NULL
ORDER BY COUNT(act.AccountId) DESC


-- P11. Metropolis

SELECT TOP(5) c.Id, c.[Name] AS [City], c.CountryCode AS [Country], COUNT(a.Id) AS [Accounts]
FROM Cities AS c
JOIN Accounts AS a ON a.CityId = c.Id
GROUP BY c.Id,
		 c.[Name],
		 c.CountryCode
ORDER BY COUNT(a.Id) DESC
	
-- P12. Romantic Getaways 
		SELECT a.Id, a.Email, c.[Name], COUNT(act.TripId) AS [Trips]
		FROM Accounts AS a
		JOIN Cities AS c ON c.Id = a.CityId
		JOIN AccountsTrips AS act ON act.AccountId = a.Id
		JOIN Trips AS t ON t.Id = act.TripId
		JOIN Hotels AS h ON h.CityId = c.Id
		GROUP BY a.Id,
				 a.Email,
				 c.[Name]
		HAVING COUNT(act.TripId) >= 1
		ORDER BY COUNT(act.TripId) DESC
				

	


-- P13. Lucrative Destinations

SELECT TOP(10) c.Id, c.[Name], SUM(h.BaseRate + r.Price) AS [Total Revenue], COUNT(t.Id) AS [Trips]
FROM Cities AS c
JOIN Hotels AS h ON h.CityId = c.Id
JOIN Rooms AS r ON r.HotelId = h.Id
JOIN Trips AS t ON t.RoomId = r.Id
GROUP BY c.Id,
	     c.[Name],
		 t.BookDate
HAVING YEAR(t.BookDate) = 2016
ORDER BY [Total Revenue] DESC,
		 [Trips] DESC

-- P15. Top Travelers


SELECT a.Id AS [AccountId], a.Email, c.CountryCode,
ROW_NUMBER() OVER(PARTITION BY c.CountryCode ORDER BY 
 COUNT(a.Id) DESC)	AS [Trips]

FROM Accounts AS a
JOIN Cities AS c ON c.Id = a.CityId
JOIN AccountsTrips AS act ON act.AccountId = a.Id
GROUP BY a.Id, a.Email, c.CountryCode
ORDER BY [Trips] DESC, a.Id ASC

--P18
GO

/*CREATE FUNCTION udf_GetAvailableRoom 
(@HotelId INT, @Date DATE, @People INT)
RETURNS VARCHAR(MAX)
	AS
		BEGIN
			DECLARE @priceOfRoom INT;
			SELECT @priceOfRoom = SUM(h.BaseRate + r.Price) * @People
			FROM Rooms AS r
			JOIN Hotels AS h ON h.Id = r.HotelId
		END
	RETURN CONCAT(
  END */










