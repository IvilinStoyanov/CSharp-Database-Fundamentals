CREATE DATABASE RentACar
GO

USE RentACar
GO

-- P01. CREATE

CREATE TABLE Clients 
	(
		Id INT PRIMARY KEY IDENTITY,
		FirstName NVARCHAR(30) NOT NULL,
		LastName NVARCHAR(30) NOT NULL,
		Gender CHAR(1) CHECK(Gender IN ('M' , 'F')),
		BirthDate DATETIME,
		CreditCard NVARCHAR(30) NOT NULL,
		CardValidity DATETIME,
		Email NVARCHAR(50) NOT NULL,
	);

CREATE TABLE Towns 
	(
		Id INT PRIMARY KEY IDENTITY,
		[Name] NVARCHAR(50) NOT NULL,
	);

CREATE TABLE Offices
	(
		Id INT PRIMARY KEY IDENTITY,
		[Name] NVARCHAR(40),
		ParkingPlaces INT,
		TownId INT FOREIGN KEY(TownId) REFERENCES Towns(Id) NOT NULL
	);

CREATE TABLE Models
	(
		Id INT PRIMARY KEY IDENTITY,
		Manufacturer NVARCHAR(50) NOT NULL,
		Model NVARCHAR(50) NOT NULL,
		ProductionYear DATETIME,
		Seats INT,
		Class NVARCHAR(10),
		Consumption DECIMAL(14, 2)
	);

CREATE TABLE Vehicles
	(
		Id INT PRIMARY KEY IDENTITY,
		ModelId INT FOREIGN KEY(ModelId) REFERENCES Models(Id) NOT NULL,
		OfficeId INT FOREIGN KEY(OfficeId) REFERENCES Offices(Id) NOT NULL,
		Mileage INT
	);

CREATE TABLE Orders
	(
		Id INT PRIMARY KEY IDENTITY,
		ClientId INT FOREIGN KEY(ClientId) REFERENCES Clients(Id) NOT NULL,
		TownId INT FOREIGN KEY(TownId) REFERENCES Towns(Id) NOT NULL,
		VehicleId INT FOREIGN KEY(VehicleId) REFERENCES Vehicles(Id) NOT NULL,
		CollectionDate DATETIME NOT NULL,
		CollectionOfficeId INT FOREIGN KEY(CollectionOfficeId) REFERENCES Offices(Id) NOT NULL,
		ReturnDate DATETIME,
		ReturnOfficeId INT FOREIGN KEY(ReturnOfficeId) REFERENCES Offices(Id),
		Bill DECIMAL(14, 2),
		TotalMileage INT
	);

-- P02. INSERT

INSERT INTO Models(Manufacturer, Model, ProductionYear, Seats, Class, Consumption)
VALUES	
	('Chevrolet', 'Astro', '2005-07-27 00:00:00.000', 4, 'Economy',	12.60),
	('Toyota', 'Solara', '2009-10-15 00:00:00.000', 7, 'Family', 13.80),
	('Volvo', 'S40', '2010-10-12 00:00:00.000', 3, 'Average', 11.30),
	('Suzuki', 'Swift', '2000-02-03 00:00:00.000', 7, 'Economy', 16.20);

INSERT INTO Orders
(ClientId, TownId, VehicleId, CollectionDate, CollectionOfficeId, ReturnDate, ReturnOfficeId, Bill, TotalMileage)
VALUES
	(17, 2, 52, '2017-08-08', 30, '2017-09-04', 42, 2360.00, 7434),
	(78, 17, 50, '2017-04-22', 10, '2017-05-09', 12, 2326.00, 7326),
	(27, 13, 28, '2017-04-25', 21, '2017-05-09 ', 34, 597.00, 1880);

-- P03. UPDATE

UPDATE Models
SET Class = 'Luxury'
WHERE Consumption > 20

-- P04.Delete 

DELETE 
FROM Orders
WHERE ReturnDate IS NULL

-- P05. Showroom 

SELECT Manufacturer, Model
FROM Models
ORDER BY Manufacturer ASC,
		 Id DESC;

-- P06. Y Generation 

