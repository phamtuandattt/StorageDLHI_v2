﻿-- Câu lệnh tạo database là: CREATEA DATABASE <DATABASE_NAME>
-- Từ khóa GO để cho sql hiểu là bắt đầu một phần khác, mà không cần cũng được luôn
CREATE DATABASE DLHI_V2
-- Để sử dụng cái database vừa tạo mình có thể sử dụng câu lệnh hoặc chọn vào cái này
-- Khi tạo xong trong dropdown sẽ có thêm một cái db mình mới tạo
USE DLHI_V2


-- Tạo bảng supplier theo phân tích

-- Đầu tiên là phải xác định kiểu dữ liệu của các thuộc tính
-- ID 
-- - Nếu mà để như cái của quynh là IDENTITY(1, 1) thì khi mình tạo 1 record mới nó sẽ tự tăng 1,2,3,....
-- - Có một hạn chế khi sử dụng IDENTITY(1, 1) là khi mình xóa thằng bất kỳ thì cái số ID nó sẽ k có cập nhật
-- - Hiện này người ta ưa dùng kiểu dữ liệu UNIQUEIDENTIFIER để làm ID ("53e060e3-b055-4b35-ac03-74af56b9b5ba")
-- - Cái này người ta dùng thuật toán để tạo ra mã với khả năng trùng dường như = 0
-- - Nên minh cũng xài cái này. Cái này trong code C# lập trình nó là Guid(), còn trong SQL là UNIQUEIDENTIFIER

-- Tiếp theo mình làm những thuộc tính còn lại
-- Trong SQL có 3 kiểu dữ liệu có thể lưu trữ dạng text
-- NVARCHAR() - Lưu dạng text tiếng việt
-- VARCHAR(), CHAR() - Cũng lưu dạng text nhưng cái Char() không lưu được Tiếng Việt. còn cái varchar() em quên mất ròi
-- Cơ bản khi lưu dữ liệu Tiếng Việt em sẽ dùng NVARCHAR()
-- Cú pháp sẽ là NVARCHAR(MAX) -> MAX sẽ là chiều dài chuỗi, nếu không để MAX thì có thể để số lượng ký tự. VD: NVARCHAR(10), NVARCHAR(200)
-- Giờ mình sẽ tạo trong bảng

-- Sau khi đã có các thuộc tính, mình sẽ xác định khóa chính khóa ngoại của bảng
-- Cách đơn giản nhất để xác định khóa chính là mình sẽ xem cái thông tin nào là không thể trùng, và có thể trùng
-- ID: Là một thuộc tính được tạo tự động và không thể nào trùng -> Khi truyền ID mình sẽ lấy được các thông tin khác như tên, địa chỉ,...
-- NAME: Thuộc tính có thể bị trùng chẳng hạn có nhiều cửa hàng giống tên -> Khi mình truyền tên vào sẽ xuất hiện nhiều cửa hàng giống tên, sql sẽ không biết mình cần lấy thằng nào
-- CERT: Mã số thuế chắc có lẽ không trùng -> Khi truyền mã số thuế mình cũng sẽ chỉ lấy được 1 nhà cung cấp duy nhất
-- PHONE: Có thể trùng
-- VIETTAT: Chắc chắc chắn trùng haha:)))
-- ADDRESS: cũng có thể trùng


-- Thiết kế, tạo khóa chính
-- Thuộc tính khỏa chính phải thỏa 2 điều kiện PHẢI LÀ DUY NHẤT và KHÔNG ĐƯỢC NULL
-- Mình xác định thuộc tính ID là khóa nên phải chỉnh có nó là NOT NULL
-- Bước 1: thêm NOT NULL và thuộc tính khóa chính -> Không cho null
-- Bước 2: Tạo khóa chính bằng cú pháp CONSTRAINT <NAME> PRIMARY KEY (<THUỘC TÍNH KHÓA CHÍNH>)

