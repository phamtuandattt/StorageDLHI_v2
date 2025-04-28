USE master;
GO
-- Create user
CREATE LOGIN user_ky_thuat
	WITH PASSWORD = N'123456@',
	CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF,
	DEFAULT_DATABASE = DLHI_V2;
GO

USE DLHI_V2
GO
CREATE USER user_ky_thuat FOR LOGIN user_ky_thuat
GO

-- Create the Role
CREATE ROLE ky_thuat_role;

-- Grant Table Permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON YourTable TO LimitedUserRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON AnotherTable TO LimitedUserRole; -- Repeat for other tables

-- Grant Execute Permission on Stored Procedures
GRANT EXECUTE ON YourStoredProcedure TO LimitedUserRole;
GRANT EXECUTE ON AnotherStoredProcedure TO LimitedUserRole; -- Repeat for other procedures

-- Add Users to the Role
EXEC sp_addrolemember 'LimitedUserRole', 'YourUser';