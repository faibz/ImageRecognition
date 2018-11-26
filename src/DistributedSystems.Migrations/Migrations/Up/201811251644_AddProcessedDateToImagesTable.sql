﻿IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Images') AND COL_LENGTH('Images', 'ProcessedDate') IS NULL
BEGIN
    ALTER TABLE Images
		ADD ProcessedDate DATETIME NULL;
END