-- Thiết kế, tạo khóa ngoại
-- Trước khi tạo khóa ngoại mình cần xác định quan hệ của 2 bảng trước
-- Đầu tiên 
-- -- Mỗi supplier sẽ có 1 hoặc nhiều tài khoàn 
-- -- Ngược lại 1 tài khoản sẽ chỉ thuộc về 1 nhà cùng cấp
-- Để hiểu rõ hơn cái quan hệ này thì ví dụ: Mình có một NCC có ID: 123, mình không biết thông tin tài khoản gì của nó hết, mình chỉ biết được tên, phone,...
-- Khi mình thêm thuộc tính BANKS_ID thì sau khi mình có được ID của nhà cung cấp 123, mình cũng sẽ lấy được BANKS_ID của nó.
-- Và sau khi biết được BANKS_ID của nó mình có thể truy xuất ra thông tin của nó như BANK_ACCOUNT, BANK_NAME... thông qua thuộc tính đó là khóa ngoại
-- Như em nói trước Thì mình có thể không cần tạo thêm một bảng xong rồi tạo khóa ngoại làm gì hết cho nó phiền nhưng khi gộp chung
-- VD: Có 1 dòng record:
-- - 123 - ABC - 000000 - "" - "" - "" - 00555555 (BANK_ACCOUNT) -> Thì mình sẽ biết được thằng 123 này có số tài khoản này
-- - Nhưng khi mình thêm 1 tài khoản nữa cho nhà cũng cấp này nó sẽ đẻ ra một dòng dữ liệu khác
-- - 111 (Lúc này không phải 123 nữa vì ID là mình sinh tự động) - ABC - 000000 - "" - "" - "" - 00555556 (BANK_ACCOUNT)
-- -> Như vậy về mặt bên ngoài thì mình biết là cái tài khoảng 00555555, 00555556 là của ABC nhưng máy nó sẽ không hiểu, bỏi vì ID của 2 cái khác nhau
-- Sau một thời gian mình lỡ quên thì mình cũng k biết nó là của đứa nào
-- Với lại cách mà dồn tất cả các thuộc tính vào một bảng nó chỉ khả thi với những CSDL (Cơ sở dữ liệu) nhỏ và ít thuộc tính, không thích hợp với CSDL lớn
-- Công dụng chính quan trọng nhất của khóa ngoại là nó sẽ quản lý dữ liệu theo dạng CHA - CON
-- VD: Biết thằng cha thì sẽ biết được tất cả những thằng con của nó và bỏ đi được nhiều dữ liệu dư thừa khi truy vấn
-- Để tạo được khóa ngoại cái quan trọng nhất là phải xác định được mối quan hệ giữa các bảng với nhau

-- MỐI QUAN HỆ: 1 SUPPLIERS SẼ CÓ 1 HOẶC NHIỀU BANKS, NGƯỢC LẠI 1 BANKS CHỈ THUỘC VỀ 1 NHÀ CUNG CẤP
-- Mình sẽ thiết kế, và thêm thuộc tính cho các bảng dựa theo câu IN HOA trên

-- Thì ở đây trong bảng SUPPLIERS mình sẽ thêm một thuộc tính SUPPLIER_BANK_ID để làm khóa ngoại tham chiếu đến bảng SUPPLIER_BANKS
-- 

GO
CREATE TABLE SUPPLIERS (
	ID UNIQUEIDENTIFIER NOT NULL, -- Thêm NOT NULL và thuộc tính khóa chính -> Khi vô tình thêm mà không truyền giá trị cho ID thì nó sẽ báo lỗi k cho nhập
	NAME NVARCHAR(MAX),
	CERT CHAR(10), -- Dữ liệu đã có thể giới hạn thì nên để giới hạn cho đỡ tốn bộ nhớ, và do dữ liệu không có chữ nên mình dùng CHAR được gòi
	EMAIL NVARCHAR(MAX),
	PHONE CHAR(12), -- Phone mình sẽ kiểm tra trong code
	VIETTAT NVARCHAR(50),
	ADDRESS NVARCHAR(MAX),

	-- Tạo khóa chính
	CONSTRAINT PK_SUPPLIERS PRIMARY KEY (ID),
	
	-- THÊM THUỘC TÍNH KHÓA NGOẠI
	-- Cho về đầu tiên: 1 SUPPLIERS SẼ CÓ 1 HOẶC NHIỀU BANKS
	-- Này nói dân dã và có cái mẹo dễ hiểu dễ làm nhất, là thằng nào làm cha thì thằng đó có thuộc tính con
	-- Mình sẽ cần tạo 1 thuộc tính con -> Theo cú pháp em nghĩ ra cho dễ nhận biết <TÊN CON>_ID -> BANK_ID
	-- Thuộc tính con có thể KHÁC TÊN với KHÓA CHÍNH trong bảng CON, nhưng BẮT BUỘC phải CÙNG KIỂ DỮ LIỆU
	BANK_ID UNIQUEIDENTIFIER, -- Tạo thuộc tính khóa ngoại
	-- TẠO KHÓA NGOẠI theo cú pháp CONSTRAINT FK_<BẢNG CHA>_<BẢNG CON> FOREIGN KEY <THUỘC TÍNH KHÓA NGOAI> REFERNCES <BẢNG CON>(<THUỘC TÍNH KHÓA CHÍNH CỦA BẢNG CON>)
	CONSTRAINT FK_SUPPLIERS_SUPPLIER_BANKS FOREIGN KEY (BANK_ID) REFERENCES SUPPLIER_BANKS(ID),
)

