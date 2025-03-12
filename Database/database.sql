-- Câu lệnh tạo database là: CREATEA DATABASE <DATABASE_NAME>
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

