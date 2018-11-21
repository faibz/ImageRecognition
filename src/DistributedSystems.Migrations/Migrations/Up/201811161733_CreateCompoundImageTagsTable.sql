IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CompoundImageTags')
BEGIN
    CREATE TABLE CompoundImageTags (
		CompoundImageId UNIQUEIDENTIFIER NOT NULL,
		Tag VARCHAR(16) NOT NULL,
		Confidence FLOAT NOT NULL,
		CONSTRAINT PK_CompoundImageTags PRIMARY KEY (CompoundImageId, Tag),
		CONSTRAINT FK_CompoundImageTags_CompoundImages_Id FOREIGN KEY (CompoundImageId) REFERENCES CompoundImages(Id)
    );
END