-- QUan trọng nhất là xác định được mối quan hệ giữa các bảng, và xác định được 2 bảng CÓ THỂ KẾT NỐI VỚI NHAU thông qua thuộc tính nào
-- Ở đây mình đã xác định được thuộc tính ID là thuộc tính kết nối giữa 2 bảng rồi
-- MỘT LƯU Ý QUAN TRỌNG NHẤT: LÀ THUỘC TÍNH KHÓA NGOAI CỦA BẢNG CHA PHẢI LÀ KHÓA CHÍNH CỦA BẢNG CON
-- Có nghĩa là trong bảng Con cái thuộc tính ID BẮT BUỘC phải là KHÓA CHÍNH

-- Bảng SUPPLIERS này chưa xong nha
-- Tiếp theo mình tạo bảng BANKS để lưu thông tin tài khoản của các Suppliers
-- TẠO BẢNG SUPPLIER_BANKS
-- Theo như bảng dữ liệu mình sẽ có 3 thuộc tính: Số tài khoản, bank_name, chi nhánh
-- Vẫn theo quy tắc cũ mình sẽ tạo cho mỗi một tài khoản 1 ID duy nhất
-- Xác định khóa chính là một thuộc tính duy nhất không thể trùng -> ID
-- Tạo khóa chính theo 2 quy tắc DUY NHẤT và NOT NULL
-- B1: Xác định khóa chính và thêm NOT NULL
-- B2: Tạo khóa chính với cú pháp: CONSTRAINT <NAME> PRIMARY KEY (<THUỘC TÍNH KHÓA CHÍNH>)
GO
CREATE TABLE SUPPLIER_BANKS (
	-- B1: Xác định khóa chính và thêm NOT NULL
	ID UNIQUEIDENTIFIER NOT NULL,
	BANK_ACCOUNT CHAR(50), -- STK chỉ chứa sổ và không thể nào hơn 50 ký tự nên mình gắn cứng độ dài để đỡ tốn bộ nhớ
	BANK_NAME NVARCHAR(500), -- Có tiếng việt không biết độ dài
	BANK_BENEFICIAL NVARCHAR(500), -- Có tiếng việt không biết độ dài

	-- B2: Tạo khóa chính với cú pháp: CONSTRAINT <NAME> PRIMARY KEY (<THUỘC TÍNH KHÓA CHÍNH>)
	CONSTRAINT PK_SUPPLIER_BANKS PRIMARY KEY (ID)
)

CREATE TABLE COMPANY_INFOS (
	ID UNIQUEIDENTIFIER NOT NULL,
	COMPANY_NAME NVARCHAR(100),
	COMPANY_CERT CHAR(13),
	COMPANY_PHONE CHAR(15),

	CONSTRAINT PK_COMPANY_INFOS PRIMARY KEY (ID)
)

CREATE TABLE DEPARMENTS (
	ID UNIQUEIDENTIFIER NOT NULL,
	DEP_NAME NVARCHAR(100),

	CONSTRAINT PK_DEPARMENTS PRIMARY KEY (ID)
)

CREATE TABLE STAFF_ROLE (
	ID UNIQUEIDENTIFIER NOT NULL,
	ROLE_NAME NVARCHAR(50),
	ROLE_ACTION NVARCHAR(100),

	CONSTRAINT PK_STAFF_ROLE PRIMARY KEY (ID),
)

CREATE TABLE STAFFS (
	ID UNIQUEIDENTIFIER NOT NULL,
	STAFF_CODE CHAR(10), --DEPARTMENT_CODE + STT_STAFF: 0000100001 -> DEP: PURCHASE(00001) + STT_STAFF: 00001
	STAFF_PWD CHAR(50),
	STAFF_NAME NVARCHAR(50),

	DEPARMENT_ID UNIQUEIDENTIFIER,
	STAFF_ROLE_ID UNIQUEIDENTIFIER,

	CONSTRAINT PK_STAFFS PRIMARY KEY (ID),
	CONSTRAINT FK_STAFFS_DEP FOREIGN KEY (DEPARMENT_ID) REFERENCES DEPARMENTS(ID),
	CONSTRAINT FK_STAFFS_STAFF_ROLES FOREIGN KEY (STAFF_ROLE_ID) REFERENCES STAFF_ROLE(ID)
)

