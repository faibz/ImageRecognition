IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CompoundImages') AND COL_LENGTH('CompoundImages', 'UploadedDate') IS NULL AND COL_LENGTH('CompoundImages', 'ProcessedDate') IS NULL
BEGIN
    ALTER TABLE CompoundImages 
		ADD UploadedDate DATETIME NOT NULL,
			ProcessedDate DATETIME NULL;
END