﻿IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CompoundImageMappings')
BEGIN
    DROP TABLE CompoundImageMappings;
END