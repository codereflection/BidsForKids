USE master;
GO
CREATE DATABASE BidForKids;
GO
-- Verify the database files and sizes
SELECT name, size, size*1.0/128 AS [Size in MBs] 
FROM sys.master_files
WHERE name = N'BidForKids';
GO
