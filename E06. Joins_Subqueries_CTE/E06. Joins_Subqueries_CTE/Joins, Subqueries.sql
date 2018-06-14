USE SoftUni

-- P01. Employee Address

SELECT  TOP(5) e.EmployeeID,
			   e.JobTitle,
		       e.AddressID, 
		       a.AddressText
		FROM   Employees AS e
		JOIN   Addresses AS a ON e.AddressID = a.AddressID
	ORDER BY   e.AddressID

-- P02. Addresses with Towns

SELECT TOP (50)  e.FirstName,
				 e.LastName,
				 t.Name,
				 a.AddressText
  FROM Employees AS e
  JOIN Addresses AS a ON e.AddressID = a.AddressID
  JOIN Towns AS t ON t.TownID = a.TownID
       ORDER BY e.FirstName,
		        e.LastName ASC

-- P03. Sales Employees

SELECT	 e.EmployeeID,
		 e.FirstName, 
		 e.LastName, 
		 d.Name
	FROM Employees AS e
	JOIN Departments AS d ON d.DepartmentID = e.DepartmentID
	WHERE d.Name = 'Sales'
	ORDER BY e.EmployeeID ASC

-- P04. Employee Departments

	SELECT TOP(5)	e.EmployeeID,
					e.FirstName,
					e.Salary,
					d.Name
   FROM Employees	AS e
   JOIN Departments AS d ON d.DepartmentID = e.DepartmentID
   WHERE e.Salary > 15000
   ORDER BY			e.DepartmentID ASC

-- P05. Employees Without Projects

SELECT TOP(3)		    e.EmployeeID,
					    e.FirstName
       FROM				Employees AS e
       LEFT OUTER  JOIN	EmployeesProjects AS ep ON ep.EmployeeID = e.EmployeeID 
       WHERE		    ep.ProjectID IS NULL
       ORDER BY			e.EmployeeID

-- P06. Employees Hired After
		
		SELECT e.FirstName,
			   e.LastName,
			   e.HireDate,
			   d.Name
		FROM   Employees AS e
		JOIN   Departments AS d ON e.DepartmentID = d.DepartmentID
		WHERE  e.HireDate > '1/1/1999'
		AND	   d.Name IN('Sales', 'Finance')	
		ORDER  BY e.HireDate ASC

-- P07. Employees With Project

		
SELECT TOP (5) e.EmployeeID,
               e.FirstName,
               p.Name
FROM Employees AS e
		 JOIN  EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
		 JOIN  Projects AS p ON ep.ProjectID = p.ProjectID
WHERE		   p.StartDate >
(
    SELECT CONVERT(DATE, '13.08.2002', 103)
)
		  AND  p.EndDate IS NULL
	 ORDER BY  e.EmployeeID

-- P08. Employee 24

	SELECT e.EmployeeID,
		   e.FirstName,
       CASE
           WHEN p.StartDate > '2005'
           THEN NULL
           ELSE p.Name
         END AS ProjectName
		 FROM Employees AS e
		 JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
		 JOIN Projects AS p ON ep.ProjectID = p.ProjectID
		 WHERE e.EmployeeID = 24

-- P09. Employee Manager

		SELECT		e.EmployeeID, 
					e.FirstName,
					e.ManagerID,
					m.FirstName AS ManagerName
		FROM	Employees AS e
			JOIN	Employees AS m ON e.ManagerID = m.ManagerID
		WHERE	e.ManagerID IN(3, 7)
		ORDER	BY e.EmployeeID ASC

		
				  
				   
				   
		
			
		
		