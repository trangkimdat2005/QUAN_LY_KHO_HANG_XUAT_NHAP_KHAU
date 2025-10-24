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
    public partial class USHangHoa : UserControl
    {

        private readonly IXNKServices _xnkServices;

        public USHangHoa()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USHangHoa_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã hàng");
            listView1.Columns.Add("Tên hàng");
            listView1.Columns.Add("Đơn vị");
            listView1.Columns.Add("Giá bán");
            listView1.Columns.Add("Số lượng");
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
            var dsHangHoa = _xnkServices.DSHangHoa();

            // Duyệt danh sách để thêm từng dòng
            foreach (var hh in dsHangHoa)
            {
                ListViewItem item = new ListViewItem(hh.MaHH);
                item.SubItems.Add(hh.TenHH);
                item.SubItems.Add(hh.DonViTinh);
                item.SubItems.Add(hh.DonGiaBan.ToString());
                item.SubItems.Add(hh.SoLuongTon.ToString());
                item.SubItems.Add(hh.TrangThai);
                listView1.Items.Add(item);
            }
        }
    }
}
