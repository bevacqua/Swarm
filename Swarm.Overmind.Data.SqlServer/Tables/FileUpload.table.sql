CREATE TABLE [dbo].[FileUpload] (
    [Id]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [Code] UNIQUEIDENTIFIER NOT NULL,
    [Date] DATETIME2 (7)    NOT NULL,
    [Path] NVARCHAR (150)   NOT NULL
);

