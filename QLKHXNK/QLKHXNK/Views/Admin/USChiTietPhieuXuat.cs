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
    public partial class USChiTietPhieuXuat : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USChiTietPhieuXuat()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USChiTietPhieuXuat_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã phiếu xuất");
            listView1.Columns.Add("Tên hàng hoá");
            listView1.Columns.Add("Số lượng");
            listView1.Columns.Add("Đơn giá xuất");
            listView1.Columns.Add("Thành tiền");

            LoadListViewData();
            AdjustListViewColumns(listView1);
        }

        // Điều chỉnh chiều rộng các cột khi kích thước ListView thay đổi
        private void AdjustListViewColumns(ListView listView)
        {
            if (listView.Columns.Count == 0) return;

            // 1️⃣ Fit nội dung từng cột trước
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
            var dsChiTietPhieuXuat = _xnkServices.DSChiTietPhieuXuat();

            // Duyệt danh sách để thêm từng dòng
            foreach (var ctpx in dsChiTietPhieuXuat)
            {
                ListViewItem item = new ListViewItem(ctpx.MaPX);
                item.SubItems.Add(ctpx.HangHoa.TenHH);
                item.SubItems.Add(ctpx.SoLuong.ToString());
                item.SubItems.Add(ctpx.DonGiaXuat.ToString());
                item.SubItems.Add(ctpx.ThanhTien.ToString());
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