SELECT   c.FirstName, c.LastName
FROM	 Clients AS c
WHERE	 DATEPART(YEAR, c.BirthDate) BETWEEN 1977 AND 1994
ORDER BY c.FirstName,
		 c.LastName,
		 c.Id;

-- P07. Spacious Office 

SELECT t.[Name] AS [TownName], o.[Name] AS [OfficeName], o.ParkingPlaces
FROM Offices AS o
JOIN Towns AS t ON t.Id = o.TownId
WHERE o.ParkingPlaces > 25
ORDER BY [TownName] ASC,
		 o.Id ASC;

-- P08. Available Vehicles 

SELECT m.Model, m.Seats, v.Mileage
FROM Vehicles AS v
JOIN Models AS m ON m.Id = v.ModelId
WHERE v.Id NOT IN
	(
	SELECT o.VehicleId
	FROM Orders AS o 
	WHERE o.ReturnDate IS NULL
	)
ORDER BY v.Mileage ASC, 
		 m.Seats DESC,
		 m.Id ASC;

-- P09. Offices per Town 

SELECT t.[Name] AS [TownName], COUNT(o.Id) AS [OfficesNumber]
FROM Towns AS t
JOIN Offices AS o ON o.TownId = t.Id
GROUP BY t.[Name]
ORDER BY [OfficesNumber] DESC,
		 [TownName] ASC;

-- P10. Buyers Best Choice 

SELECT m.Manufacturer, m.Model, COUNT(o.Id) AS [TimesOrdered]
FROM Vehicles AS v
LEFT JOIN Orders AS o ON o.VehicleId = v.Id
LEFT JOIN Models AS m ON m.Id = v.ModelId
GROUP BY m.Manufacturer,
		 m.Model
ORDER BY [TimesOrdered] DESC,
		 m.Manufacturer DESC,
		 m.Model ASC;

-- P11.	Kinda Person
SELECT Names, Class
FROM 
(
	SELECT c.FirstName + ' ' + c.LastName AS [Names], m.Class,
	 RANK() OVER (PARTITION BY c.FirstName + ' ' + c.LastName ORDER BY COUNT(m.Class) DESC) AS RANK
	FROM Clients AS c
	JOIN Orders AS o ON o.ClientId = c.Id
	JOIN Vehicles AS v ON v.Id = o.VehicleId
	JOIN Models AS m ON m.Id = v.ModelId
	GROUP BY c.FirstName + ' ' + c.LastName, m.Class
) AS H1
WHERE RANK = 1
ORDER BY Names, Class

-- P12. Age Groups Revenue 

SELECT AgeGroup =
	CASE
		WHEN YEAR(c.BirthDate) BETWEEN 1970 AND 1979 THEN '70''s'
		WHEN YEAR(c.BirthDate) BETWEEN 1980 AND 1989 THEN '80''s'
		WHEN YEAR(c.BirthDate) BETWEEN 1990 AND 1999 THEN '90''s'
		ELSE 'Others'
	END,
	SUM(o.Bill) AS Revenue,
	AVG(o.TotalMileage) AS [AverageMileage]
FROM Clients AS c
JOIN Orders AS o ON c.Id = o.ClientId
GROUP BY
	CASE
		WHEN YEAR(c.BirthDate) BETWEEN 1970 AND 1979 THEN '70''s'
		WHEN YEAR(c.BirthDate) BETWEEN 1980 AND 1989 THEN '80''s'
		WHEN YEAR(c.BirthDate) BETWEEN 1990 AND 1999 THEN '90''s'
		ELSE 'Others'
	END
ORDER BY AgeGroup 

-- P13. Consumption in Mind 
SELECT Manufacturer, AverageConsumption
FROM (
	SELECT TOP(7) m.Model,
				  m.Manufacturer,
				  AVG(m.Consumption) AS [AverageConsumption], 
				  COUNT(m.Model) AS Counter
	FROM Orders AS o
	JOIN Vehicles AS v ON v.Id = o.VehicleId
	JOIN Models AS m ON m.Id = v.ModelId
	GROUP BY	 m.Manufacturer, 
				 m.Model
	--HAVING AVG(m.Consumption) BETWEEN 5 AND 15
	ORDER BY Counter DESC
) AS H1
WHERE AverageConsumption  BETWEEN 5 AND 15
ORDER BY Manufacturer,
		 AverageConsumption

