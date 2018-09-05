CREATE TABLE [dbo].[AppRole]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(256) NOT NULL
)

GO

CREATE INDEX [IX_ApplRole_NName] ON [dbo].[AppRole] ([Name])
