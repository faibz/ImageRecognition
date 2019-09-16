IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Maps') AND COL_LENGTH('Maps', 'ColumnCnt') IS NOT NULL AND COL_LENGTH('Maps', 'RowCnt') IS NOT NULL
BEGIN
	EXEC sp_rename 'dbo.Maps.ColumnCnt', 'ColumnCount', 'COLUMN';
	EXEC sp_rename 'dbo.Maps.RowCnt', 'RowCount', 'COLUMN';
END