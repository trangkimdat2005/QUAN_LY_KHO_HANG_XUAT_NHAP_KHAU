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
    public partial class USNhaCungCap : UserControl
    {
        private readonly IXNKServices _xnkServices;

        public USNhaCungCap()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USNhaCungCap_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã nhà cung cấp");
            listView1.Columns.Add("Tên nhà cung cấp");
            listView1.Columns.Add("Số điện thoại");
            listView1.Columns.Add("Email");
            listView1.Columns.Add("Địa chỉ");
            listView1.Columns.Add("Quốc gia");
            listView1.Columns.Add("Ghi chú");

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
            var dsNhaChungCap = _xnkServices.DSNhaCungCap();

            // Duyệt danh sách để thêm từng dòng
            foreach (var ncc in dsNhaChungCap)
            {
                ListViewItem item = new ListViewItem(ncc.MaNCC);
                item.SubItems.Add(ncc.TenNCC);
                item.SubItems.Add(ncc.SoDienThoai);
                item.SubItems.Add(ncc.Email);
                item.SubItems.Add(ncc.DiaChi);
                item.SubItems.Add(ncc.QuocGia);
                item.SubItems.Add(ncc.GhiChu);
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
