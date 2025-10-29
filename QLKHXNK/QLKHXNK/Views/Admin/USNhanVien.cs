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
            listView1.Columns.Add("Mã nhân viên");
            listView1.Columns.Add("Tên nhân viên");
            listView1.Columns.Add("Giới tính");
            listView1.Columns.Add("Số điện thoại");
            listView1.Columns.Add("Email");
            listView1.Columns.Add("Chức vụ");
            listView1.Columns.Add("Lương");
            listView1.Columns.Add("Trạng thái");

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
