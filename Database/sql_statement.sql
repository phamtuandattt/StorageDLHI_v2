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

SELECT *FROM SUPPLIER_BANKS
INSERT INTO SUPPLIER_BANKS VALUES ('', N'', N'',N'')