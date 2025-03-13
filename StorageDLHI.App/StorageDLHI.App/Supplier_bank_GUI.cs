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
            


            // Sau khi truy vấn xong mình sẽ thực hiện đóng kết nối 
            // -- Mình có thể không cần đóng -> Cũng không ảnh hưởng đến hoạt động
            // -- Nhưng khi không đóng nếu như phần mềm hoạt động một thời gian dài, sẽ chiếm đường kết nối lâu, khi đó truy vấn nhiều sẽ bị đơ hoặc tắc nghẽn
            // -- Để tốt nhất mình nên đóng kết nối sau khi sử dụng
            _connection.Close();

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
