using QLKHXNK.Models.Entities;
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
    public partial class USThongKe : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USThongKe()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USThongKe_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Kho");
            listView1.Columns.Add("Hàng hoá");
            listView1.Columns.Add("Tháng");
            listView1.Columns.Add("Năm");
            listView1.Columns.Add("Số lượng nhập");
            listView1.Columns.Add("Số lượng xuất");
            listView1.Columns.Add("Tồn kho");
            listView1.Columns.Add("Ghi chú");

            LoadListViewData();
            AdjustListViewColumns(listView1);
        }

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
            var dsThongKe = _xnkServices.GetAll<ThongKe>();

            // Duyệt danh sách để thêm từng dòng
            foreach (var tk in dsThongKe)
            {
                ListViewItem item = new ListViewItem(_xnkServices.GetById<Kho>(tk.MaKho).TenKho);
                item.SubItems.Add(_xnkServices.GetById<HangHoa>(tk.MaHH).TenHH);
                item.SubItems.Add(tk.Thang.ToString());
                item.SubItems.Add(tk.Nam.ToString());
                item.SubItems.Add(tk.SoLuongNhap.ToString());
                item.SubItems.Add(tk.SoLuongXuat.ToString());
                item.SubItems.Add(tk.SoLuongTon.ToString());
                item.SubItems.Add(tk.GhiChu);
                listView1.Items.Add(item);
            }
        }
    }
}
