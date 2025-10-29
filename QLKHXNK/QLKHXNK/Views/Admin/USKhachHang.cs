using QLKHXNK.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKHXNK.Views.Admin
{
    public partial class USKhachHang : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USKhachHang()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USKhachHang_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã khách hàng");
            listView1.Columns.Add("Tên khách hàng");
            listView1.Columns.Add("Số điện thoại");
            listView1.Columns.Add("loại khách hàng");

            LoadListViewData();
            AdjustListViewColumns(listView1);
        }

        // Điều chỉnh chiều rộng các cột khi kích thước ListView thay đổi
        private void AdjustListViewColumns(ListView listView)
        {
            if (listView.Columns.Count == 0) return;

            listView.BeginUpdate();
            foreach (ColumnHeader col in listView.Columns)
            {
                col.Width = -2; // auto fit theo nội dung
            }
            listView.EndUpdate();
        }

        // Tải dữ liệu vào ListView từ dịch vụ
        private void LoadListViewData()
        {
            // Xóa dữ liệu cũ
            listView1.Items.Clear();

            // Ví dụ danh sách hàng hóa
            var dsKhachHang = _xnkServices.DSKhachHang();

            // Duyệt danh sách để thêm từng dòng
            foreach (var kh in dsKhachHang)
            {
                ListViewItem item = new ListViewItem(kh.MaKH);
                item.SubItems.Add(kh.TenKH);
                item.SubItems.Add(kh.SoDienThoai);
                item.SubItems.Add(kh.LoaiKH);
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
