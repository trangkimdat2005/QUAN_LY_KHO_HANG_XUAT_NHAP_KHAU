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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            listView1.Columns.Add("Mã KH");
            listView1.Columns.Add("Tên KH");
            listView1.Columns.Add("Số điện thoại");
            listView1.Columns.Add("loại KH");

            LoadListViewData();
            AdjustListViewColumns();
        }

        // Điều chỉnh chiều rộng các cột khi kích thước ListView thay đổi
        private void AdjustListViewColumns()
        {
            int totalWidth = listView1.ClientSize.Width; // chiều rộng hiển thị thực tế (trừ thanh cuộn)
            int columnCount = listView1.Columns.Count;

            if (columnCount == 0) return;

            int columnWidth = totalWidth / columnCount;

            foreach (ColumnHeader col in listView1.Columns)
            {
                col.Width = columnWidth;
            }
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
    }
}
