using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App
{
    public partial class Supplier_bank_GUI : Form
    {
        // Mình sẽ khởi tạo giá trị ban đầu cho chuỗi connection = null
        // Mình sẽ gán giá trị cho nó trong hàm khởi tạo
        // giá trị = null giống như giá trị rỗng ""
        // Trong C# để khởi tạo một đối tượng không có giá trị thì mình sẽ gán cho nó bằng NULL hoặc bằng ""
        private SqlConnection _connection = null;


        // Hàm khởi tạo
        public Supplier_bank_GUI()
        {
            InitializeComponent();
            _connection = new SqlConnection("server=DESKTOP-KD2BPDJ;database=DLHI_v2;Integrated Security = true;uid=sa;pwd=Aa123456@");
        }

        // Hàm xử lý khi nhấn btn add
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Để đảm bảo quá trình kết nối và truy vấn dữ liệu trong SQL được thực hiện và sẽ trả về lỗi nếu như có vấn đề gì 
            // Mình sẽ phải đặt nó trong một vòng try {} catch {} -> Bắt buộc
            try
            {
                // Mở kết nối từ C# xuống CSDL
                // Đầu tiên mình cần kiểm tra xem trạng thái của chuỗi kết nối (có 2 trạng thái OPEN() || CLOSE())
                // --Trạng thái Open(): là trạng thái mà đường kết nối xuống CSDL đã được mở và có thể truy vấn xuống CSDL
                // --Trạng thái Close(): là trạng thái đường kết nối xuống CSDL đã bị đóng và không thể truy vấn xuống CSDL
                // Khi bắt đầu truy vấn xuống CSDL mình cần kiểm tra xem là đường kết nối có được mở hay chưa, nếu đang ở trạng thái đóng (Close())
                // -> Mở kết nối (Chuyển trạng thái thành Open()) -> Mới có thể truy vấn
                // -- Thư viện System.Data.SqlClient cung cấp cho mình thuộc tính State để biểu thị trạng thại hoạt động của đường kết nối
                if (_connection.State == ConnectionState.Closed) // Kiểm tra trạng thái
                {
                    _connection.Open(); // Mở kết nối
                }


                // .... Thao tác truy vấn
                // ----------- B1: Tạo câu truy vấn. Tạo câu truy vấn INSERT 
                // --> "INSERT INTO SUPPLIER_BANKS VALUES ('<ID>', '<BANK_ACCOUNT>', N'<BANK_NAME>', N'BANK_BENEFICIAL')"
                // --> Mình sẽ lấy dữ liệu từ các textbox đã nhập
                // --> Theo cú pháp: <TEXTBOX_ID>.Text -> Sẽ lấy text của textbox. VD: txtBankAccount.Text, txtBankName.Text
                // Tiếp theo mình sẽ sử dụng câu lệnh: string.Format() để có thể truyền dữ liệu vào câu truy vấn
                // Minh sẽ khai báo câu truy vấn với các thuộc tính tương ứng cần thay đổi là {0}, {1},...
                // Như câu sql bên dưỡi:
                // - {0} : Là ID
                // - {1} : Là Account
                // - {2} : Là Name
                // - {3} : Là Beneficial
                // Câu lệnh string.Format gồm 2 phần
                // - Phân câu lệnh sql: INSERT INTO SUPPLIER_BANKS VALUES ('{0}', '{1}', N'{2}', N'{3}')
                // - Phần tham số: Guid.NewGuid(), txtAcc.Text, txtName.Text, txtBeneficial.Text
                // string.Format sẽ tự đông thay thế các giá trị phần tham số vào các số {0}, {1} tương ứng
                // {0} = Guid.NewGuid()
                // {1} = txtAcc.Text
                // {2} = txtName.Text
                // {3} = txtBeneficial.Text
                string sqlQuery = string.Format("INSERT INTO SUPPLIER_BANKS VALUES ('{0}', '{1}', N'{2}', N'{3}')",
                    Guid.NewGuid(), txtAcc.Text, txtName.Text, txtBeneficial.Text);


                // Trong C# khi truy vấn dữ liệu sẽ trả về 3 dạng dữ liệu
                // - Dạng 1: DataTable -> Dữ liệu dạng bảng. VD: Khi lấy dữ liệu SUPPLIER_BANKS 
                //                     -> Dữ liệu nhân được sẽ có cấu trúc giống như bảng trong SQL
                //      - VD: Kết quả của câu truy vấn SELECT *FROM SUPPLIER_BANKS
                //              ID - BANK_ACCOUNT - BANK_NAME - BANK_BENEFICIAL 
                //             ... - 222333444555 - VƯƠNG BỬU - VƯƠNG BỬU

                //      --> Thì kết quả mà DATATABLE nhân được cũng có cấu trúc như vậy
                //              ID - BANK_ACCOUNT - BANK_NAME - BANK_BENEFICIAL 
                //             ... - 222333444555 - VƯƠNG BỬU - VƯƠNG BỬU
                // => Dạng 1 sẽ thường được sử dụng cho các thao tác LẤY dữ liệu trong Database và hiển thị lên GridData

                // - Dạng 2: Trả về số -1, 0, 1 -> Dạng 2 sẽ được sử dụng khi thực hiện các thao tác THÊM, XÓA, SỬA
                //          - Khi thao tác THÊM, XÓA, SỬA trả về giá trị = 1 => THÊM, XÓA, SỬA THÀNH CÔNG và không có lỗi
                //          - Ngược lại khi trả về -1 => Thao tác không thành công, xảy ra lỗi
                //          - Khi trả về 0 => Thao tác được thực hiện nhưng không có gì thay đổi (Ít xảy ra)

                // - Dạng 3: Trả về TRUE || FALSE
                //          - Dạng này thường chỉ sử dụng khi Update DATABASE
                //          - Thông thường đối với những bảng nhiều item, thao tác THÊM, XÓA, SỬA, nhập xuất nhiều mình sẽ chỉ lấy dữ liệu lần đâu
                //          - Sau đó thao tác trên bảng mình lấy ban đâu -> Cuối cùng sau khi kết thúc tất cả các thao tác mình sẽ UPDATE xuống DATABASE một lần
                //          - Như vậy sẽ hạn chế truy xuất xuống SQL quá nhiều, vì truy xuất quá nhiều cũng sẽ gây lag
                //          - Ví dụ như một lần mình thêm 100 dòng, thì SQL sẽ phải nhận lệnh INSERT 100 lần
                //              -> Đối với số lượng thao tác ít thực hiện gọi INSERT sẽ rất nhanh và chính xác, không tốn thời gian
                //              -> Đối với số lượng thao tác nhiều thì gọi 100 lần INSERT, đầu tiên sẽ tốn bộ nhớ và tài nguyên, giảm hiệu xuất
                //          => Chỉ gọi INSERT => Khi số lượng thao tác it
                //          - Dạng 3 sẽ trả về giá trị TRUE nếu như mình UPDATE cả một BẢNG thành công
                //          - Trả về FALSE nếu như không thành công


                // ----------- B2: Thực hiện tạo SqlCommand
                // Trong phần sự kiện Add này mình sẽ thực hiện thao tác và trả về kiểu dạng 2
                // Để thực hiện được câu SQL INSERT mình cần sử dụng SqlCommand
                // SqlCommand sẽ giúp tạo câu query và truyền xuống SQL nó sẽ hiểu là câu này là một câu lệnh SQL, SQL sẽ phải thực thi nó
                // SqlCommand sẽ cần 2 tham số
                //  1. Câu lệnh SQL (Mình đã tạo phía trên) 
                //  2. chuỗi connection (Mình đã khởi tạo phía trên)
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection);


                // ----------- B3: THực hiện câu sql và nhận về kết quả
                // -- Khúc này thì mình có thể học thuộc cái cấu trúc này
                // -- Để thực hiện và nhận kết quả (Thành công || Không thành công) mình sẽ dùng phương thức ExecuteNoneQuery()
                // -> Trả về 1: Thực hiện thao tác thành công
                // -> Trả về -1: Thực hiện không thành công
                // -> Trả về 0: Trường hợp này em chưa thấy bao giờ nên cũng không biết giải thích sao :))
                int rs = sqlCommand.ExecuteNonQuery();


                // ----------- B4: Kiểm tra kết quả và thông báo cho người dùng biết
                if (rs == 1)
                {
                    MessageBox.Show("Thành công");
                }
                else
                {
                    MessageBox.Show("Không thành công");
                }


                // Sau khi truy vấn xong mình sẽ thực hiện đóng kết nối 
                // -- Mình có thể không cần đóng -> Cũng không ảnh hưởng đến hoạt động
                // -- Nhưng khi không đóng nếu như phần mềm hoạt động một thời gian dài, sẽ chiếm đường kết nối lâu, khi đó truy vấn nhiều sẽ bị đơ hoặc tắc nghẽn
                // -- Để tốt nhất mình nên đóng kết nối sau khi sử dụng
                _connection.Close();
            }
            // Mình sẽ sử dụng thư viện có sẵn SqlException để phát hiện lỗi khi thực hiện SQL
            catch (SqlException ex)
            {
                // Mình sẽ đưa ra 1 cảnh báo lỗi
                throw new Exception(ex.Message);
            }


           

        }

        // Hàm xử lý khi nhấn btn Modify
        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        // Hàm xử lý khi nhấn Load
        private void btnLoad_Click(object sender, EventArgs e)
        {

        }
    }
}
