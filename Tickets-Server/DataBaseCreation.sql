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
RankId int PRIMARY KEY IDENTITY,-- קוד שחקן
RankType Nvarchar(100) --מנהל--משתמש רגיל--משתמש מתקדם
);

CREATE TABLE FeedBacks(
FeedBackId int PRIMARY KEY IDENTITY,---מספר בקשת הביקורת
[FeedBackType] int,
Info Nvarchar(1000) NOT NULL
);

CREATE TABLE Users(
Username Nvarchar(100) NOT NULL, ---שם משתמש
[Password] Nvarchar(100) PRIMARY KEY NOT NULL,   --- סיסמה- מפתח ראשי
Email Nvarchar(100) NOT NULL,
Age int,---גיל
Gender Nvarchar(100) NOT NULL,
RankId INT,
FeedBackId INT,
IsAdmin BOOL,
FOREIGN KEY(RankId) REFERENCES Ranks(RankId),
FOREIGN KEY(FeedBackId) REFERENCES FeedBacks(FeedBackId),

);

Create Table Teams(
TeamId int PRIMARY KEY IDENTITY,
Capacity int,
TeamName Nvarchar(100) NOT NULL,
TeamCity Nvarchar(100) NOT NULL,
PriceForTicket int,

);

CREATE TABLE Tickets(
TicketId INT PRIMARY KEY IDENTITY,
Price int,
Place Nvarchar(100) NOT NULL,
[Row] int,
Seats Int,
TeamId INT,
FOREIGN KEY(TeamId) REFERENCES Teams(TeamId),
);

-- Create a login for the admin user
CREATE LOGIN [AdminLogin] WITH PASSWORD = 'Ran1234';
Go

-- Create a user in the TamiDB database for the login
CREATE USER [AdminUser] FOR LOGIN [AdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [AdminUser];
Go


INSERT INTO Ranks Values('Admin')

INSERT INTO FeedBacks Values(1,'i was banned for no reason')

INSERT INTO Users Values('RanTheAdmin','Ran1234',18,'Male',0,0) --FAILED



--INSERT INTO Users Values(N'Amir',N'1234',17,N'Male',2,0)
--INSERT INTO Tickets Values(1,70,N'Gate 1',1,2,1)


--select * from FeedBacks

/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=Tickets_Server;User ID=TaskAdminLogin;Password=kukuPassword;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context TicketsServerDBContext -DataAnnotations -force
*/