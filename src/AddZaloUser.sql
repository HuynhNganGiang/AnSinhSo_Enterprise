BEGIN TRANSACTION;
GO

DROP TABLE [PaymentDetails];
GO

EXEC sp_rename N'[Payments].[PolicyId]', N'WelfareCaseId', N'COLUMN';
GO

EXEC sp_rename N'[Payments].[IX_Payments_PolicyId]', N'IX_Payments_WelfareCaseId', N'INDEX';
GO

ALTER TABLE [Payments] ADD [ActualPaymentDate] datetime2 NULL;
GO

ALTER TABLE [Payments] ADD [Amount] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [Payments] ADD [CitizenId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
GO

ALTER TABLE [Payments] ADD [HouseholdId] uniqueidentifier NULL;
GO

ALTER TABLE [Payments] ADD [Method] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Payments] ADD [Notes] nvarchar(2000) NULL;
GO

ALTER TABLE [Payments] ADD [PaymentNumber] nvarchar(50) NOT NULL DEFAULT N'';
GO

ALTER TABLE [Payments] ADD [ScheduledDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

ALTER TABLE [Households] ADD [HouseholdCode] nvarchar(20) NOT NULL DEFAULT N'';
GO

ALTER TABLE [Households] ADD [Latitude] float NULL;
GO

ALTER TABLE [Households] ADD [Longitude] float NULL;
GO

ALTER TABLE [HouseholdMembers] ADD [RelationshipTypeId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
GO

ALTER TABLE [Citizens] ADD [Latitude] float NULL;
GO

ALTER TABLE [Citizens] ADD [Longitude] float NULL;
GO

CREATE TABLE [AiRecommendations] (
    [Id] uniqueidentifier NOT NULL,
    [RecommendationNumber] nvarchar(50) NOT NULL,
    [TargetType] int NOT NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [RuleVersion] nvarchar(20) NOT NULL,
    [Category] int NOT NULL,
    [Score] int NOT NULL,
    [Confidence] int NOT NULL,
    [Title] nvarchar(200) NOT NULL,
    [Summary] nvarchar(1000) NOT NULL,
    [Status] int NOT NULL,
    [ReviewedBy] nvarchar(100) NULL,
    [ReviewedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [Reasons] nvarchar(max) NULL,
    CONSTRAINT [PK_AiRecommendations] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Notifications] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(255) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [RecipientCitizenId] uniqueidentifier NOT NULL,
    [Channel] nvarchar(max) NOT NULL,
    [Priority] nvarchar(max) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [ScheduledAt] datetime2 NULL,
    [SentAt] datetime2 NULL,
    [ReadAt] datetime2 NULL,
    [RetryCount] int NOT NULL,
    [CorrelationId] uniqueidentifier NOT NULL,
    [SourceModule] nvarchar(max) NULL,
    [SourceId] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PaymentPoints] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(50) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Street] nvarchar(200) NOT NULL,
    [Ward] nvarchar(100) NOT NULL,
    [District] nvarchar(100) NOT NULL,
    [Province] nvarchar(100) NOT NULL,
    [PostalCode] nvarchar(20) NOT NULL,
    [Latitude] float NULL,
    [Longitude] float NULL,
    [Status] int NOT NULL,
    [Description] nvarchar(500) NULL,
    [DisplayOrder] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_PaymentPoints] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [RelationshipTypes] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(50) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_RelationshipTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [WelfarePrograms] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(50) NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    [Description] nvarchar(1000) NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_WelfarePrograms] PRIMARY KEY ([Id])
);
GO

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

CREATE TABLE [NotificationHistories] (
    [Id] uniqueidentifier NOT NULL,
    [NotificationId] uniqueidentifier NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Timestamp] datetime2 NOT NULL,
    [Note] nvarchar(1000) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_NotificationHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_NotificationHistories_Notifications_NotificationId] FOREIGN KEY ([NotificationId]) REFERENCES [Notifications] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [WelfareCases] (
    [Id] uniqueidentifier NOT NULL,
    [CitizenId] uniqueidentifier NOT NULL,
    [HouseholdId] uniqueidentifier NULL,
    [ProgramId] uniqueidentifier NOT NULL,
    [Snapshot_CitizenNumber] nvarchar(20) NOT NULL,
    [Snapshot_FullName] nvarchar(255) NOT NULL,
    [Snapshot_DateOfBirth] datetime2 NOT NULL,
    [Snapshot_Gender] nvarchar(20) NOT NULL,
    [Snapshot_HouseholdCode] nvarchar(50) NULL,
    [Snapshot_Address] nvarchar(1000) NOT NULL,
    [Snapshot_Phone] nvarchar(20) NULL,
    [Snapshot_CreatedAt] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [Notes] nvarchar(2000) NULL,
    [BenefitAmount] decimal(18,2) NULL,
    [EffectiveFrom] datetime2 NULL,
    [EffectiveTo] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_WelfareCases] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WelfareCases_Citizens_CitizenId] FOREIGN KEY ([CitizenId]) REFERENCES [Citizens] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_WelfareCases_WelfarePrograms_ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [WelfarePrograms] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Payments_CitizenId] ON [Payments] ([CitizenId]);
GO

CREATE INDEX [IX_Payments_HouseholdId] ON [Payments] ([HouseholdId]);
GO

CREATE UNIQUE INDEX [IX_Payments_PaymentNumber] ON [Payments] ([PaymentNumber]);
GO

CREATE UNIQUE INDEX [IX_Households_HouseholdCode] ON [Households] ([HouseholdCode]);
GO

CREATE UNIQUE INDEX [IX_AiRecommendations_RecommendationNumber] ON [AiRecommendations] ([RecommendationNumber]);
GO

CREATE INDEX [IX_AiRecommendations_TargetType_TargetId] ON [AiRecommendations] ([TargetType], [TargetId]);
GO

CREATE INDEX [IX_NotificationHistories_NotificationId] ON [NotificationHistories] ([NotificationId]);
GO

CREATE UNIQUE INDEX [IX_PaymentPoints_Code] ON [PaymentPoints] ([Code]);
GO

CREATE UNIQUE INDEX [IX_RelationshipTypes_Code] ON [RelationshipTypes] ([Code]);
GO

CREATE INDEX [IX_WelfareCases_CitizenId] ON [WelfareCases] ([CitizenId]);
GO

CREATE INDEX [IX_WelfareCases_ProgramId] ON [WelfareCases] ([ProgramId]);
GO

CREATE UNIQUE INDEX [IX_WelfarePrograms_Code] ON [WelfarePrograms] ([Code]);
GO

CREATE INDEX [IX_ZaloUsers_CitizenId] ON [ZaloUsers] ([CitizenId]);
GO

CREATE INDEX [IX_ZaloUsers_CitizenIdentityId] ON [ZaloUsers] ([CitizenIdentityId]);
GO

CREATE UNIQUE INDEX [IX_ZaloUsers_ZaloId] ON [ZaloUsers] ([ZaloId]);
GO

ALTER TABLE [Payments] ADD CONSTRAINT [FK_Payments_Citizens_CitizenId] FOREIGN KEY ([CitizenId]) REFERENCES [Citizens] ([Id]) ON DELETE NO ACTION;
GO

ALTER TABLE [Payments] ADD CONSTRAINT [FK_Payments_WelfareCases_WelfareCaseId] FOREIGN KEY ([WelfareCaseId]) REFERENCES [WelfareCases] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260817041918_AddZaloUser', N'8.0.0');
GO

COMMIT;
GO

