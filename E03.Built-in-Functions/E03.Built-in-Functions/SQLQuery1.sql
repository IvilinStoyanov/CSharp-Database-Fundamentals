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
