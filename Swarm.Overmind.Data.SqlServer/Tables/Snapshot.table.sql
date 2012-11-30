CREATE TABLE [dbo].[Snapshot] (
    [Id]                  BIGINT           IDENTITY (1, 1) NOT NULL,
    [Started]             DATETIME         NOT NULL,
    [Duration]            TIME (7)         NOT NULL,
    [Completed]           INT              NOT NULL,
    [Successful]          INT              NOT NULL,
    [Name]                NVARCHAR (200)   NOT NULL,
    [Failed]              INT              NOT NULL,
    [TimedOut]            INT              NOT NULL,
    [IdleUsers]           INT              NOT NULL,
    [SleepingUsers]       INT              NOT NULL,
    [BusyUsers]           INT              NOT NULL,
    [ExecutionId]         BIGINT           NOT NULL,
    [DroneId]             UNIQUEIDENTIFIER NOT NULL,
    [Average]             FLOAT            NOT NULL,
    [AverageResponseTime] FLOAT            NOT NULL
);



