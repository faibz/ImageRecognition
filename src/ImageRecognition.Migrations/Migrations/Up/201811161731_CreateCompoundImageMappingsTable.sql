IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CompoundImageMappings')
BEGIN
    CREATE TABLE CompoundImageMappings (
		ImageId UNIQUEIDENTIFIER NOT NULL,
		CompoundImageId UNIQUEIDENTIFIER NOT NULL,
		CONSTRAINT PK_CompoundImageMappings PRIMARY KEY (ImageId, CompoundImageId),
		CONSTRAINT FK_CompoundImageMappings_ImageId_Images_Id FOREIGN KEY (ImageId) REFERENCES Images(Id),
		CONSTRAINT FK_CompoundImageMappings_CompoundImageId_CompoundImages_Id FOREIGN KEY (CompoundImageId) REFERENCES CompoundImages(Id)
    );
END