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
