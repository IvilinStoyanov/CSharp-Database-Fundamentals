CREATE DATABASE Instagram

CREATE TABLE Users(
	Id BIGINT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) NOT NULL UNIQUE,
	[Password] VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX),
	LastLoginTime DATETIME,
	IsDeleted BIT
 )

 INSERT INTO Users (Username, [Password], ProfilePicture, LastLoginTime, IsDeleted) VALUES 
('Stamat', '123', NULL, CONVERT(datetime,'22-05-2018',103), '0'),
('Pesho', '1223', NULL, CONVERT(datetime,'23-05-2018',103), '0'),
('Gosho', '1234', NULL, CONVERT(datetime,'24-05-2018',103), '0'),
('Blqblq', '12223', NULL,CONVERT(datetime,'25-05-2018',103), '0'),
('Mlqmlq', '123232', NULL, CONVERT(datetime,'26-05-2018',103), '0')

ALTER TABLE Users
ADD CONSTRAINT CHK_ProfilePicture CHECK (DATALENGHT(ProfilePicture) <= 900 * 1024)


