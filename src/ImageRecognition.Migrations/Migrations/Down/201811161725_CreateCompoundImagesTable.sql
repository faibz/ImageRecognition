﻿IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CompoundImages')
BEGIN
    DROP TABLE CompoundImages;
END