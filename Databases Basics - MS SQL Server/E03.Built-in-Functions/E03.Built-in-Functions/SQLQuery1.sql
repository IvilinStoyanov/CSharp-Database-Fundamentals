USE SoftUni

-- P01. Find Names of All Employees by First Name

SELECT FirstName,
       LastName
FROM Employees
WHERE FirstName LIKE 'SA%'

-- P02. Find Names of All employees by Last Name 

SELECT FirstName,
       LastName
FROM Employees
WHERE LastName LIKE '%ei%'

-- P03. Find First Names of All Employess

SELECT FirstName 
FROM Employees
WHERE DepartmentID = 3 OR DepartmentID = 10
AND DATEPART(YEAR, HireDate) BETWEEN 1995 AND 2005

-- P04. Find All Employees Except Engineers

SELECT FirstName,
       LastName
FROM Employees
WHERE JobTitle NOT LIKE '%engineer%'

-- P05.  Find Towns with Name Length

SELECT Name
FROM Towns
WHERE LEN(Name) = 5
      OR LEN(Name) = 6
ORDER BY Name

-- P06. Find Towns Starting With

SELECT TownID , 
       [Name]
FROM Towns
WHERE LEFT(Name, 1) = 'M'
OR LEFT(Name, 1) = 'K'
OR LEFT(Name, 1) = 'B'
OR LEFT(Name, 1) = 'E'
ORDER BY Name

-- P07. Find Towns Not Starting With

SELECT TownId,
       Name
FROM Towns
WHERE LEFT(Name, 1) NOT LIKE '[RBD]'
ORDER BY Name

-- P08. Create View Employees Hired After
GO
CREATE VIEW V_EmployeesHiredAfter2000
AS
     SELECT FirstName,
            LastName
     FROM Employees
     WHERE DATEPART(YEAR, HireDate) > 2000
GO
-- P09. Length of Last Name

SELECT FirstName,
	   LastName
FROM Employees
WHERE LEN(LastName) = 5

USE Geography

-- P10 Countries Holding 'A'

SELECT CountryName,
       IsoCode AS [ISO Code]
FROM Countries
WHERE CountryName LIKE '%a%a%a%'
ORDER BY IsoCode

-- P11. Mix of Peak and River Names

SELECT Peaks.PeakName,
       Rivers.RiverName,
       LOWER(CONCAT(LEFT(Peaks.PeakName, LEN(Peaks.PeakName)-1), Rivers.RiverName)) AS Mix
FROM Peaks
     JOIN Rivers ON RIGHT(Peaks.PeakName, 1) = LEFT(Rivers.RiverName, 1)
ORDER BY Mix;

USE Diablo

-- P12. Games from 2011 and 2012 year

SELECT TOP (50) Name,
                FORMAT(CAST(Start AS DATE), 'yyyy-MM-dd') AS [Start]
FROM Games
WHERE DATEPART(YEAR, Start) BETWEEN 2011 AND 2012
ORDER BY Start,
         Name

-- P13. User Email Providers

SELECT Username,
       RIGHT(Email, LEN(Email)-CHARINDEX('@', Email)) AS [Email Provider]
FROM Users
ORDER BY [Email Provider],
         Username

-- P14.  Get Users with IPAddress Like Pattern

SELECT Username,
       IpAddress AS [IP Address]
FROM Users
WHERE IpAddress LIKE '___.1_%._%.___'
ORDER BY Username

-- P15. Show All Games with Duration

SELECT Name AS [Game],
       CASE
           WHEN DATEPART(HOUR, Start) BETWEEN 0 AND 11
           THEN 'Morning'
           WHEN DATEPART(HOUR, Start) BETWEEN 12 AND 17
           THEN 'Afternoon'
           WHEN DATEPART(HOUR, Start) BETWEEN 18 AND 23
           THEN 'Evening'
           ELSE 'N\A'
       END AS [Part of the Day],
       CASE
           WHEN Duration <= 3
           THEN 'Extra Short'
           WHEN Duration BETWEEN 4 AND 6
           THEN 'Short'
           WHEN Duration > 6
           THEN 'Long'
           WHEN Duration IS NULL
           THEN 'Extra Long'
           ELSE 'Error - must be unreachable case'
       END AS [Duration]
FROM Games
ORDER BY Name,
         [Duration],
         [Part of the Day]

-- P16. Orders Table
USE Orders

SELECT ProductName,
       OrderDate,
       DATEADD(DAY, 3, OrderDate) AS [Pay Due],
       DATEADD(MONTH, 1, OrderDate) AS [Deliver Due]
FROM Orders