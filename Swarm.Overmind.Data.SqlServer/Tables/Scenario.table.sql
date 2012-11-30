CREATE TABLE [dbo].[Scenario] (
    [Id]               BIGINT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (255)    NOT NULL,
    [VirtualUsers]     INT              NOT NULL,
    [SleepTime]        TIME (7)         NOT NULL,
    [RampUpTime]       TIME (7)         NOT NULL,
    [SamplingInterval] TIME (7)         NOT NULL,
    [RequestTimeout]   TIME (7)         NOT NULL,
    [Method]           INT              NOT NULL,
    [Endpoint]         VARCHAR (1024)   NOT NULL,
    [LogLevel]         INT              NOT NULL,
    [FileCode]         UNIQUEIDENTIFIER NOT NULL
);