CREATE TABLE ORIGINS (
	ID UNIQUEIDENTIFIER NOT NULL,
	ORIGIN_CODE CHAR(5),
	ORIGIN_NAME NVARCHAR(100),

	CONSTRAINT PK_ORIGIN PRIMARY KEY (ID)
)


CREATE TABLE MATERIAL_TYPES (
	ID UNIQUEIDENTIFIER NOT NULL,
	TYPE_CODE CHAR(5),
	TYPE_DES NVARCHAR(50),

	CONSTRAINT PK_MATERIAL_GROUPS PRIMARY KEY(ID)
)

CREATE TABLE MATERIAL_STANDARD (
	ID UNIQUEIDENTIFIER NOT NULL,
	STANDARD_CODE CHAR(3),
	STANDARD_DES NVARCHAR(50),

	CONSTRAINT PK_MATERIAL_STANDARD PRIMARY KEY (ID)
)

CREATE TABLE TAX (
	ID UNIQUEIDENTIFIER NOT NULL,
	TAX_PERCENT NVARCHAR(5),

	CONSTRAINT PK_TAX PRIMARY KEY (ID)
)

CREATE TABLE COST (
	ID UNIQUEIDENTIFIER NOT NULL,
	COST_NAME NVARCHAR(50),

	CONSTRAINT PK_COST PRIMARY KEY (ID)
)

CREATE TABLE UNITS (
	ID UNIQUEIDENTIFIER NOT NULL,
	UNIT_CODE NVARCHAR(10),

	CONSTRAINT PK_UNITS PRIMARY KEY (ID)
)

CREATE TABLE MATERIAL_SOURCE (
	ID UNIQUEIDENTIFIER NOT NULL,
	MATERIAL_SOURCE_NAME NVARCHAR(50),

	CONSTRAINT PK_MATERIAL_SOURCE PRIMARY KEY (ID)
)

CREATE TABLE PRODUCT_TYPES (
	ID UNIQUEIDENTIFIER NOT NULL,
	PRODUCT_TYPE_NAME NVARCHAR(100),

	CONSTRAINT PK_PRODUCT_TYPES PRIMARY KEY (ID),
)

CREATE TABLE PRODUCTS (
	ID UNIQUEIDENTIFIER NOT NULL,
	PRODUCT_NAME NVARCHAR(100),
	PRODUCT_DES_2 CHAR(100),
	PRODUCT_CODE CHAR(20), -- ORGIRIN(2) + TYPES_CODE(5) + STANDARD_CODE(3) + SIZE (...)
	PRODUCT_MATERIAL_CODE CHAR(20), -- STANDARD_NAME
	PICTURE_LINK NVARCHAR(100),
	PICTURE VARBINARY(MAX),
	A_THINHNESS CHAR(10),
	B_DEPTH CHAR(10),
	C_WIDTH CHAR(10),
	D_WEB CHAR(10),
	E_FLAG CHAR(10),
	F_LENGTH CHAR(10),
	G_WEIGHT CHAR(10),
	USED_NOTE NVARCHAR(100),

	UNIT_ID UNIQUEIDENTIFIER,
	PRODUCT_TYPE_ID UNIQUEIDENTIFIER,

	CONSTRAINT PK_PRODUCTS PRIMARY KEY (ID),
	CONSTRAINT FK_PRODUCTS_UNIT FOREIGN KEY (UNIT_ID) REFERENCES UNITS(ID),
	CONSTRAINT FK_PRODUCTS_PRODUCT_TYPES FOREIGN KEY (PRODUCT_TYPE_ID) REFERENCES PRODUCT_TYPES(ID)
)

CREATE TABLE MPRS (
	ID UNIQUEIDENTIFIER NOT NULL,
	MPR_NO CHAR(50),
	MPR_WO_NO CHAR(50),
	MPR_PROJECT_NAME CHAR(50),
	MPR_REV_TOTAL INT,
	MPR_CREATE_DATE DATETIME,
	MPR_EXPECTED_DELIVERY_DATE DATETIME,
	MPR_PREPARED NVARCHAR(100),
	MPR_REVIEWED NVARCHAR(100),
	MPR_APPROVED NVARCHAR(100),

	STAFF_ID UNIQUEIDENTIFIER,

	CONSTRAINT PK_MPRS PRIMARY KEY (ID),
	CONSTRAINT FK_MPRS_STAFFS FOREIGN KEY (STAFF_ID) REFERENCES STAFFS(ID),
)

