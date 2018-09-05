CREATE TABLE [dbo].[AppUser]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [UserName] NVARCHAR(256) NOT NULL,
    [Email] NVARCHAR(256) NULL,
    [EmailConfirmed] BIT NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NULL,
    [PhoneNumber] NVARCHAR(20) NULL,
    [PhoneNumberConfirmed] BIT NOT NULL,
    [TwoFactorEnabled] BIT NOT NULL
)

GO

CREATE INDEX [IX_AppUser_UserName] ON [dbo].[AppUser] ([UserName])

GO

CREATE INDEX [IX_AppUser_Email] ON [dbo].[AppUser] ([Email])
