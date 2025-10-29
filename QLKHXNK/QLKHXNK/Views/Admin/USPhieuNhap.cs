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
    public partial class USPhieuNhap : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USPhieuNhap()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USPhieuNhap_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã phiếu nhập");
            listView1.Columns.Add("Nhân viên");
            listView1.Columns.Add("Nhà cung cấp");
            listView1.Columns.Add("Kho");
            listView1.Columns.Add("Ngày Nhập");
            listView1.Columns.Add("Tổng số lượng");
            listView1.Columns.Add("Tổng giá trị");
            listView1.Columns.Add("Ghi chú");

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
            var dsPhieuNhap = _xnkServices.DSPhieuNhap();

            // Duyệt danh sách để thêm từng dòng
            foreach (var pn in dsPhieuNhap)
            {
                ListViewItem item = new ListViewItem(pn.MaPN);
                item.SubItems.Add(pn.NhanVien.TenNV);
                item.SubItems.Add(pn.Kho.TenKho);
                item.SubItems.Add(pn.NhaCungCap.TenNCC);
                item.SubItems.Add(pn.NgayNhap.ToShortDateString());
                item.SubItems.Add(pn.TongSoLuong.ToString());
                item.SubItems.Add(pn.TongGiaTri.ToString());
                item.SubItems.Add(pn.GhiChu);
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
