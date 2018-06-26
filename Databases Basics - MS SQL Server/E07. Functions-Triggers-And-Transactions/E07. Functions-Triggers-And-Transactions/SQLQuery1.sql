USE SoftUni

--P01. Employees with Salary Above 35000
GO

CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000 
AS
	BEGIN 
		SELECT FirstName AS [First Name],
		 LastName AS [Last Name]
		FROM Employees
		WHERE Salary > 35000
	END;

--P02. Employees with Salary Above Number
GO

CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber (@minSalary DECIMAL(18,4))
AS
	BEGIN
		SELECT FirstName AS [First Name],
			   LastName AS [Last Name]
		FROM Employees
		WHERE Salary >= @minSalary
	END;

--P03. Town Names Starting With 
GO

CREATE PROCEDURE usp_GetTownsStartingWith (@TownStartWith NVARCHAR(MAX))
AS
	BEGIN 
		SELECT Name
		FROM Towns
		WHERE Name LIKE(@TownStartWith + '%');
	END;

-- P04. Employees from Town 
GO

CREATE PROCEDURE usp_GetEmployeesFromTown(@EmploeeyTown NVARCHAR(MAX))
AS
	BEGIN
		SELECT FirstName AS [First Name],
			   LastName AS [Last Name]
		FROM Employees AS e
		JOIN Addresses AS adr ON e.AddressID = adr.AddressID
		JOIN Towns AS t ON t.TownID = adr.TownID
		WHERE t.Name = @EmploeeyTown;
	END;

	-- TEST
	-- EXEC usp_GetEmployeesFromTown 'Sofia'

-- P05. Salary Level Function
GO

CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS VARCHAR(7)
	BEGIN
		IF(@salary IS NULL)
			BEGIN 
				RETURN NULL;
			END;
		IF(@salary < 30000)
			BEGIN
				RETURN 'Low';
			END;
		ELSE
			BEGIN
		IF(@salary <= 50000)
			BEGIN
				RETURN 'Average';
			END;
		END;
		RETURN 'High'
	END;

-- P06. Employees by Salary Level 
GO

CREATE PROCEDURE usp_EmployeesBySalaryLevel (@salaryLevel VARCHAR(7))
AS
	BEGIN
		SELECT FirstName AS [First Name],
			   LastName AS [Last Name]
		FROM Employees
		WHERE dbo.ufn_GetSalaryLevel(Salary) = @salaryLevel;
	END;

-- P07. Define Function 
GO

CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(MAX), @word NVARCHAR(MAX))
RETURNS BIT
AS
     BEGIN
         DECLARE @currentIndex INT= 1;
         WHILE(@currentIndex <= LEN(@word))
             BEGIN
                 DECLARE @currentLetter CHAR= SUBSTRING(@word, @currentIndex, 1);
                 IF(CHARINDEX(@currentLetter, @setOfLetters) <= 0)
                     BEGIN
                         RETURN 0;
                 END;
                 SET @currentIndex+=1;
             END;
         RETURN 1;
     END;

-- P08. Delete Employees and Departments 
GO
	CREATE PROCEDURE usp_DeleteEmployeesFromDepartment
(
                 @departmentId INT
)
AS
     BEGIN
         ALTER TABLE Employees ALTER COLUMN ManagerID INT;

         ALTER TABLE Employees ALTER COLUMN DepartmentID INT;

         UPDATE Employees
           SET
               DepartmentID = NULL
         WHERE EmployeeID IN
         (
         (
             SELECT EmployeeID
             FROM Employees
             WHERE DepartmentID = @departmentId
         )
         );

         UPDATE Employees
           SET
               ManagerID = NULL
         WHERE ManagerID IN
         (
         (
             SELECT EmployeeID
             FROM Employees
             WHERE DepartmentID = @departmentId
         )
         );

         ALTER TABLE Departments ALTER COLUMN ManagerID INT;

         UPDATE Departments
           SET
               ManagerID = NULL
         WHERE DepartmentID = @departmentId;

         DELETE FROM Departments
         WHERE DepartmentID = @departmentId;

         DELETE FROM EmployeesProjects
         WHERE EmployeeID IN
         (
         (
             SELECT EmployeeID
             FROM Employees
             WHERE DepartmentID = @departmentId
         )
         );

         DELETE FROM Employees
         WHERE DepartmentID = @departmentId;

         SELECT COUNT(*)
         FROM Employees
         WHERE DepartmentID = @departmentId;
     END;

-- PART II
--P09. Find Full Name 
GO
USE Bank

