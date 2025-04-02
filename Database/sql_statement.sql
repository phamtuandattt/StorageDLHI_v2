﻿-- Phần tạo truy vấn trong SQL 
-- Minh cần chú ý nhất là 3 mệnh đề chính SELECT, FROM WHERE
-- SELECT: Mệnh đề SELECT là mệnh đề biểu thị các cột dữ liệu mình cần lấy
-- == VD: Trong bảng SUPPLIERS có 8 cột (ID, NAME, CERT, EMAIL, PHONE, VIETTAT, ADDRESS, BANK_ID). 
-- ==	Mình muốn lấy 3 cột NAME, CERT, EMAIL --> SELECT NAME, CERT, EMAIL
-- ==   Mình muốn lấy 5 cột NAME, CERT, VIETTAT, ADDRESS, BANK_ID --> SELECT NAME, CERT, VIETTAT, ADDRESS, BANK_ID
-- ==   Mình lấy cột nào thì mình SELECT cột đó
SELECT NAME, CERT, EMAIL 
FROM SUPPLIERS

-- ==   Nếu mình muốn lấy hết tất cả các cột --> SELECT *
SELECT *
FROM SUPPLIERS

-- ==   Mình muốn đặt tên cho cột. VD: Thay vì mình SELECT thông thường nó sẽ trả về đúng tên thuộc tính đã tạo trong bảng
-- ==   Chẳng hạn láy cột NAME -> Sẽ trả về tên cột là NAME, lấy cột VIETTAT -> tên cột trả về sẽ là VIETTAT
-- ==   Mình muốn đặt tên cho cột trả về thì sẽ dùng từ khóa AS + '<TÊN MUỐN ĐẶT>'
SELECT NAME AS 'SUPPLIER_NAME', CERT AS 'SUPPLIER_CERT'
FROM SUPPLIERS

-- ==   LƯU Ý: TRONG SQL BẤT KỂ KIỂU LIỆU DẠNG TEXT NÀO CÓ CHƯA DỮ LIỆU LÀ TIẾNG VIỆT THÌ KHI NHẬP LIỆU PHẢI KÈM THEO KÝ TỰ "N" PHÍA TRƯỚC
-- ==          ĐỂ CÓ THỂ HIỂN THỊ TIẾNG VIÊT, CÒN NẾU DẠNG TEXT NHƯNG KHÔNG PHẢI TIẾNG VIỆT THÌ KHÔNG CẦN
-- ==		   CHẲNG HẠN ĐẶT TÊN CỘT TRẢ VỀ BẰNG TIẾNG VIỆT THÌ SẼ LÀ N'NHÀ CUNG CẤP'

SELECT NAME AS N'NHÀ CUNG CẤP', CERT AS N'MÃ SỐ THUẾ'
FROM SUPPLIERS

-- THỰC RA KHI MÌNH THIẾT KẾ CSDL VÀ KẾT NỐI CSDL THÌ MÌNH SẼ KHÔNG CẦN PHẢI ĐỔI TÊN CỘT TRẢ VỀ
-- NÊN GIỮ NGUYÊN TÊN THUỘC TÍNH -> KHI TRUY VẤN TRONG C# SẼ DỄ DÀNG HƠN
-- ĐỔI TÊN CHỈ NÊN ÁP DỤNG TRONG TRƯỜNG HỢP THAO TÁC TOÀN BỘ TRONG SQL SERVER, ĐỂ DỄ HIỂU


-- MỆNH ĐỀ FROM: là mệnh đề bắt buộc phải có trong một câu truy vấn (Query)
-- == Mệnh đề cho biết dữ liệu, các thuộc tính sẽ từ lấy từ những bảng nào
-- VD: lấy dữ liệu từ bảng SUPPLIERS -> ...FROM SUPPLIER -> Câu truy vấn đầy đủ -> SELECT *FROM SUPPLIERS
SELECT *FROM SUPPLIERS
-- ==  Có thể lấy dữ liệu từ nhiều bảng khác nhau bằng cách liệu kê -> ...FROM SUPPLIERS, SUPPLIERS_BANK, ...


