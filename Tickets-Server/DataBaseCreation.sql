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
