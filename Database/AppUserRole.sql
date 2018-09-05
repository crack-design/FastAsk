CREATE TABLE [dbo].[AppUserRole]
(
	[UserId] INT NOT NULL,
	[RoleId] INT NOT NULL
    PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AppUserRole_User] FOREIGN KEY ([UserId]) REFERENCES [AppUser]([Id]),
    CONSTRAINT [FK_AppUserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [AppRole]([Id])
)
