ALTER TABLE [dbo].[ScenarioExecution]
    ADD CONSTRAINT [FK_ScenarioExecution_Scenario] FOREIGN KEY ([ScenarioId]) REFERENCES [dbo].[Scenario] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