CREATE TABLE MPR_DETAIL (
	ID UNIQUEIDENTIFIER NOT NULL,
	MPR_ID UNIQUEIDENTIFIER NOT NULL,
	PRODUCT_ID UNIQUEIDENTIFIER NOT NULL,
	MPR_QTY INT,
	USAGE NVARCHAR(50),
	MPS CHAR(50),
	REV INT,
	DWG_BOQRECEIVE_DATE CHAR(50),
	ISSUE_DATE NVARCHAR(100),
	REQ_DATE DATETIME,
	MPR_REMARKS NVARCHAR(200),

	CONSTRAINT PK_MPR_DETAIL PRIMARY KEY (ID, MPR_ID, PRODUCT_ID),
	CONSTRAINT FK_MPR_DETAIL_MPR FOREIGN KEY (MPR_ID) REFERENCES MPRS(ID),
	CONSTRAINT FK_MPR_DETAIL_PRODUCTS FOREIGN KEY (PRODUCT_ID) REFERENCES PRODUCTS(ID),
)

CREATE TABLE POS (
	ID UNIQUEIDENTIFIER NOT NULL,
	PO_NO CHAR(50),
	PO_MPR_NO CHAR(50),
	PO_WO_NO CHAR(50),
	PO_PROJECT_NAME CHAR(50),
	PO_REV_TOTAL INT,
	PO_CREATE_DATE DATETIME,
	PO_EXPECTED_DELIVERY_DATE DATETIME,
	PO_PREPARED NVARCHAR(100),
	PO_REVIEWED NVARCHAR(100),
	PO_AGREEMENT NVARCHAR(100), -- Staff nane
	PO_APPROVED NVARCHAR(100),
	PO_PAYMENT_TERM NVARCHAR(100),
	PO_DISPATCH_BOX NVARCHAR(100),
	PO_TOTAL_AMOUNT BIGINT,

	COST_ID UNIQUEIDENTIFIER,
	TAX_ID UNIQUEIDENTIFIER,
	SUPPLIER_ID UNIQUEIDENTIFIER,
	STAFF_ID UNIQUEIDENTIFIER,

	CONSTRAINT PK_POS PRIMARY KEY (ID),
	CONSTRAINT FK_POS_COST FOREIGN KEY (COST_ID) REFERENCES COST(ID),
	CONSTRAINT FK_POS_TAX FOREIGN KEY (TAX_ID) REFERENCES TAX(ID),
	CONSTRAINT FK_POS_SUPPLIER FOREIGN KEY (SUPPLIER_ID) REFERENCES SUPPLIERS(ID),
	CONSTRAINT FK_POS_STAFFS FOREIGN KEY (STAFF_ID) REFERENCES STAFFS(ID),
)

CREATE TABLE PO_DETAIL (
	ID UNIQUEIDENTIFIER NOT NULL,
	PO_ID UNIQUEIDENTIFIER NOT NULL,
	PO_PRODUCT_ID UNIQUEIDENTIFIER NOT NULL,
	PO_QTY INT,
	PO_PRICE BIGINT,
	PO_AMOUNT BIGINT,
	REQ_DATE DATETIME,
	PO_RECEVIE NVARCHAR(100),
	PO_REMARKS NVARCHAR(100),

	CONSTRAINT PK_PO_DETAIL PRIMARY KEY (ID, PO_ID, PO_PRODUCT_ID),
	CONSTRAINT FK_PO_DETAIL_PO FOREIGN KEY (PO_ID) REFERENCES POS(ID),
	CONSTRAINT FK_PO_DETAIL_PRODUCTS FOREIGN KEY (PO_PRODUCT_ID) REFERENCES PRODUCTS(ID)
)

CREATE TABLE WAREHOUSES (
	ID UNIQUEIDENTIFIER NOT NULL,
	WAREHOUSE_CODE CHAR(20),
	WAREHOUSE_NAME NVARCHAR(50),
	WAREHOUSE_ADDRESS NVARCHAR(100),

	CONSTRAINT PK_WAREHOSES PRIMARY KEY (ID),
)

