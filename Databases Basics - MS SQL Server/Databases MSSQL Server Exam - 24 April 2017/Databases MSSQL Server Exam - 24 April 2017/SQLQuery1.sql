CREATE DATABASE WMS
USE WMS
GO

-- PART I CREATE TABLES

CREATE TABLE Clients 
	(
		ClientId INT PRIMARY KEY IDENTITY NOT NULL,
		FirstName VARCHAR(50) NOT NULL,
		LastName VARCHAR(50) NOT NULL,
		Phone CHAR(12) NOT NULL
	);

CREATE TABLE Mechanics
	(
		MechanicId INT PRIMARY KEY IDENTITY NOT NULL,
		FirstName VARCHAR(50) NOT NULL,
		LastName VARCHAR(50) NOT NULL,
		[Address] VARCHAR(255) NOT NULL
	);

CREATE TABLE Models
	(
		ModelId INT PRIMARY KEY IDENTITY NOT NULL,
		[Name] VARCHAR(50) UNIQUE NOT NULL
	);

CREATE TABLE Jobs
	(
		JobId INT PRIMARY KEY IDENTITY NOT NULL,
		ModelId INT FOREIGN KEY(ModelId) REFERENCES Models(ModelId) NOT NULL,
		[Status] VARCHAR(11)  CHECK([Status] IN ('Pending' , 'In Progress' , 'Finished')) DEFAULT 'Pending' NOT NULL,
		ClientId INT FOREIGN KEY(ClientId) REFERENCES Clients(ClientId)  NOT NULL,
		MechanicId INT FOREIGN KEY(MechanicId) REFERENCES Mechanics(MechanicId),
		IssueDate DATE NOT NULL,
		FinishDate DATE
	);

CREATE TABLE Orders 
	(
		OrderId INT PRIMARY KEY IDENTITY NOT NULL,
		JobId INT FOREIGN KEY(JobId) REFERENCES Jobs(JobId) NOT NULL,
		IssueDate DATE,
		Delivered BIT DEFAULT 0,
	);

CREATE TABLE Vendors
	(
		VendorId INT PRIMARY KEY IDENTITY NOT NULL,
		[Name] VARCHAR(50) UNIQUE NOT NULL
	);

CREATE TABLE Parts
	(
		PartId INT PRIMARY KEY IDENTITY NOT NULL,
		SerialNumber VARCHAR(50) UNIQUE NOT NULL,
		[Description] VARCHAR(255),
		Price DECIMAL(6, 2) NOT NULL CHECK(Price >= 1),
		VendorId INT FOREIGN KEY(VendorId) REFERENCES Vendors(VendorId) NOT NULL,
		StockQty INT CHECK(StockQty >= 0 ) DEFAULT 0,
	)

CREATE TABLE OrderParts
	(
		OrderId INT FOREIGN KEY(OrderId) REFERENCES Orders(OrderId) NOT NULL,
		PartId INT FOREIGN KEY(PartId) REFERENCES Parts(PartId) NOT NULL,
		Quantity INT CHECK(Quantity >= 0 ) DEFAULT 1,

		CONSTRAINT PK_OrderParts PRIMARY KEY(OrderId, PartId)
	);

CREATE TABLE PartsNeeded
	(
		JobId INT FOREIGN KEY(JobId) REFERENCES Jobs(JobId) NOT NULL,
		PartId INT FOREIGN KEY(PartId) REFERENCES Parts(PartId) NOT NULL,
		Quantity INT CHECK(Quantity >= 0 ) DEFAULT 1,

		CONSTRAINT PK_PartsNeeded PRIMARY KEY(JobId, PartId)
	);

-- P02. Insert 

INSERT INTO Clients(FirstName, LastName, Phone)
VALUES
	('Teri', 'Ennaco', '570-889-5187'),
	('Merlyn', 'Lawler', '201-588-7810'),
	('Georgene', 'Montezuma', '925-615-5185'),
	('Jettie', 'Mconnell', '908-802-3564'),
	('Lemuel', 'Latzke', '631-748-6479'),
	('Melodie', 'Knipp', '805-690-1682'),
	('Candida', 'Corbley', '908-275-8357');

INSERT INTO Parts (SerialNumber, [Description], Price, VendorId)
VALUES
	('WP8182119', 'Door Boot Seal', 117.86, 2),
	('W10780048', 'Suspension Rod', 42.81, 1),
	('W10841140', 'Silicone Adhesive ', 6.77, 4),
	('WPY055980', 'High Temperature Adhesive', 13.94, 3);

-- P03. Update

UPDATE Jobs 
SET MechanicId = 3, [Status] = 'In Progress'
WHERE [Status] = 'Pending'

-- P04. Delete 
DELETE FROM OrderParts 
WHERE OrderId = 19

DELETE FROM Orders
WHERE OrderId = 19

