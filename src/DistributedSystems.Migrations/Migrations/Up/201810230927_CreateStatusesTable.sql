IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Statuses')
BEGIN
    CREATE TABLE Statuses (
		Id TINYINT NOT NULL,
		Name VARCHAR (32) NOT NULL,
		CONSTRAINT PK_Statuses_Id PRIMARY KEY (Id)
    );

	INSERT INTO [dbo].[Statuses] ([Id], [Name]) VALUES (0, 'Errored');
	INSERT INTO [dbo].[Statuses] ([Id], [Name]) VALUES (1, 'Upload Complete');
	INSERT INTO [dbo].[Statuses] ([Id], [Name]) VALUES (2, 'Awaiting Processing');
	INSERT INTO [dbo].[Statuses] ([Id], [Name]) VALUES (3, 'Processing');
	INSERT INTO [dbo].[Statuses] ([Id], [Name]) VALUES (4, 'Re-processing - Low confidence');
	INSERT INTO [dbo].[Statuses] ([Id], [Name]) VALUES (5, 'Complete');
END