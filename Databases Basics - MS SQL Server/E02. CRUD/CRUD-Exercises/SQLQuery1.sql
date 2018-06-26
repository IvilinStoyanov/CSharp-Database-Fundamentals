USE SoftUni
-- P02. Find All Information About Departments

SELECT *
FROM Departments

-- P03.  Find all Department Names

SELECT [Name]
FROM Departments

-- P04. Find Salary of Each Employee

SELECT FirstName, LastName, Salary
FROM Employees

-- P05. Find Full Name of Each Employee

SELECT FirstName, MiddleName, LastName
FROM Employees

-- P06. Find Email Address of Each Employee

SELECT FirstName + '.' + LastName + '@softuni.bg' AS 'Full Email Adress'
FROM Employees

-- P07. Find All Different Employee’s Salaries

SELECT DISTINCT Salary
FROM Employees 

-- P08. Find all Information About Employees

SELECT *
FROM Employees
WHERE JobTitle = 'Sales Representative'

-- P09. Find Names of All Employees by Salary in Range

SELECT FirstName,
       LastName,
       JobTitle
FROM Employees
WHERE Salary >= 20000
      AND Salary <= 30000

-- P10. Find Names of All Employees

SELECT FirstName + ' ' + MiddleName + ' ' + LastName AS 'Full Name'
FROM Employees
WHERE Salary = 25000
      OR Salary = 14000
      OR Salary = 12500
      OR Salary = 23600

-- P11. Find All Employees Without Manager

SELECT FirstName,
       LastName
FROM Employees
WHERE ManagerID IS NULL

-- P12. Find All Employees with Salary More Than

SELECT FirstName,
       LastName,
       Salary
FROM Employees
WHERE Salary > 50000
ORDER BY Salary DESC

-- P13. Find 5 Best Paid Employees

SELECT TOP (5) FirstName,
               LastName
FROM Employees
ORDER BY Salary DESC

-- P14. Find All Employees Except Marketing

SELECT FirstName,
       LastName
FROM Employees
WHERE NOT DepartmentID = 4

-- P15. Sort Employees Table

SELECT *
FROM Employees
ORDER BY Salary DESC,
         FirstName ASC,
         LastName DESC,
         MiddleName ASC

-- P16. Create View Employees with Salaries
GO 

CREATE VIEW V_EmployeesSalaries
AS
     SELECT FirstName,
            LastName,
            Salary
     FROM Employees

GO

SELECT *
FROM V_EmployeesSalaries


-- P17. Create View Employees with Job Titles
GO

CREATE VIEW V_EmployeeNameJobTitle
AS
     SELECT FirstName + ' ' + ISNULL(MiddleName, '') + ' ' + LastName AS 'Full Naeme',
            JobTitle AS 'Job Title'
     FROM Employees

GO
-- P18. Distinct Job Titles

SELECT DISTINCT
       JobTitle
FROM Employees

-- P19. Find First 10 Started Projects

SELECT TOP (10) *
FROM Projects
ORDER BY StartDate,
         [Name]

-- P20. Last 7 Hired Employees

SELECT TOP (7) firstName,
               LastName,
               HireDate
FROM Employees
ORDER BY HireDate DESC

-- P21. Increase Salaries

USE SoftUni

DECLARE @EngineeringID INT

DECLARE @ToolDesignID INT

DECLARE @MarketingID INT

DECLARE @InformationServicesID INT

SELECT TOP (1) @EngineeringID = DepartmentID
FROM Departments
WHERE [Name] = 'Engineering'

SELECT TOP (1) @ToolDesignID = DepartmentID
FROM Departments
WHERE [Name] = 'Tool Design'

SELECT TOP (1) @MarketingID = DepartmentID
FROM Departments
WHERE [Name] = 'Marketing'

SELECT TOP (1) @InformationServicesID = DepartmentID
FROM Departments
WHERE [Name] = 'Information Services'

UPDATE Employees
  SET
      Salary *= 1.12
WHERE DepartmentID = @EngineeringID
      OR DepartmentID = @ToolDesignID
      OR DepartmentID = @MarketingID
      OR DepartmentID = @InformationServicesID

SELECT Salary
FROM Employees

-- P22. All Mountain Peaks
USE Geography

SELECT PeakName
FROM Peaks
ORDER BY PeakName

-- P23. Biggest Countries by Population

SELECT TOP (30) CountryName,
                [Population]
FROM Countries
WHERE ContinentCode = 'EU'
ORDER BY [Population] DESC,
         CountryName

-- P24. Countries and Currency (Euro / Not Euro)

SELECT CountryName,
       CountryCode,
       CASE CurrencyCode
           WHEN 'EUR'
           THEN 'Euro'
           ELSE 'Not Euro'
       END AS 'Currency'
FROM Countries
ORDER BY CountryName

-- P25. All Diablo Characters

USE Diablo

SELECT [Name]
FROM Characters
ORDER BY [Name]
