CREATE TABLE [dbo].[Log] (
    [Id]         BIGINT IDENTITY (1, 1) NOT NULL,
    [Date]       DATETIME       NOT NULL,
    [Thread]     VARCHAR (255)  NOT NULL,
    [Level]      VARCHAR (50)   NOT NULL,
    [Logger]     VARCHAR (255)  NOT NULL,
    [Message]    VARCHAR (4000) NOT NULL,
    [Exception]  VARCHAR (512)  NULL,
    [StackTrace] VARCHAR (2000) NULL,
    [SQL]        VARCHAR (2000) NULL,
    [RequestUrl] VARCHAR (2000) NULL
);