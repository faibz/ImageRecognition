﻿IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CompoundImageTags')
BEGIN
    DROP TABLE CompoundImageTags;
END