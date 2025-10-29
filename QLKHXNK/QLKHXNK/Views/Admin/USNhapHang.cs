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
    public partial class USNhapHang : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USNhapHang()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USNhapHang_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã hàng");
            listView1.Columns.Add("Tên hàng");
            listView1.Columns.Add("Số lượng tồn");

            listView2.Columns.Add("Mã hàng");
            listView2.Columns.Add("Tên hàng");
            listView2.Columns.Add("Số lượng nhập");
            listView2.Columns.Add("Đơn giá nhập");
            listView2.Columns.Add("Thành tiền");

            LoadListViewData();
            AdjustListViewColumns(listView1);
            AdjustListViewColumns(listView2);
        }

        private void AdjustListViewColumns(ListView listView)
        {
            if (listView.Columns.Count == 0) return;

            // 1️⃣ Auto resize theo nội dung
            foreach (ColumnHeader col in listView.Columns)
            {
                col.Width = -2; // tự động fit theo nội dung
            }

            // 2️⃣ Tính tổng chiều rộng sau khi fit
            int totalWidth = listView.Columns.Cast<ColumnHeader>().Sum(c => c.Width);
            int availableWidth = listView.ClientSize.Width;

            // 3️⃣ Nếu tổng nhỏ hơn chiều rộng ListView → chia phần dư đều cho các cột
            if (totalWidth < availableWidth)
            {
                int extra = availableWidth - totalWidth;
                int addEach = extra / listView.Columns.Count;

                foreach (ColumnHeader col in listView.Columns)
                {
                    col.Width += addEach;
                }

                // Nếu chia không hết do làm tròn, cộng phần dư còn lại vào cột cuối
                int remainder = extra % listView.Columns.Count;
                listView.Columns[listView.Columns.Count - 1].Width += remainder;
            }
        }

        // Tải dữ liệu vào ListView từ dịch vụ
        private void LoadListViewData()
        {
            // Xóa dữ liệu cũ
            listView1.Items.Clear();

            // Ví dụ danh sách hàng hóa
            var DSHangHoa = _xnkServices.DSHangHoa();

            // Duyệt danh sách để thêm từng dòng
            foreach (var hh in DSHangHoa)
            {
                ListViewItem item = new ListViewItem(hh.MaHH);
                item.SubItems.Add(hh.TenHH);
                item.SubItems.Add(hh.SoLuongTon.ToString());
                listView1.Items.Add(item);
            }
        }
    }
}
