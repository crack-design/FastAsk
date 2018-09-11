CREATE TABLE [dbo].[AppUser]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [UserName] NVARCHAR(256) NOT NULL,
    [Email] NVARCHAR(256) NULL,
    [PasswordHash] NVARCHAR(MAX) NULL,
)

GO

CREATE INDEX [IX_AppUser_UserName] ON [dbo].[AppUser] ([UserName])

GO

CREATE INDEX [IX_AppUser_Email] ON [dbo].[AppUser] ([Email])
