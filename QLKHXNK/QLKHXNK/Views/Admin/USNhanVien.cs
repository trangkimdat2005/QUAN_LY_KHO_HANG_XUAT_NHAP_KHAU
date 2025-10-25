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
    public partial class USNhanVien : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USNhanVien()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USNhanVien_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã NV");
            listView1.Columns.Add("Tên NV");
            listView1.Columns.Add("Giới tính");
            listView1.Columns.Add("Số điện thoại");
            listView1.Columns.Add("Email");
            listView1.Columns.Add("Chức vụ");
            listView1.Columns.Add("Lương");
            listView1.Columns.Add("Trạng thái");

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
            var dsNhanVien = _xnkServices.DSNhanVien();

            // Duyệt danh sách để thêm từng dòng
            foreach (var nv in dsNhanVien)
            {
                ListViewItem item = new ListViewItem(nv.MaNV);
                item.SubItems.Add(nv.TenNV);
                item.SubItems.Add(nv.GioiTinh);
                item.SubItems.Add(nv.SoDienThoai);
                item.SubItems.Add(nv.Email);
                item.SubItems.Add(nv.ChucVu);
                item.SubItems.Add(nv.LuongCoBan.ToString());
                item.SubItems.Add(nv.TrangThai);
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
