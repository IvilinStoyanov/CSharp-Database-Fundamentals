CREATE DATABASE ReportService

GO
--PART I CREATE

CREATE TABLE Users 
	(
		Id INT PRIMARY KEY IDENTITY,
		Username NVARCHAR(30) NOT NULL UNIQUE,
		[Password] NVARCHAR(50) NOT NULL,
		[Name] NVARCHAR (50),
		Gender CHAR(1) CHECK(Gender IN ('M' , 'F')),
		BirthDate DATETIME,
		Age INT,
		Email NVARCHAR(50) NOT NULL
	);

CREATE TABLE Departments 
	(
		Id INT PRIMARY KEY IDENTITY,
		[Name] NVARCHAR(50) NOT NULL
	);

CREATE TABLE Employees 
	(
		Id INT PRIMARY KEY IDENTITY,
		FirstName NVARCHAR(25),
		LastName NVARCHAR(25),
		Gender CHAR(1) CHECK(Gender IN('M' , 'F')),
		BirthDate DATETIME,
		Age INT,
		DepartmentId INT FOREIGN KEY(DepartmentId) REFERENCES Departments(Id) NOT NULL
	);

CREATE TABLE Categories
	(
		Id INT PRIMARY KEY IDENTITY,
		[Name] VARCHAR(50) NOT NULL,
		DepartmentId INT FOREIGN KEY(DepartmentId) REFERENCES Departments(Id),
	);
		
CREATE TABLE [Status]
	(
		Id INT PRIMARY KEY IDENTITY,
		Label VARCHAR(30) NOT NULL
	);

CREATE TABLE Reports
	(
		Id INT PRIMARY KEY IDENTITY,
		CategoryId INT FOREIGN KEY(CategoryId) REFERENCES Categories(Id) NOT NULL,
		StatusId INT FOREIGN KEY(StatusId) REFERENCES [Status](Id) NOT NULL,
		OpenDate DATETIME NOT NULL,
		CloseDate DATETIME,
		[Description] VARCHAR(200),
		UserId INT FOREIGN KEY(UserId) REFERENCES Users(Id) NOT NULL,
		EmployeeId INT FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)
	);

-- PART II Insert
SELECT *
FROM Departments

INSERT INTO Employees(FirstName, LastName, Gender, BirthDate, DepartmentId)
VALUES 
	('Marlo',   'O’Malley',    'M' , '9/21/1958',  1),
	('Niki', '   Stanaghan', 'F' , '11/26/1969', 4),
	('Ayrton',   'Senna',      'M' , '03/21/1960', 9),
	('Ronnie',   'Peterson',   'M' , '02/14/1944', 9),
	('Giovanna', 'Amati',	   'F' , '07/20/1959', 5)

INSERT INTO Reports(CategoryId, StatusId, OpenDate, CloseDate, [Description], UserId, EmployeeId)
VALUES
(1, 1, '04/13/2017', NULL, 'Stuck Road on Str.133', 6, 2),
(6, 3, '09/05/2015', '12/06/2015', 'Charity trail running', 3, 5),
(14, 2, '09/07/2015', NULL, 'Falling bricks on Str.58', 5, 2),
(4, 3, '07/03/2017', '07/06/2017', 'Cut off streetlight on Str.11', 1, 1)	

--P03. UPDATE

UPDATE Reports
SET StatusId = 2
WHERE StatusId = 1 AND CategoryId = 4

--P04. DELETE

DELETE Reports 
WHERE StatusId = 4;

-- P05. Users by Age 

SELECT Username, Age
FROM Users
ORDER BY Age ASC,
		 Username DESC;

-- P06.  Unassigned Reports 

SELECT [Description], OpenDate
FROM Reports
WHERE EmployeeId IS NULL 
ORDER BY OpenDate ASC,
		 [Description] ASC;

-- P07. Employees & Reports 

SELECT e.FirstName, e.LastName, r.[Description], FORMAT(r.OpenDate, 'yyyy-MM-dd') AS OpenDate
FROM Employees AS e
JOIN Reports AS r ON e.Id = r.EmployeeId
WHERE EmployeeId IS NOT NULL
ORDER BY e.Id ASC,
		 r.OpenDate ASC,
		 r.Id ASC;

-- P08. Most Reported Category 

SELECT c.[Name] AS [CategoryName], COUNT(r.Id) AS [ReportsNumber]
FROM Categories AS c
JOIN Reports AS r ON c.Id = r.CategoryId
GROUP BY c.[Name]
ORDER BY ReportsNumber DESC,
	     c.[Name];

-- P09. Employees in Category

SELECT c.[Name] AS [CategoryName], COUNT(e.Id) AS [Employees Number]
FROM Categories AS c
JOIN Employees AS e ON c.DepartmentId = e.DepartmentId
GROUP BY c.[Name];

-- P10. Users per Employee 

SELECT e.FirstName + ' ' + e.LastName AS [Name], COUNT(r.UserId) AS [Users Number]
FROM Reports AS r
RIGHT JOIN Employees AS e ON e.Id = r.EmployeeId
GROUP BY e.FirstName + ' ' + e.LastName
ORDER BY [Users Number] DESC,
		 [Name] ASC;










