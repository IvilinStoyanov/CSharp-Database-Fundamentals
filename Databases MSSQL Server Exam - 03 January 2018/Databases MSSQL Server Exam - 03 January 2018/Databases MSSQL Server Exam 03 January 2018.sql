CREATE DATABASE RentACar
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