GO

CREATE PROCEDURE usp_GetHoldersFullName 
AS 
	BEGIN
		SELECT CONCAT(FirstName + ' ' , LastName) AS [Full Name]
		FROM AccountHolders
	END;

-- P10. People with Balance Higher Than 
GO

CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan (@Balance DECIMAL(18,4))
AS
	BEGIN 
		SELECT	 ah.FirstName AS [First Name],
				 ah.LastName AS [Last Name]
		FROM	 AccountHolders AS ah
	    JOIN	 Accounts AS a ON ah.Id = a.AccountHolderId
		GROUP BY ah.FirstName,
				 ah.LastName
		HAVING   @Balance < SUM(a.Balance);
	END;

--P11. Future Value Function 
GO

CREATE FUNCTION ufn_CalculateFutureValue
(
                @sum                MONEY,
                @yearlyInterestRate FLOAT,
                @numberOfYears      INT
)
RETURNS MONEY
AS
     BEGIN
         RETURN @sum * (POWER(1 + @yearlyInterestRate, @numberOfYears));
     END;

--P12. Calculating Interest 
GO

CREATE PROCEDURE usp_CalculateFutureValueForAccount
(
                 @accountId    INT,
                 @interestRate FLOAT
)
AS
     BEGIN
         SELECT a.Id AS [Account Id],
                ah.FirstName AS [First Name],
                ah.LastName AS [Last Name],
                a.Balance AS [Current Balance],
                dbo.ufn_CalculateFutureValue(a.Balance, @interestRate, 5)
         FROM Accounts AS a
              JOIN AccountHolders AS ah ON a.AccountHolderId = ah.Id
         WHERE a.Id = @accountId;
     END;

--P14. Create Table Logs 
GO

CREATE TABLE Logs
(
             LogId     INT NOT NULL IDENTITY,
             AccountId INT NOT NULL,
             OldSum    MONEY NOT NULL,
             NewSum    MONEY NOT NULL,
             CONSTRAINT PK_Logs PRIMARY KEY(LogId),
             CONSTRAINT FK_Logs_Accounts FOREIGN KEY(AccountId) REFERENCES Accounts(Id)
);

GO
CREATE TRIGGER tr_Accounts_Logs_After_Update ON Accounts
FOR UPDATE
AS
     BEGIN
         INSERT INTO Logs
         VALUES
         (
         (
             SELECT Id
             FROM deleted
         ),
         (
             SELECT Balance
             FROM deleted
         ),
         (
             SELECT Balance
             FROM inserted
         )
         );
     END;

-- P15. Create Table Emails 
GO
CREATE TABLE NotificationEmails
(
             Id        INT NOT NULL IDENTITY,
             Recipient INT NOT NULL,
             Subject   NVARCHAR(50) NOT NULL,
             Body      NVARCHAR(255) NOT NULL,
             CONSTRAINT PK_NotificationEmails PRIMARY KEY(Id)
);

GO
CREATE TRIGGER tr_Logs_NotificationEmails ON Logs
FOR INSERT
	AS	
		 BEGIN
         INSERT INTO NotificationEmails
         VALUES
         (
         (
             SELECT AccountId
             FROM inserted
         ),
         CONCAT('Balance change for account: ',
               (
                   SELECT AccountId
                   FROM inserted
               )),
         CONCAT('On ', FORMAT(GETDATE(), 'dd-MM-yyyy HH:mm'), ' your balance was changed from ',
               (
                   SELECT OldSum
                   FROM Logs
               ), ' to ',
               (
                   SELECT NewSum
                   FROM Logs
               ), '.')
         );
     END;

--P16. Deposit Money
GO
CREATE PROCEDURE usp_DepositMoney
(
                 @accountId   INT,
                 @moneyAmount MONEY
)
AS
     BEGIN
         IF(@moneyAmount < 0)
             BEGIN
                 RAISERROR('Cannot deposit negative value', 16, 1);
         END;
             ELSE
             BEGIN
                 IF(@accountId IS NULL
                    OR @moneyAmount IS NULL)
                     BEGIN
                         RAISERROR('Missing value', 16, 1);
                 END;
         END;
         BEGIN TRANSACTION;
         UPDATE Accounts
           SET
               Balance+=@moneyAmount
         WHERE Id = @accountId;
         IF(@@ROWCOUNT < 1)
             BEGIN
                 ROLLBACK;
                 RAISERROR('Account doesn''t exists', 16, 1);
         END;
         COMMIT;
     END;
	 