-- MỆNH ĐỀ WHERE: Được hiểu là mệnh đề điều kiện, cần sử dụng khi muốn lấy dữ liệu theo điều kiện
-- CÚ PHÁP SỬ DỤNG MÊNH ĐỀ WHERE: ...WHERE <ĐIỀU KIỆN>
-- VD: Lấy những nhà cung cấp có VIETTAT là VB -> SELECT *FROM SUPPLIERS WHERE VIETTAT = 'VB'
SELECT *FROM SUPPLIERS WHERE VIETTAT = 'VB'

-- Mệnh đề WHERE có nhiều dạng: Lấy dữ liệu theo 1 điều kiện, Lấy dữ liệu theo nhiều điều kiện
-- MỘT SỐ DẠNG ĐIÊU KIỆN SỬ DỤNG TRONG MỆNH ĐỀ WHERE
-- <ĐIỀU KIỆN 1> AND <ĐIỀU KIỆN 2> ... - KẾT HỢP NHIỀU ĐIỀU KIỆN
-- == VD: Lấy những Supplier có VIETTAT = VB và địa chỉ = Phú mỹ
SELECT *FROM SUPPLIERS 
WHERE VIETTAT = 'VB' AND ADDRESS = N'Phú Mỹ' 

-- <ĐIỀU KIỆN 1> OR <ĐIỀU KIỆN 2> ... - LẤY ĐIỀU KIỆN 1 HOẶC LẤY ĐIỀU KIỆN 2
-- == VD: Lấy nhà cung cấp có địa chỉ bằng Phú Mỹ hoăc Name = "TEST"
SELECT *FROM SUPPLIERS 
WHERE NAME = N'TEST' OR ADDRESS = N'Phú Mỹ' 
-- == TOÁN TỦ OR sẽ lấy tất cả các dữ liệu thỏa điều kiện
-- => Dòng nào có Name = TEST nó sẽ lấy, dòng nào có ADDRESS = Phú Mỹ nó cũng lấy

-- <THUỘC TÍNH 1> IN <DANH SÁCH GIÁ TRỊ CẦN XÉT> - KIỂM TRA TRONG DANH SÁCH GIÁ TRỊ CẦN XÉT CÓ THUỘC TÍNH 1 TỒN TẠI KHÔNG
-- <THUỘC TÍNH 1> NOT IN <DANH SÁCH GIÁ TRỊ CẦN XÉT> - KIỂM TRA TRONG DANH SÁCH GIÁ TRỊ CẦN XÉT CÓ THUỘC TÍNH 1 TỒN TẠI KHÔNG
-- == HAI PHẦN IN || NOT IN KHI NÀO GẶP DỮ LIỆU CẦN LÀM MÌNH SẼ QUAY LẠI, HIỆN GIỜ CHƯA CÓ VÍ DỤ

-- LIKE - TÌM KIẾM TƯƠNG ĐƯƠNG, THƯỜNG CHỈ ÁP DỤNG CHO KIỂU DỮ LIỆU DẠNG CHUỖI
-- Toán tử LIKE mình sẽ áp dụng thường xuyên trong những câu truy vấn dạng tìm kiếm theo chuỗi
-- Trong toán tử LIKE '%' -> Biểu thị cho một chuỗi bất kỳ 
-- -> Nếu để ...NAME LIKE '%' -> Thì sẽ lấy hết các NAME
SELECT *FROM SUPPLIERS
WHERE NAME LIKE '%'

-- -> Nếu để ...NAME LIKE 'V%' -> Ý nghĩa là lấy NAME bắt đầu có chữ V, dấu % ở sau tượng trưng cho các ký tự còn lại
SELECT *FROM SUPPLIERS
WHERE NAME LIKE 'V%'

-- -> Nếu để ...NAME LIKE '%T' -> Ý nghĩa là lấy NAME có chữ cái kết thúc là T, dấu % ở đầu tượng trưng cho các ký tự ở đầu
SELECT *FROM SUPPLIERS
WHERE NAME LIKE '%T'

