-------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------
-- Get product for create MPR
GO
CREATE PROC GET_PRODUCTS
AS
BEGIN
	SELECT PRODUCTS.ID, PRODUCT_NAME, PRODUCT_DES_2, PRODUCT_CODE, PRODUCT_MATERIAL_CODE, PICTURE,
		A_THINHNESS, B_DEPTH, C_WIDTH, D_WEB, E_FLAG, F_LENGTH, G_WEIGHT, UNIT_CODE, USED_NOTE,
		PICTURE_LINK, PRODUCTS.UNIT_ID AS UNIT_ID, ORIGIN_ID, M_TYPE_ID, STANDARD_ID
	FROM PRODUCTS, UNITS
	WHERE PRODUCTS.UNIT_ID = UNITS.ID
END
GO

-- Get MPR detail
GO
CREATE PROC GET_MPR_DETAIL @MPR_ID UNIQUEIDENTIFIER 
AS
BEGIN
	SELECT MPR_DETAIL.ID, MPR_DETAIL.MPR_ID, MPR_DETAIL.PRODUCT_ID, PRODUCTS.PRODUCT_NAME, PRODUCTS.PRODUCT_DES_2, PRODUCTS.PRODUCT_MATERIAL_CODE,
		PRODUCTS.A_THINHNESS, PRODUCTS.B_DEPTH, PRODUCTS.C_WIDTH, PRODUCTS.D_WEB, PRODUCTS.E_FLAG, PRODUCTS.F_LENGTH, PRODUCTS.G_WEIGHT, 
		MPR_DETAIL.MPR_QTY, MPR_DETAIL.USAGE, MPR_DETAIL.ISSUE_DATE, MPR_DETAIL.MPR_REMARKS
	FROM MPR_DETAIL, PRODUCTS
	WHERE MPR_DETAIL.PRODUCT_ID = PRODUCTS.ID
		AND MPR_DETAIL.MPR_ID = @MPR_ID
END
GO

-- Get PO
GO
CREATE PROC GET_POS 
AS
BEGIN
	SELECT POS.ID, PO_NO, PO_MPR_NO, PO_WO_NO, PO_PROJECT_NAME, PO_REV_TOTAL, PO_CREATE_DATE, PO_EXPECTED_DELIVERY_DATE, PO_PREPARED,
		PO_REVIEWED, PO_AGREEMENT, PO_APPROVED, PO_PAYMENT_TERM, PO_PLACE_OF_CONTRY, PO_TOTAL_AMOUNT,
		COST.COST_NAME, TAX.TAX_PERCENT, SUPPLIERS.NAME
	FROM POS, COST, TAX, SUPPLIERS
	WHERE POS.COST_ID = COST.ID AND POS.TAX_ID = TAX.ID AND POS.SUPPLIER_ID = SUPPLIERS.ID
END
GO


-- Get PO for Import product
GO
CREATE PROC GET_POS_FOR_IMPORT_PRODUCT
AS
BEGIN
	SELECT POS.ID, PO_NO, PO_MPR_NO, PO_WO_NO, PO_PROJECT_NAME, PO_REV_TOTAL, PO_CREATE_DATE, PO_EXPECTED_DELIVERY_DATE, PO_PREPARED,
		PO_REVIEWED, PO_AGREEMENT, PO_APPROVED, PO_PAYMENT_TERM, PO_PLACE_OF_CONTRY, PO_TOTAL_AMOUNT,
		COST.COST_NAME, TAX.TAX_PERCENT, SUPPLIERS.NAME
	FROM POS, COST, TAX, SUPPLIERS
	WHERE POS.COST_ID = COST.ID AND POS.TAX_ID = TAX.ID AND POS.SUPPLIER_ID = SUPPLIERS.ID AND POS.IS_IMPORTED = 0
END
GO
EXEC GET_POS_FOR_IMPORT_PRODUCT
-- Get PO detail
GO
CREATE PROC GET_PO_DETAIL @PO_ID UNIQUEIDENTIFIER 
AS
BEGIN
	SELECT PO_DETAIL.ID, PO_DETAIL.PO_ID, PO_DETAIL.PO_PRODUCT_ID, PRODUCTS.PRODUCT_NAME, PRODUCTS.PRODUCT_DES_2, PRODUCTS.PRODUCT_MATERIAL_CODE,
		PRODUCTS.A_THINHNESS, PRODUCTS.B_DEPTH, PRODUCTS.C_WIDTH, PRODUCTS.D_WEB, PRODUCTS.E_FLAG, PRODUCTS.F_LENGTH, PRODUCTS.G_WEIGHT, 
		PO_DETAIL.PO_QTY AS PO_DETAIL_QTY, PO_DETAIL.PO_PRICE AS PO_DETAIL_PRICE, PO_DETAIL.PO_AMOUNT AS PO_DETAIL_AMOUNT
	FROM PO_DETAIL, PRODUCTS
	WHERE PO_DETAIL.PO_PRODUCT_ID = PRODUCTS.ID
		AND PO_DETAIL.PO_ID = @PO_ID