-- P14.	Debt Hunter
SELECT Names, Emails, Bills, TownsNames	 
FROM
	(
		SELECT ROW_NUMBER() OVER (PARTITION BY t.[Name] ORDER BY o.Bill DESC) AS OrderBYHighestBill,
			   CONCAT(c.FirstName, ' ', c.LastName) AS [Names],
			   c.Email AS Emails,
			   o.Bill AS Bills,
			   c.Id AS ClientId,
			   t.[Name] AS TownsNames
		
		FROM Clients AS c
		JOIN Orders AS o ON o.ClientId = c.Id
		JOIN Towns AS t ON t.Id = o.TownId
		WHERE o.Bill IS NOT NULL AND o.CollectionDate > c.CardValidity
	) AS H1
WHERE OrderBYHighestBill IN(1, 2)
ORDER BY TownsNames,
		 Bills,
		 ClientId;

-- P15. Town Statistics 

SELECT t.[Name] AS [TownName], 
		SUM(H.M) * 100 / (ISNULL(SUM(H.M),0) + ISNULL(SUM(H.F), 0)) AS MalePercent,
		SUM(H.F) * 100 / (ISNULL(SUM(H.M),0) + ISNULL(SUM(H.F), 0)) AS MalePercent	
	FROM 
		(
			SELECT o.TownId,
			CASE WHEN(Gender = 'M') THEN COUNT(o.Id)  END AS M,
			CASE WHEN(Gender = 'F') THEN COUNT(o.Id)  END AS F
			FROM Orders AS o
			JOIN Clients AS c ON c.Id = o.ClientId
			GROUP BY c.Gender, o.TownId
		) AS H
	JOIN Towns AS T ON t.Id = H.TownId
	GROUP BY t.[Name]

-- P17.	Find My Ride
GO

CREATE FUNCTION udf_CheckForVehicle
		(@townName NVARCHAR(50),
		 @seatsNumber INT)
RETURNS NVARCHAR(MAX)
	AS
		BEGIN
			DECLARE @Result VARCHAR(100) = 
				( 
					SELECT TOP(1) CONCAT(o.Name, ' - ', m.Model)
					FROM Towns AS t 
					JOIN Offices AS o ON o.TownId = t.Id
					JOIN Vehicles AS v ON v.OfficeId = o.Id
					JOIN Models AS m ON m.Id = v.ModelId
					WHERE t.[Name] = @townName AND m.Seats = @seatsNumber
					ORDER BY o.[Name]
				)
			IF(@Result IS NULL)	
				BEGIN	 
				 RETURN 'NO SUCH VEHICLE FOUND';
				END
				 RETURN @Result;
		END

-- P18.	Move a Vehicle
GO

CREATE PROCEDURE usp_MoveVehicle @vehicleId INT , @officeId INT
AS
	BEGIN		
		BEGIN TRANSACTION
			UPDATE Vehicles
			SET OfficeId = @officeId
			WHERE Id = @vehicleId

			DECLARE @countVehiclesById INT = 
			(
				SELECT COUNT(v.Id) FROM Vehicles AS v
				WHERE v.OfficeId = @officeId
			)

			DECLARE @parkingPlaces INT =
			(
				SELECT o.ParkingPlaces FROM Offices AS o
				WHERE o.Id = @officeId
			)

			IF(@countVehiclesById > @parkingPlaces) 
				BEGIN
					ROLLBACK;
					RAISERROR('Not enough room in this office!', 16, 1);
					RETURN
				END
			COMMIT
	END	

--P19. Move the Tally
GO

CREATE TRIGGER tr_MoveTheTally
ON Orders
FOR UPDATE
	AS
		BEGIN
			DECLARE @newTotalMileage INT =
				(
					SELECT TotalMileage FROM inserted
				)
			DECLARE @oldTotalMileage INT =
				(
					SELECT TotalMileage FROM deleted
				)
			DECLARE @vehicleId INT = 
				(
					SELECT VehicleId FROM inserted
				)

			IF (@oldTotalMileage IS NULL AND @vehicleId IS NOT NULL)
				BEGIN
					UPDATE Vehicles
					SET Mileage += @newTotalMileage
					WHERE Id = @vehicleId
				END
		END