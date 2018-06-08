-- P01. Records’ Count

SELECT COUNT(Id) AS [Count]
FROM WizzardDeposits

-- P02. Longest Magic Wand

SELECT MAX(MagicWandSize) AS LongestMagicWand
FROM WizzardDeposits

-- P03.  Longest Magic Wand per Deposit Groups

SELECT DepositGroup,
MAX(MagicWandSize) AS LongestMagicWand
FROM WizzardDeposits
GROUP BY DepositGroup

-- P04. Smallest Deposit Group per Magic Wand Size

SELECT TOP(2) DepositGroup
FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize)

-- P05. Deposits Sum

SELECT DepositGroup,
SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
GROUP BY DepositGroup

-- P06. Deposits Sum for Ollivander Family

SELECT DepositGroup,
SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup

-- P07. Deposits Filter

SELECT DepositGroup,
SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY TotalSum DESC

-- P08. Deposit Charge

SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge) AS MinDepositCharge
FROM WizzardDeposits
GROUP BY DepositGroup, MagicWandCreator
ORDER BY MagicWandCreator, DepositGroup ASC 

-- P09. Age Groups

SELECT CASE 
		   WHEN w.Age BETWEEN 0 AND 10
           THEN '[0-10]'
           WHEN w.Age BETWEEN 11 AND 20
           THEN '[11-20]'
           WHEN w.Age BETWEEN 21 AND 30
           THEN '[21-30]'
           WHEN w.Age BETWEEN 31 AND 40
           THEN '[31-40]'
           WHEN w.Age BETWEEN 41 AND 50
           THEN '[41-50]'
           WHEN w.Age BETWEEN 51 AND 60
           THEN '[51-60]'
           WHEN w.Age >= 61
           THEN '[61+]'
           ELSE 'N\A'
		END AS AgeGroup,
		COUNT(*) AS WizzardsCount
	FROM WizzardDeposits AS w
	GROUP BY CASE
			 WHEN w.Age BETWEEN 0 AND 10
			 THEN '[0-10]'
             WHEN w.Age BETWEEN 11 AND 20
             THEN '[11-20]'
             WHEN w.Age BETWEEN 21 AND 30
             THEN '[21-30]'
             WHEN w.Age BETWEEN 31 AND 40
             THEN '[31-40]'
             WHEN w.Age BETWEEN 41 AND 50
             THEN '[41-50]'
             WHEN w.Age BETWEEN 51 AND 60
             THEN '[51-60]'
             WHEN w.Age >= 61
             THEN '[61+]'
             ELSE 'N\A'
         END

-- P10. First Letter

SELECT LEFT(FirstName, 1) AS FirstLetter
FROM WizzardDeposits
WHERE DepositGroup = 'Troll Chest'
GROUP BY LEFT(FirstName, 1)
ORDER BY FirstLetter

-- P11. Average Interest

SELECT DepositGroup,
       IsDepositExpired,
       AVG(1.0 * DepositInterest)
FROM WizzardDeposits
WHERE DepositStartDate > '01/01/1985'
GROUP BY DepositGroup,
         IsDepositExpired
ORDER BY DepositGroup DESC,
         IsDepositExpired

-- P12. Rich Wizard, Poor Wizard

SELECT SUM(w.Difference)
FROM
(
    SELECT DepositAmount -
    (
        SELECT DepositAmount
        FROM WizzardDeposits AS wsd
        WHERE wsd.Id = wd.Id + 1
    ) AS Difference
    FROM WizzardDeposits AS wd
) AS w

-- P13. Departments Total Salaries

USE SoftUni

SELECT DepartmentID, SUM(Salary) AS TotalSalary
FROM Employees
GROUP BY DepartmentID

-- P14.  Employees Minimum Salaries

SELECT DepartmentID, MIN(Salary) AS MinimumSalary
FROM Employees
WHERE DepartmentID LIKE '[2, 5, 7]'
AND HireDate > '01/01/2000'
GROUP BY DepartmentID

-- P15. Employees Average Salaries

SELECT *
INTO NewTable
FROM Employees
WHERE Salary > 30000;

DELETE FROM NewTable
WHERE ManagerID = 42;

UPDATE NewTable
  SET
      Salary += 5000
WHERE DepartmentID = 1;

SELECT DepartmentID,
       AVG(Salary)
FROM NewTable
GROUP BY DepartmentID

-- P16. Employees Maximum Salaries

SELECT DepartmentID, MAX(Salary) AS MaxSalary
FROM Employees
GROUP BY DepartmentID
HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000 

-- P17. Employees Count Salaries

SELECT COUNT(Salary)
FROM Employees
WHERE ManagerID IS NULL

-- P18. 3rd Highest Salary

SELECT salaries.DepartmentID,
       salaries.Salary
FROM
(
    SELECT DepartmentID,
           Salary, 
           DENSE_RANK() OVER(PARTITION BY DepartmentID ORDER BY Salary DESC) AS Rank
    FROM Employees
    GROUP BY DepartmentID,
             Salary
) AS salaries
WHERE Rank = 3
GROUP BY salaries.DepartmentID,
         salaries.Salary

--P19. Salary Challenge

SELECT TOP 10 FirstName,
              LastName,
              DepartmentID
FROM Employees AS e
WHERE Salary >
(
    SELECT AVG(Salary)
    FROM Employees AS em
    WHERE e.DepartmentID = em.DepartmentID)
