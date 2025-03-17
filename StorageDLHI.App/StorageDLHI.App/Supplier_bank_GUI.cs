using StorageDLHI.DAL.Models;
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
            // Tạo chuỗi connection
            _connection = new SqlConnection("server=DESKTOP-KD2BPDJ;database=DLHI_v2;Integrated Security = true;uid=sa;pwd=Aa123456@");


            // Gọi hàm LoadData() để khi bắt đầu chương trình sẽ lấy dữ liệu từ CSDL lên và hiển thị trước
            LoadData();
        }

        private void LoadData()
        {
            // Trước khi thao tác với CSDL luôn luôn nhớ đặt trong 1 vòng try - catch
            try
            {
                // Mở đường kết nối xuống CSDL
                if (_connection.State == ConnectionState.Closed) // Kiểm tra trạng thái
                {
                    _connection.Open(); // Mở kết nối
                }

                // B1: Tạo câu truy vấn SQL
                // Ở đây mình sẽ lấy hết các dữ liệu trong bảng SUPPLIER_BANKS
                // => Câu sql: SELECT *FROM SUPPLIER_BANKS
                // Ở đây mình không cần sử dụng string.Format vì câu sql này đã đầy đủ thông tin rồi và không cần thay đổi gì cả
                string sqlQuery = "SELECT *FROM SUPPLIER_BANKS";


                // Ở thao tác này mình sẽ cần tạo biến để nhận giá trị từ sql trả về
                // Để lấy dữ liệu từ SQL trả về mình sẽ cần sử dụng kiểu dữ liệu DẠNG 1 là kiểu dữ liệu DataTable
                // Đầu tiền để có thể lấy được dữ liệu và gán cho DataTable thì mình cần tạo một DataSet
                DataSet ds = new DataSet();

                // Tiếp theo mình cần sử dụng Kiểu dữ liệu: SqlDataAdapter để đọc và lấy dữ liệu từ sql  
                // SqlDataAdapter gồm 2 tham số khí khởi tạo: Câu lệnh SQL, ConnectionString
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, _connection);

                // Sau khi dữ liệu đã được lấy lên từ CSDL, mình sẽ thực hiện gán dữ liệu cho DataSet
                // Theo CÚ PHÁP: <SqlDataAdapter>.Fill(<DataSet>, "Table_Name")
                // "Table_Name": Đặt theo ý mình hiểu, ví dụ lấy DS ngân hàng thì đặt là DS_BANK, tùy ý
                da.Fill(ds, "DS_BANKS");

                // Sau khi đã gán dữ liệu cho DataSet thành công mình sẽ thực hiện lấy dữ liệu của DataSet và trả về cho DataTable
                // Mình đã lấy dữ liệu và đặt tên cho nó là DS_BANKS -> Thì khi lấy mình cũng cần truyền đúng cái tên DS_BANKS
                DataTable dt = new DataTable();
                dt = ds.Tables["DS_BANKS"];

                // Khi đã có được DataTable có dữ liệu thì mình sẽ hiển thị lên grid
                // CÚ PHÁP: <Grid>.DataSource = <DataTable>
                // <gird>: Là grid mình đã tạo trên form để hiển thị dữ liệu
                // <DataTable>: Chính là "dt" mình đã có được ở bước trên
                // ==>
                grdBanks.DataSource = dt;



                // Đóng kết nối xuống CSDL
                _connection.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
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
            // Phải nhớ try - catch trước khi thao tác với CSDL
            try
            {
                // Tạo đường kết nối xuống CSDL
                if (_connection.State == ConnectionState.Closed) // Kiểm tra trạng thái
                {
                    _connection.Open(); // Mở kết nối
                }


                // Tương tự các bước như INSERT
                // - B1: Tạo câu UPDATE SQL theo cú pháp: UPDATE <TABLE_NAME> SET <DANH SÁCH THUỘC TÍNH UPDATE> WHERE <ĐIỀU KIỆN UPDATE>
                // - <TABLE_NAME>: SUPPLIER_BANKS
                // - <DANH SÁCH THUỘC TÍNH UPDATE>: BANK_ACCOUNT, BANK_NAME, BANK_BENEFICIAL
                // - <ĐIỀU KIỆN UPDATE>: ID = '...'
                // Sử dụng string.Format để thay thế các giá trị vào câu lệnh SQL
                string sqlQuery = string.Format("UPDATE SUPPLIER_BANKS SET BANK_ACCOUNT = '{0}', BANK_NAME = N'{1}', BANK_BENEFICIAL = N'{2}' WHERE ID = '{3}' ",
                    txtAcc.Text, txtName.Text, txtBeneficial.Text, txtID.Text);

                // B2: Tạo SqlCommand
                SqlCommand cmd = new SqlCommand(sqlQuery, _connection);

                // B3: Thực hiện câu lệnh và nhận kết quả trả về
                // Trả về 1: là thành công
                // Trả về -1: là không thành công
                // Trả về 0: là không có gì được thực hiện
                int rs = cmd.ExecuteNonQuery();

                // ----------- B4: Kiểm tra kết quả và thông báo cho người dùng biết
                if (rs == 1)
                {
                    MessageBox.Show("Thành công");
                }
                else
                {
                    MessageBox.Show("Không thành công");
                }


                // Đóng kết nối xuống CSDL
                _connection.Close();

            }
            catch (SqlException ex)
            {
                // Mình sẽ đưa ra 1 cảnh báo lỗi
                throw new Exception(ex.Message);
            }
        }

        // Hàm xử lý khi nhấn Load
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Mình đã tạo hàm LoadData() ở trên nên ở đây mình sẽ gọi lại hàm LoadData()
            LoadData();
        }


        // Bắt sự kiện khi click vào mỗi ô sẽ hiển thị các dữ liệu tương ứng lên text box 
        private void grdBanks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm trả xem grdBanks có dòng dữ liệu nào không
            // Nếu không có thì return => Có nghĩa là hủy bỏ các thao tác tiếp theo -> Tránh gây lỗi chương trình
            if (grdBanks.Rows.Count <= 0) return; 

            // Nếu có dữ liệu -> Lấy vị trí của con trỏ chuột đã nhấn vào -> Ví dụ nhấn vào dòng 1 thì Index sẽ bằng 0
            // Do mọi thứ trọng lập trình đều bắt đầu từ số 0
            // Mình nhìn trên màn mình là dòng đầu tiền sẽ luôn luôn bắt đầu với Index = 0
            // Mình nhìn trên màn hình là dòng số 7 thì trong code Index sẽ bằng 6
            // Nếu mà muốn tính lẹ thì nhìn trên màn hình sau đó -1 sẽ có được Index
            // Dòng 5 => Index = 5 - 1 = 4
            // Dòng 1991 => Index  = 1991 - 1 = 1990

            // Lấy Index của dòng mình chọn(1)
            // CÚ PHÁP: <grid chứa dữ liệu>.CurrentRow.Index;
            int rsl = grdBanks.CurrentRow.Index;

            // Tiếp theo mình sẽ lấy dữ liệu của ô mình mong muốn theo thứ tự từ trái qua phải, từ Index = 0 -> Index = 1, 2, 3
            // CÚ PHÁP: <grid chứa dữ liệu>.Rows[<Index của dòng mình đã chọn>].Cells[<Ô dữ liệu mình muốn lấy>].Value.ToString()
            // <grid chứa dữ liệu>: grdBanks (ID mình đã đặt khi tạo form)
            // <Index của dòng mình đã chọn>: Mình đã lấy được ở phía trên số (1)
            // <Ô dữ liệu mình muốn lấy>(2): VD: trên màn hình hiển thị dữ liệu theo tứ tự từ trái qua phải là: ID, ACCOUNT, NAME, BENEFICIAL
            // -> Tương ứng Index của các cột đó sẽ là
            // - ID (Index =  0)
            // - ACCOUNT (Index = 1)
            // - NAME (Index = 2)
            // - BENEFICIAL (Index = 3)
            // => Mình lấy BENEFICIAL thì (2) = 3, tương tự lấy ACCOUNT thì (2) = 1
            // => Mình lấy ID => (2) = 0
            // => Theo cú pháp mình sẽ được: grdBanks.Rows[rsl].Cells[0].Value.ToString();
            var idString = grdBanks.Rows[rsl].Cells[0].Value.ToString();

            // => Sau khi mình đã lấy được giá trị của cột ID thì mình sẽ hiển thị nó lên textbox
            // Mình sẽ gán giá trị cho TextBox txtID bằng phương thức .Text
            txtID.Text = idString;
        }


    }
}
