IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Maps') AND COL_LENGTH('Maps', 'ColumnCount') IS NOT NULL AND COL_LENGTH('Maps', 'RowCount') IS NOT NULL
BEGIN
	EXEC sp_rename 'dbo.Maps.ColumnCount', 'ColumnCnt', 'COLUMN';
	EXEC sp_rename 'dbo.Maps.RowCount', 'RowCnt', 'COLUMN';
END