-- P05. Clients by Name

SELECT FirstName, LastName, Phone
FROM Clients
ORDER BY LastName ASC,
		 ClientId ASC;

-- P06. Job Status 

SELECT [Status], IssueDate
FROM Jobs
WHERE STATUS NOT LIKE 'Finished'
ORDER BY IssueDate ASC,
		 JobId ASC;

-- P07. Mechanic Assignments 

SELECT m.FirstName + ' ' + m.LastName AS [Mechanic], j.[Status], j.IssueDate
FROM Jobs AS j
JOIN Mechanics AS m ON m.MechanicId = j.MechanicId
ORDER BY m.MechanicId ASC,
		 j.IssueDate ASC,
		 j.JobId ASC;

-- P08. Current Clients 

SELECT c.FirstName + ' ' + LastName AS [Client], (DATEDIFF(DAY,j.IssueDate, '2017/04/24')) AS [Days going], j.[Status]
FROM Clients AS c
JOIN Jobs AS j ON c.ClientId = j.ClientId
WHERE j.[Status] NOT LIKE 'Finished'
ORDER BY [Days going] DESC,
	     c.ClientId ASC;

-- P09.	Mechanic Performance

SELECT m.FirstName + ' ' + m.LastName AS [Mechanic], AVG(DATEDIFF(DAY, j.IssueDate , j.FinishDate)) AS [Average Days]
FROM Mechanics AS m
JOIN Jobs AS j ON m.MechanicId = j.MechanicId
GROUP BY m.FirstName + ' ' + m.LastName, m.MechanicId
ORDER BY m.MechanicId ASC

-- P10. Hard Earners 

SELECT TOP(3) m.FirstName + ' ' + m.LastName AS [Mechanic], COUNT(j.MechanicId) AS [Jobs]
FROM Mechanics AS m
JOIN Jobs AS j ON m.MechanicId = j.MechanicId
WHERE j.Status NOT LIKE 'Finished'
GROUP BY m.FirstName + ' ' + m.LastName, m.MechanicId
HAVING COUNT(j.MechanicId) > 1
ORDER BY [Jobs] DESC,
	     m.MechanicId ASC;

-- P11. Available Mechanics

SELECT (m.FirstName + ' ' + m.LastName) AS  [Available]
FROM Mechanics AS m
WHERE m.MechanicId NOT IN(
SELECT MechanicId FROM Jobs WHERE Status NOT LIKE 'Finished' AND MechanicId IS NOT NULL)
ORDER BY m.MechanicId ASC;

-- P12. Parts Cost

SELECT ISNULL(SUM(p.Price * op.Quantity),0) AS [Parts Total]
FROM   Parts AS p
JOIN   OrderParts AS op ON op.PartId = p.PartId
JOIN   Orders AS o ON o.OrderId = op.OrderId
WHERE  DATEDIFF(WEEK, o.IssueDate, '2017-04-24') <= 3


--P13. Past Expenses

SELECT  j.JobId, ISNULL(SUM(op.Quantity * p.Price),0) AS [Total]
FROM Jobs AS j
LEFT JOIN Orders AS o ON o.JobId = j.JobId
LEFT JOIN OrderParts AS op ON op.OrderId = o.OrderId
LEFT JOIN Parts AS p ON p.PartId = op.PartId
WHERE j.[Status] LIKE 'Finished'
GROUP BY j.JobId
ORDER BY [Total] DESC,
	     j.JobId ASC;

-- P14.	Model Repair Time

SELECT m.ModelId, m.[Name], CAST(AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)) AS VARCHAR) + ' days' AS [Average Service Time]
FROM Models AS m
LEFT JOIN Jobs AS j ON m.ModelId = j.ModelId
GROUP BY m.ModelId, m.[Name]
ORDER BY [Average Service Time] ASC

--P15. Faultiest Model 

SELECT TOP(1) m.[Name], COUNT(j.JobId), SUM(op.Quantity * p.Price) AS [Parts Total]
FROM Jobs AS j
  JOIN Models AS m ON j.ModelId = m.ModelId
LEFT JOIN Orders AS o ON o.JobId = j.JobId
LEFT JOIN OrderParts AS op ON op.OrderId = o.OrderId
LEFT JOIN Parts AS p ON p.PartId = op.PartId
GROUP BY m.[Name]
ORDER BY([Parts Total]) DESC

--P17. Cost of Order
GO
CREATE FUNCTION udf_GetCost 
(@jobId INT)
RETURNS DECIMAL(6, 2)
AS
	BEGIN
		RETURN
		(
		SELECT(ISNULL(SUM(p.Price),0))
		FROM Parts AS p
			JOIN OrderParts AS op ON op.PartId = p.PartId
			JOIN Orders AS o ON o.OrderId = op.OrderId
			WHERE o.JobId = @jobId
		);
			END;







		 
        


	

