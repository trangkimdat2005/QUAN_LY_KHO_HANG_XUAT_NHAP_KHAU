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
    public partial class USKho : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USKho()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USKho_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã Kho");
            listView1.Columns.Add("Tên Kho");
            listView1.Columns.Add("Diện tích");
            listView1.Columns.Add("Loại kho");
            listView1.Columns.Add("Sức chứa");
            listView1.Columns.Add("Trạng thái");
            listView1.Columns.Add("Địa chỉ");
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
            var dsKho = _xnkServices.DSKho();

            // Duyệt danh sách để thêm từng dòng
            foreach (var k in dsKho)
            {
                ListViewItem item = new ListViewItem(k.MaKho);
                item.SubItems.Add(k.TenKho);
                item.SubItems.Add(k.DienTich.ToString());
                item.SubItems.Add(k.LoaiKho);
                item.SubItems.Add(k.SucChua.ToString());
                item.SubItems.Add(k.TrangThai);
                item.SubItems.Add(k.DiaChi);
                item.SubItems.Add(k.GhiChu);
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