CREATE TABLE WAREHOUSE_DETAIL (
	ID UNIQUEIDENTIFIER NOT NULL,
	WAREHOUSE_ID UNIQUEIDENTIFIER NOT NULL,
	PRODUCT_ID UNIQUEIDENTIFIER NOT NULL,
	PRODUCT_IN_STOCK INT,

	CONSTRAINT PK_WAREHOUSE_DETAIL PRIMARY KEY (ID, WAREHOUSE_ID, PRODUCT_ID),
	CONSTRAINT FK_WAREHOSE_DETAIL_WAREHOUSE FOREIGN KEY (WAREHOUSE_ID) REFERENCES WAREHOUSES(ID),
	CONSTRAINT FK_WAREHOSE_DETAIL_PRODUCT FOREIGN KEY (PRODUCT_ID) REFERENCES PRODUCTS(ID)
)

CREATE TABLE IMPORT_PRODUCTS (
	ID UNIQUEIDENTIFIER NOT NULL,
	IMPORT_DATE DATETIME,
	IMPORT_DAY INT,
	IMPORT_MONTH INT,
	IMPORT_YEAR INT,
	IMPORT_QTY_TOTAL INT,
	
	WAREHOUSE_ID UNIQUEIDENTIFIER,
	STAFF_ID UNIQUEIDENTIFIER,

	CONSTRAINT PK_IMPORT_PRODUCTS PRIMARY KEY (ID),
	CONSTRAINT FK_IMPORT_PRODUCTS_WAREHOUSE FOREIGN KEY (WAREHOUSE_ID) REFERENCES WAREHOUSES(ID),
	CONSTRAINT FK_IMPORT_PRODUCTS_STAFFS FOREIGN KEY (STAFF_ID) REFERENCES STAFFS(ID),
)

CREATE TABLE IMPORT_PRODUCT_DETAIL (
	ID UNIQUEIDENTIFIER NOT NULL,
	IMPORT_PRODUCT_ID UNIQUEIDENTIFIER NOT NULL,
	PRODUCT_ID UNIQUEIDENTIFIER NOT NULL,
	QTY INT,

	CONSTRAINT PK_IMPORT_PRODUCT_DETAIL PRIMARY KEY (ID, IMPORT_PRODUCT_ID, PRODUCT_ID),
	CONSTRAINT FK_IMPORT_PRODUCT_DETAIL_IMPORT_PRODUCT FOREIGN KEY (IMPORT_PRODUCT_ID) REFERENCES IMPORT_PRODUCTS(ID),
	CONSTRAINT FK_IMPORT_PRODUCT_DETAIL_PRODUCTS FOREIGN KEY (PRODUCT_ID) REFERENCES PRODUCTS(ID),
)

CREATE TABLE DELIVERY_PRODUCTS (
	ID UNIQUEIDENTIFIER NOT NULL,
	DELIVERY_DATE DATETIME,
	DELIVERY_DAY INT,
	DELIVERY_MONTH INT,
	DELIVERY_YEAR INT,
	DELIVERY_QTY_TOTAL INT,

	WAREHOUSE_ID UNIQUEIDENTIFIER,
	STAFF_ID UNIQUEIDENTIFIER,

	CONSTRAINT PK_DELIVERY_PRODUCTS PRIMARY KEY (ID),
	CONSTRAINT FK_DELIVERY_PRODUCTS_WAREHOUSE FOREIGN KEY (WAREHOUSE_ID) REFERENCES WAREHOUSES(ID),
	CONSTRAINT FK_DELIVERY_PRODUCTS_STAFFS FOREIGN KEY (STAFF_ID) REFERENCES STAFFS(ID),
)

CREATE TABLE DELIVERY_PRODUCT_DETAIL (
	ID UNIQUEIDENTIFIER NOT NULL,
	DELIVERY_PRODUCT_ID UNIQUEIDENTIFIER NOT NULL,
	PRODUCT_ID UNIQUEIDENTIFIER NOT NULL,
	QTY INT,

	CONSTRAINT PK_DELIVERY_PRODUCT_DETAIL PRIMARY KEY (ID, DELIVERY_PRODUCT_ID, PRODUCT_ID),
	CONSTRAINT FK_DELIVERY_PRODUCT_DETAIL_DELIVERY_PRODUCT FOREIGN KEY (DELIVERY_PRODUCT_ID) REFERENCES DELIVERY_PRODUCTS(ID),
	CONSTRAINT FK_DELIVERY_PRODUCT_DETAIL_PRODUCTS FOREIGN KEY (PRODUCT_ID) REFERENCES PRODUCTS(ID),
)