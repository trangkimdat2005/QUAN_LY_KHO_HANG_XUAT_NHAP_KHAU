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
    public partial class USPhieuXuat : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USPhieuXuat()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USPhieuXuat_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã phiếu xuất");
            listView1.Columns.Add("Nhân viên");
            listView1.Columns.Add("Khách hàng");
            listView1.Columns.Add("Kho");
            listView1.Columns.Add("Ngày xuất");
            listView1.Columns.Add("Tổng số lượng");
            listView1.Columns.Add("Tổng giá trị");
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
            var dsPhieuXuat = _xnkServices.DSPhieuXuat();

            // Duyệt danh sách để thêm từng dòng
            foreach (var px in dsPhieuXuat)
            {
                ListViewItem item = new ListViewItem(px.MaPX);
                item.SubItems.Add(px.NhanVien.TenNV);
                item.SubItems.Add(px.Kho.TenKho);
                item.SubItems.Add(px.KhachHang.TenKH);
                item.SubItems.Add(px.NgayXuat.ToShortDateString());
                item.SubItems.Add(px.TongSoLuong.ToString());
                item.SubItems.Add(px.TongGiaTri.ToString());
                item.SubItems.Add(px.GhiChu);
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