-- -> Nếu để ...NAME LIKE '%' + <MỘT KÝ TỰ BẤT KỲ CẦN TÌM> + '%' 
-- -> Ý nghĩa là lấy NAME có chứa <MỘT KÝ TỰ BẤT KỲ CẦN TÌM>
SELECT *FROM SUPPLIERS
WHERE NAME LIKE '%' + 'ES' + '%'
-- ==> Tóm lại: Trong toán tử LIKE ký tự '%' là tượng trung cho cả một chuỗi cần tìm, các ký tự đặt trước ('V%'), đặt sau ('%T), ký tự % là điều kiện để mình tìm


-- BETWEEN - TÌM KIẾM TRONG KHOẢNG, THƯỜNG ÁP DỤNG CHO KIỂU DỮ LIỆU SỐ
-- CÚ PHÁP: SELECT *FROM <TABLE> WHERE <THUỘC TÍNH CẦN XÉT> BETWEEN <GIÁ TRỊ ĐẦU> AND <GIÁ TRỊ CUỐI>
-- VD: Tìm các sản phẩm được nhập từ ngày 1/1/2024 đến ngày 15/1/2024
-- SELECT *FROM NHAPHANG
-- WHERE NGAYNHAP BETWEEN '1/1/2024' AND '15/1/2024'
-- Phần ví dụ này do chưa có dữ liệu thật nên em để VD minh họa

--------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------
--------------------------- PHẦN INSERT DỮ LIỆU VÀO BẢNG -----------------------------
-- CÚ PHẤP:  INSERT INTO <TABLE_NAME> (<DANH SÁCH THUỘC TÍNH CẦN INSERT DỮ LIỆU>) VALUES ('<DANH SÁCH DỮ LIỆU CẦN INSERT>')
-- <TABLE_NAME> : Là tên bảng dữ liệu cần insert dữ liệu. VD: SUPPLIERS, SUPPLIER_BANK,...
-- (<DANH SÁCH THUỘC TÍNH CẦN INSERT DỮ LIỆU>): Phần này mình sẽ liệt kê những thuộc tính nào cần thêm dữ liệu
--												VD: Mình cần INSERT dữ liệu cho 3 thuộc tính NAME, CERT, PHONE của bảng SUPPLIERS
--												=> INSERT INTO SUPPLIER (NAME, CERT, PHONE)
--												=> Phân liệt kê các thuộc tính cần INSERT dữ liệu này sẽ cho phép mình sắp xếp INSERT thuộc tính nào trước
--												thuộc tính nào sau. VD: Mình muốn INSERT thuộc tính NAME, rồi đến PHONE, rồi đến CERT 
--												=> Mình sẽ liệt kê theo thứ tự mình muốn ...SUPPLIER(NAME, PHONE, CERT)
--												- Nếu như mình không liệt kê thứ tự, danh sách các thuộc tính thì mặc định khi INSERT 
--												sẽ theo thứ tự của bảng đã tạo (Thứ tự giống như lúc mình viết câu lệnh CREATE TABLE ....)
--												=> Phần này mình có thể bỏ qua, không cần thiết phải liệt kê, tùy trường hợp cần thì mình mới sử dụng
-- (<DANH SÁCH DỮ LIỆU CẦN INSERT>): Là dữ liệu mà mình muốn thêm vào bảng
--									- CHÚ Ý: DANH SÁCH DỮ LIỆU PHẢI TRÙNG KHỚP VỚI THỨ TỰ CÁC THUỘC TÍNH CỦA BẢNG
-- VD: Bảng SUPPLIERS khi mình tạo các thuộc tính được sắp xếp như sau: (ID, NAME, CERT, EMAIL, PHONE, VIETTAT, ADDRESS, BANK_ID)
--		=> <DANH SÁCH DỮ LIỆU CẦN INSERT> cũng phải được sắp xếp theo thứ tự trên
--		=> ('...ID...', N'NCC_TEXT, '444555333', '0987654321', N'NCC, N'Mỹ xuân', '...BANK_ID...')
-- VD: Thêm dữ liệu cho 3 thuộc tính ID, NAME, CERT của bảng SUPPLIER
-- => INSERT INTO SUPPLIER (ID, NAME, CERT) -> LIỆT KÊ CÁC THUỘC TÍNH CẦN THÊM DỮ LIỆU
-- => VALUES ('...ID...', N'TEST', '000999888') -> THÊM DỮ LIỆU TƯƠNG ỨNG THEO CÁC THUỘC TÍNH ĐÃ LIỆT KÊ
-- VD: Thêm dữ liệu cho thuộc tính ID, PHONE, ADDRESS, BANK_ID của bảng SUPPLIERS
-- => INSERT INTO SUPPLIERS (ID, PHONE, ADDRESS, BANK_ID)
-- => VALUES ('...ID...', '0987654321', N'Mỹ Xuân', '...BANK_ID...')

-- ==> Khi muốn thêm dữ liệu cho tất cả các thuộc tính trong bảng thì mình sẽ không cần phải liệt kê các thuộc tính cần INSERT
-- ==> INSERT INTO SUPPLIER VALUES ('...ID...', N'TEST', '333444555', N'test@gmail.com', '0987654321', 'TE', N'Mỹ Xuân', '...BANK_ID...')



--------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------
--------------------------- PHẦN UDPATE DỮ LIỆU --------------------------------------
-- CÚ PHÁP CỦA UPDATE:
--			UPDATE <TABLE_NAME> SET <DANH SÁCH CÁC THUỘC TÍNH CẦN UPDATE> WHERE <ĐIÊU KIỆN ĐỂ UPDATE>
-- <TABLE_NAME>: Là bảng mình sẽ thực hiện thao tác UPDATE. VD: SUPPLIERS, SUPPLIER_BANK,...

-- <DANH SÁCH CÁC THUỘC TÍNH CẦN UPDATE>: Liệt kê các thuộc tính và giá trị cần update tương ứng.
-- VD: UPDATE thông tin nhà cung cấp. Tên nhà cung cấp = TEST 111, Cert = 000999000, địa chỉ = Phú mỹ
-- Áp dụng cú pháp => UPDATE SUPPLIERS SET NAME = N'TEST 111, CERT = '000999000', ADDRESS = N'Phú Mỹ'(1)

-- <ĐIỀU KIỆN ĐỂ UPDATE>: Phần này sẽ cho biết mình cần UPDATE thông tin cho đối tượng nào
-- VD: UPDATE thông tin nhà cung cấp có ID = '...'. Với NAME = N'TEST 222', Cert = '999888777', ADDRESS = N'Bà Rịa'
-- Áp dụng cú pháp => UPDATE SUPPLIERS SET NAME = N'TEST 222', CERT = '999888777', ADDRESS = N'Bà Rịa' WHERE ID = '...'(2)

-- Câu lệnh UPDATE(1): Sẽ UPDATE thông tin NAME, CERT, ADDRESS mới cho tất cả các nhà cũng cấp. Do mình không chỉ định cụ thể điều kiện UPDATE
-- Câu lệnh UPDATE(2): Sẽ chỉ UPDATE thông tin mới cho nhà cung cấp có ID = '...'.
-- Mình cũng có thể UPDATE thông tin theo nhiều điều kiện, và liệt kê các điều kiện trong Hàm WHERE tương tự như mình SELECT dữ liệu
-- VD: Update ADDRESS = N'Vũng Tàu', EMAIL = N'vt@gmail.com' cho nhà cung cấp có NAME = N'TEST 222' và CERT = '999888777'
--		=> UPDATE SUPPLIERS SET ADDRESS = N'Vũng Tàu', EMAIL = N'vt@gmail.com' WHERE NAME = N'TEST 222' AND CERT = '999888777'
--		=> SQL sẽ tìm những NCC thỏa 2 điều kiện NAME = N'TEST 222' và CERT = '999888777' để UPDATE thông tin
--		=> Ngược lại nếu không có dòng nào thì sẽ không UPDATE
-- Phần UPDATE sẽ còn nhiều phần nâng cao hơn nhưng do mình chưa có dữ liệu và điều kiện cụ thể nên mình chưa Ví Dụ được



--------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------
--------------------------- PHẦN DELETE DỮ LIỆU --------------------------------------
-- CÚ PHÁP:    DELETE <TABLE_NAME> WHERE <ĐIỀU KIỆN XÓA>
-- <ĐIỀU KIỆN XÓA>: Tùy theo yêu cầu mình sẽ xét điều kiện để xóa
-- VD: Xóa nhà cung cấp có tên TEST, trong bảng SUPPLIER_BANKS => Mình sẽ xét điều kiện NAME = N'TEST' 
--		=> SQL sẽ tìm tất cả các dòng dữ liệu có NAME = N'TEST' trong bảng SUPPLIER_BANK để xóa
--		=> DELETE SUPPLIER_BANKS WHERE NAME = N'TEST'
--		=> DELETE SUPPLIER_BANKS WHERE ID = '6d86f5eb-b33c-43f6-921d-a21a6d6b1dfb' -> DELETE đòng nào có ID = '...'
-- Mình cũng có thể xóa theo nhiều điều kiện: NAME = N'TEST' và BANK_ACCOUNT = '777777777'
-- => DELETE SUPPLIER_BANKS WHERE NAME = N'TEST' AND BANK_ACCOUNT = '777777777'

-- LƯU Ý QUAN TRỌNG: KHI THỰC HIỆN XÓA CÁC DÒNG DỮ LIỆU MÀ CÓ CHƯA KHÓA KHOẠI THÌ PHẢI LÀN LƯỢT XÓA THẰNG CHA TRƯỚC KHI XÓA THẰNG CON
-- VD: Trong bảng SUPPLIER_BANK  có tài khoản ABC là của SUPPLIER Đất mới
--		=> Mình đã logic từ trước thì thằng SUPPLIERS là cha của SUPPLIER_BANK
--		=> Mình phải xóa thằng có chứa KHÓA NGOẠI TRƯỚC => Xóa thằng SUPPLIER VƯƠNG BỬU TRƯỚC
--		=> Sau đó mình mới có thể xóa thằng SUPPLIER_BANK ABC
-- ==> PHẢI XÓA THẰNG CÓ CHỨA KHÓA NGOẠI TRƯỚC SAU ĐÓ MỚI XÓA ĐƯỢC THẰNG THAM CHIẾU ĐẾN NÓ

select *from SUPPLIERS
select *from SUPPLIER_BANKS

SELECT *FROM ORIGINS
INSERT INTO ORIGINS VALUES ('', '', N'')
UPDATE ORIGINS SET ORIGIN_CODE = '', ORIGIN_NAME = N'' WHERE ID = ''

SELECT *FROM MATERIAL_TYPES
INSERT INTO MATERIAL_TYPES VALUES ('', '', N'')
UPDATE MATERIAL_TYPES SET TYPE_CODE = '', TYPE_DES = N'' WHERE ID = ''

SELECT *FROM MATERIAL_STANDARD
INSERT INTO MATERIAL_STANDARD VALUES ('', '', N'')
UPDATE MATERIAL_STANDARD SET STANDARD_CODE = '', STANDARD_DES = N'' WHERE ID = ''


SELECT *FROM TAX
INSERT INTO TAX VALUES ('', N'')
UPDATE TAX SET TAX_PERCENT = N'' WHERE ID = ''

SELECT *FROM UNITS
INSERT INTO UNITS VALUES ('', N'')
UPDATE UNITS SET UNIT_CODE = N'' WHERE ID = ''

SELECT *FROM COST
INSERT INTO COST VALUES ('', N'')
UPDATE COST SET COST_NAME = N'' WHERE ID = ''

-- Modify COLUMN
ALTER TABLE TAX
ALTER COLUMN TAX_PERCENT NVARCHAR(50);

SELECT *FROM SUPPLIERS
UPDATE SUPPLIERS SET NAME = N'', CERT = '', EMAIL = N'', PHONE = '', VIETTAT = N'', ADDRESS = N'' WHERE ID = ''
SELECT *FROM SUPPLIER_BANKS WHERE SUPPLIER_ID = 'FDDA2976-2BB1-433D-AC04-08CC3360F182'
UPDATE SUPPLIER_BANKS SET BANK_ACCOUNT = '', BANK_NAME = N'', BANK_BENEFICIAL = N'' WHERE ID = ''

SELECT *FROM SUPPLIER_BANKS WHERE ID = '00000000-0000-0000-0000-000000000000'
DELETE SUPPLIERS WHERE ID = '7D51BBA0-6EE6-4ED6-9842-B68101237764'

UPDATE SUPPLIERS SET NAME = N'Vương Bửu', CERT = '0310200231', EMAIL = N'tranding-hcm@ipc-vietnam.com.vn', PHONE = '35472342', VIETTAT = N'VB', ADDRESS = N'mỹ' WHERE ID = 'fdda2976-2bb1-433d-ac04-08cc3360f182'