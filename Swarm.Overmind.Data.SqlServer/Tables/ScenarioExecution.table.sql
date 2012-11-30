CREATE TABLE [dbo].[ScenarioExecution] (
    [Id]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [ScenarioId] BIGINT        NOT NULL,
    [Started]    DATETIME2 (7) NOT NULL,
    [Finished]   DATETIME2 (7) NULL,
    [Status]     INT           NOT NULL
);

