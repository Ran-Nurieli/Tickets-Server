namespace Tickets_Server
{

    public class DataBaseCreation
    {
     ﻿Use master
        Go
        IF EXISTS (SELECT * FROM sys.databases WHERE name = N'Tickets_Server')
        BEGIN
            DROP DATABASE MyAppName_DB;
        END

        Go
        Create Database Tickets_Server
        Go

        Use Tickets_Server
        Go

        CREATE TABLE Users(
        Username Nvarchar(100), ---שם משתמש
        [Password] Nvarchar(100) PRIMARY KEY,   --- סיסמה- מפתח ראשי
        Age int,---גיל
        Gender Nvarchar(100),
        FOREIGN KEY(RankId) REFERENCES Ranks(RankId),
        FOREIGN KEY(FeedBackType) REFERENCES FeedBacks(FeedBackType),

        
        );

        CREATE TABLE Tickets(
        TicketId Nvarchar(1000) PRIMARY KEY IDENTITY,
        Price int,
        Place Nvarchar(100),
        [Row] int,
        Seats Int,
        FOREIGN KEY(TeamId) REFERENCES Teams(TeamId),
        

        );

        Create Table Teams(
        TeamId int PRIMARY KEY IDENTITY,
        Capacity int,
        TeamName Nvarchar(100),
        TeamCity Nvarchar(100),
        PriceForTicket int,
        FOREIGN KEY(TicketsId) REFERENCES Tickets(TicketsId),

        );

        CREATE TABLE Ranks(
        RankId int PRIMARY KEY,
        RankType Nvarchar(100)
        );

        CREATE TABLE FeedBacks(
        FeedBackId int PRIMARY KEY IDENTITY,
        FeedBackType int,
        [FeedBackType] int,
        Info Nvarchar(1000)
        );

        


    }
}
