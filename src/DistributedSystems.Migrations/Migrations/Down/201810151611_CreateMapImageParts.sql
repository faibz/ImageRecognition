﻿IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'MapImageParts')
BEGIN
    DROP TABLE MapImageParts;
END