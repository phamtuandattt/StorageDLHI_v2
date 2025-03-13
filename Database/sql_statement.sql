-- Phần tạo truy vấn trong SQL 
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