END
GO

-- GET Import 
GO
CREATE PROC GET_IMPORTS
AS
BEGIN
	SELECT IMPORT_PRODUCTS.ID, FROM_PO_NO, IMPORT_DATE, STAFF_NAME
	FROM IMPORT_PRODUCTS, STAFFS
	WHERE IMPORT_PRODUCTS.STAFF_ID = STAFFS.ID
END
GO
EXEC GET_IMPORTS

-- GET Import detail by ID
GO
CREATE PROC GET_IMPORT_DETAIL_BY_ID @IMPORT_ID UNIQUEIDENTIFIER
AS
BEGIN
	SELECT IMPORT_PRODUCT_DETAIL.ID, IMPORT_PRODUCT_ID, IMPORT_PRODUCT_DETAIL.PRODUCT_ID, 
	A_THINHNESS, B_DEPTH, C_WIDTH, D_WEB, E_FLAG, F_LENGTH, G_WEIGHT, QTY, IMPORT_PRODUCT_DETAIL.WAREHOUSE_ID, WAREHOUSE_NAME
	FROM IMPORT_PRODUCT_DETAIL, PRODUCTS, WAREHOUSES
	WHERE IMPORT_PRODUCT_DETAIL.PRODUCT_ID = PRODUCTS.ID AND IMPORT_PRODUCT_DETAIL.WAREHOUSE_ID = WAREHOUSES.ID
		AND IMPORT_PRODUCT_DETAIL.IMPORT_PRODUCT_ID = @IMPORT_ID
	ORDER BY WAREHOUSE_NAME
END
GO
EXEC GET_IMPORT_DETAIL_BY_ID '221C221F-6BB6-4EBA-BE7E-A33F8AFB58C8'

-- GET detail of warehosue
GO
CREATE PROC GET_WAREHOUSE_DETAILBY_ID @WAREHOUSE_ID UNIQUEIDENTIFIER
AS
BEGIN
	SELECT WAREHOUSE_DETAIL.ID, WAREHOUSE_DETAIL.WAREHOUSE_ID, WAREHOUSE_DETAIL.PRODUCT_ID, PRODUCT_NAME,
	PRODUCT_CODE, PRODUCT_DES_2, PRODUCT_MATERIAL_CODE,
	A_THINHNESS, B_DEPTH, C_WIDTH, D_WEB, E_FLAG, F_LENGTH, G_WEIGHT, WAREHOUSE_DETAIL.PRODUCT_IN_STOCK
	FROM WAREHOUSE_DETAIL, PRODUCTS
	WHERE WAREHOUSE_DETAIL.PRODUCT_ID = PRODUCTS.ID AND WAREHOUSE_DETAIL.WAREHOUSE_ID = @WAREHOUSE_ID
	ORDER BY PRODUCT_MATERIAL_CODE
END
GO
EXEC GET_WAREHOUSE_DETAILBY_ID 'BF85327F-F958-4494-AFE3-0DECA1B4B8E5'
SELECT *FROM PRODUCTS

-- UPDATE WAREHOUSE STOCK
GO
CREATE PROCEDURE [dbo].[UpdateWarehouseStock]
    @Items WarehouseImportType READONLY,
    @ResultCode INT OUTPUT  -- New output parameter
AS
BEGIN
    SET NOCOUNT OFF;  -- Enable row count so we can capture it

    BEGIN TRY
        MERGE WAREHOUSE_DETAIL AS target
        USING @Items AS source
        ON target.WAREHOUSE_ID = source.WAREHOUSE_ID
           AND target.PRODUCT_ID = source.PRODUCT_ID

        WHEN MATCHED THEN
            UPDATE SET PRODUCT_IN_STOCK = target.PRODUCT_IN_STOCK + source.PRODUCT_IN_STOCK

        WHEN NOT MATCHED THEN
            INSERT (ID, WAREHOUSE_ID, PRODUCT_ID, PRODUCT_IN_STOCK)
            VALUES (source.ID, source.WAREHOUSE_ID, source.PRODUCT_ID, source.PRODUCT_IN_STOCK);

        -- Success: set return code to row count
        SET @ResultCode = @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        -- Failure: -1 means error
        SET @ResultCode = -1;
    END CATCH
END
