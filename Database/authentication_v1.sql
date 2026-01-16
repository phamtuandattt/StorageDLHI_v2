---- Use the correct database
--USE DLHI_TEST;
--GO

--CREATE LOGIN user_test_v1 WITH PASSWORD = 'user_test_v1';
--GO

--CREATE USER user_test_v1 FOR LOGIN user_test_v1;
--GO

--GRANT SELECT, INSERT, UPDATE ON SCHEMA::dbo TO user_test_v1;
--GRANT EXECUTE TO user_test_v1;

---- Insert user
--INSERT INTO STAFFS (ID, STAFF_CODE, STAFF_PWD, STAFF_NAME, STAFF_DEVICE_NAME)
--VALUES ('1cabfc65-146c-41c4-af61-ae917b035783', '', 'user_test_v1', N'user_test_v1', N'DESKTOP-KD2BPDJ')


-- Step 1: Create a SQL Server authenticated user
USE [master];
GO
CREATE LOGIN [YourUsername] WITH PASSWORD = 'YourStrongPassword';
GO

-- Step 2: Create a user in the target database and link it to the login
USE [YourDatabaseName];
GO
CREATE USER [YourUsername] FOR LOGIN [YourUsername];
GO

-- Step 3: Grant SELECT, INSERT, UPDATE permissions on all tables
GRANT SELECT, INSERT, UPDATE ON SCHEMA::dbo TO [YourUsername];
GO

-- Step 4: Grant EXECUTE permission on all stored procedures
GRANT EXECUTE ON SCHEMA::dbo TO [YourUsername];
GO