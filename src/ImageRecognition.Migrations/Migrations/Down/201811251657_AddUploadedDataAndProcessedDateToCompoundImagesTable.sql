IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CompoundImages') AND COL_LENGTH('CompoundImages', 'UploadedDate') IS NOT NULL AND COL_LENGTH('CompoundImages', 'ProcessedDate') IS NOT NULL 
BEGIN
    ALTER TABLE CompoundImages DROP COLUMN UploadedDate;
	ALTER TABLE CompoundImages DROP COLUMN ProcessedDate;
END