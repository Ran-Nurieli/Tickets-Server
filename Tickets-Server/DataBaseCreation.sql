USE master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'Tickets_Server')
BEGIN
    DROP DATABASE Tickets_Server;
END

Go
Create Database Tickets_Server
Go

Use Tickets_Server
Go

CREATE TABLE Ranks(
RankId int PRIMARY KEY,
RankType Nvarchar(100)
);

CREATE TABLE FeedBacks(
FeedBackId int PRIMARY KEY IDENTITY,
[FeedBackType] int,
Info Nvarchar(1000)
);

CREATE TABLE Users(
Username Nvarchar(100), ---שם משתמש
[Password] Nvarchar(100) PRIMARY KEY,   --- סיסמה- מפתח ראשי
Age int,---גיל
Gender Nvarchar(100),
RankId INT,
FeedBackId INT,
FOREIGN KEY(RankId) REFERENCES Ranks(RankId),
FOREIGN KEY(FeedBackId) REFERENCES FeedBacks(FeedBackId),

);

Create Table Teams(
TeamId int PRIMARY KEY IDENTITY,
Capacity int,
TeamName Nvarchar(100),
TeamCity Nvarchar(100),
PriceForTicket int,

);

CREATE TABLE Tickets(
TicketId INT PRIMARY KEY IDENTITY,
Price int,
Place Nvarchar(100),
[Row] int,
Seats Int,
TeamId INT,
FOREIGN KEY(TeamId) REFERENCES Teams(TeamId),
);

-- Create a login for the admin user
CREATE LOGIN [TaskAdminLogin] WITH PASSWORD = 'kukuPassword';
Go

-- Create a user in the TamiDB database for the login
CREATE USER [TaskAdminUser] FOR LOGIN [TaskAdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [TaskAdminUser];
Go

INSERT INTO Users Values(N'Amir',N'1234',17,N'Male',2,0)
INSERT INTO Tickets Values(1,70,N'Gate 1',1,2,1)


--select * from FeedBacks

/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=Tickets_Server;User ID=TaskAdminLogin;Password=kukuPassword;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context TicketsServerDBContext -DataAnnotations -force
*/