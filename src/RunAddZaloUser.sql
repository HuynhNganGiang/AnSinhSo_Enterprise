CREATE TABLE [ZaloUsers] (
    [Id] uniqueidentifier NOT NULL,
    [ZaloId] nvarchar(100) NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    [Avatar] nvarchar(1000) NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [CitizenId] uniqueidentifier NULL,
    [CitizenIdentityId] uniqueidentifier NULL,
    [IsFollowing] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_ZaloUsers] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_ZaloUsers_CitizenId] ON [ZaloUsers] ([CitizenId]);
GO

CREATE INDEX [IX_ZaloUsers_CitizenIdentityId] ON [ZaloUsers] ([CitizenIdentityId]);
GO

CREATE UNIQUE INDEX [IX_ZaloUsers_ZaloId] ON [ZaloUsers] ([ZaloId]);
GO
