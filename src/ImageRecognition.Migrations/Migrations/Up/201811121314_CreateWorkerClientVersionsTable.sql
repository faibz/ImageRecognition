﻿IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'WorkerClientVersions')
BEGIN
    CREATE TABLE WorkerClientVersions (
		Version VARCHAR(16) NOT NULL,
		ReleaseDate DATETIME NOT NULL,
		Location VARCHAR(255) NOT NULL,
		Hash VARCHAR(40) NOT NULL,
		CONSTRAINT PK_WorkerClientVersions PRIMARY KEY (Version)
    );
END