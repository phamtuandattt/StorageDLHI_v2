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
EXEC GET_PRODUCTS