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
[Password] Nvarchar(100) NOT NULL,   --- סיסמה- מפתח ראשי
Email Nvarchar(100) PRIMARY KEY NOT NULL,
Phone Nvarchar(20) NOT NULL,
Age int,---גיל
Gender Nvarchar(100) NOT NULL,
RankId INT,
FeedBackId INT,
IsAdmin bit NOT NULL Default 0,
FavoriteTeamId int,
FOREIGN KEY(RankId) REFERENCES Ranks(RankId),
FOREIGN KEY(FeedBackId) REFERENCES FeedBacks(FeedBackId),
FOREIGN KEY(FavoriteTeamId) REFERENCES Teams(TeamId),

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
Gate int NOT NULL,
[Row] int,
Seats Int,
TeamId INT,
FOREIGN KEY(TeamId) REFERENCES Teams(TeamId),
UserEmail nvarchar(100),
FOREIGN KEY(UserEmail) REFERENCES Users(Email),
CompetingTeamId INT,
FOREIGN KEY(CompetingTeamId) REFERENCES Teams(TeamId),
);

CREATE TABLE PurchaseRequests(
TicketId INT PRIMARY KEY,
FOREIGN KEY(TicketId) REFERENCES Tickets(TicketId),
BuyerEmail Nvarchar(100) NOT NULL,
FOREIGN KEY(BuyerEmail) REFERENCES Users(Email),
IsAccepted bit NOT NULL Default 0,

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


INSERT INTO Ranks Values('Rookie')
INSERT INTO Ranks Values('Advanced')
INSERT INTO Ranks Values('Admin')

INSERT INTO Teams Values(10000,'Hapoel','Tel Aviv',50)
INSERT INTO Teams Values(10000,'Hapoel','Haifa',50)
INSERT INTO Teams Values(10000,'Maccabi','Tel Aviv',50)

INSERT INTO Users Values('Amir','Amir12345','Amir1@gmail.com','0509991122',31,'Male',2,null,1)
INSERT INTO Users Values('Ran1401','R12345','Amir2@gmail.com','0519991122',45,'Male',2,null,0)
INSERT INTO Users Values('Guy111','Ran12345','Amir3@gmail.com','0529991122',22,'Male',2,null,0)
INSERT INTO Users Values('Bennn','S12345','Amir4@gmail.com','0539991122',19,'Male',2,null,0)
INSERT INTO Users Values('Gal123','A12345','Amir5@gmail.com','0549991122',36,'Female',2,null,0)
INSERT INTO Users Values('Ben1234','C12345','Amir6@gmail.com','0559991122',17,'Female',2,null,0)
INSERT INTO Users Values('Guy123','B12345','Amir7@gmail.com','0569991122',26,'Male',2,null,0)

INSERT INTO Tickets (Price, Gate, [Row], Seats, TeamId,CompetingTeamId ,UserEmail)
VALUES (70,1, 1, 5, (SELECT TeamId FROM Teams WHERE TeamName = 'Hapoel' AND TeamCity = 'Tel Aviv'),
(SELECT TeamId FROM Teams WHERE TeamName = 'Hapoel' AND TeamCity = 'Haifa'),'Amir1@gmail.com')

INSERT INTO Tickets (Price, Gate, [Row], Seats, TeamId, UserEmail)
VALUES (70,1, 7, 7, (SELECT TeamId FROM Teams WHERE TeamName = 'Maccabi' AND TeamCity = 'Tel Aviv'),
(SELECT TeamId FROM Teams WHERE TeamName = 'Hapoel' AND TeamCity = 'Tel Aviv'),'Amir1@gmail.com')

INSERT INTO Tickets (Price, Gate, [Row], Seats, TeamId, UserEmail)
VALUES (70,2, 3, 1, (SELECT TeamId FROM Teams WHERE TeamName = 'Hapoel' AND TeamCity = 'Tel Aviv'),
(SELECT TeamId FROM Teams WHERE TeamName = 'Maccabi' AND TeamCity = 'Ramat Gan'),'Amir1@gmail.com')

INSERT INTO Tickets (Price, Gate, [Row], Seats, TeamId, UserEmail)
VALUES (70,2, 8, 2, (SELECT TeamId FROM Teams WHERE TeamName = 'Maccabi' AND TeamCity = 'Tel Aviv'),
(SELECT TeamId FROM Teams WHERE TeamName = 'Hapoel' AND TeamCity = 'Galil'),'Amir1@gmail.com')

INSERT INTO Tickets (Price, Gate, [Row], Seats, TeamId, UserEmail)
VALUES (70,3, 5, 3, (SELECT TeamId FROM Teams WHERE TeamName = 'Hapoel' AND TeamCity = 'Haifa'),
(SELECT TeamId FROM Teams WHERE TeamName = 'Hapoel' AND TeamCity = 'Tel Aviv'),'Amir1@gmail.com')

INSERT INTO Tickets (Price, Gate, [Row], Seats, TeamId, UserEmail)
VALUES (70,3, 2, 4, (SELECT TeamId FROM Teams WHERE TeamName = 'Hapoel' AND TeamCity = 'Haifa'),
(SELECT TeamId FROM Teams WHERE TeamName = 'Maccabi' AND TeamCity = 'Netanya'),'Amir1@gmail.com')


--select * from Users

/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=Tickets_Server;User ID=AdminLogin;Password=Ran1234;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context TicketsServerDBContext -DataAnnotations -force
*/