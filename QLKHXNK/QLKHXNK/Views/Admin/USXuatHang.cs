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
    public partial class USXuatHang : UserControl
    {
        private readonly IXNKServices _xnkServices;

        //private List<HangHoa> hangHoas;

        public USXuatHang()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USXuatHang_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã hàng");
            listView1.Columns.Add("Tên hàng");
            listView1.Columns.Add("Số lượng tồn");

            listView2.Columns.Add("Mã hàng");
            listView2.Columns.Add("Tên hàng");
            listView2.Columns.Add("Số lượng xuất");



            LoadListViewData();
            AdjustListViewColumns(listView1);
            AdjustListViewColumns(listView2);
            numericUpDownSoLuongXuat.Value = 1;

            // Load dữ liệu cho comboBoxNhaCungCap
            var khachHangs = _xnkServices.GetAll<KhachHang>();
            comboBoxKhachHang.DataSource = khachHangs;
            comboBoxKhachHang.DisplayMember = "TenKH";
            comboBoxKhachHang.ValueMember = "MaKH";
            // Load dữ liệu cho comboBoxKho
            var khos = _xnkServices.GetAll<Kho>();
            comboBoxKho.DataSource = khos;
            comboBoxKho.DisplayMember = "TenKho";
            comboBoxKho.ValueMember = "MaKho";

        }

        private void AdjustListViewColumns(ListView listView)
        {
            if (listView.Columns.Count == 0) return;


            foreach (ColumnHeader col in listView.Columns)
            {
                col.Width = -2; // tự động fit theo nội dung
            }

        }

        // Tải dữ liệu vào ListView từ dịch vụ
        private void LoadListViewData()
        {
            try
            {
                // Xóa dữ liệu cũ
                listView1.Items.Clear();

                // Ví dụ danh sách hàng hóa
                var DSHangHoa = _xnkServices.GetAll<HangHoa>();

                // Duyệt danh sách để thêm từng dòng
                foreach (var hh in DSHangHoa)
                {
                    ListViewItem item = new ListViewItem(hh.MaHH);
                    item.SubItems.Add(hh.TenHH);
                    item.SubItems.Add(hh.SoLuongTon.ToString());
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu hàng hóa: " + ex.Message, "Error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewItem listViewItem = listView1.SelectedItems[0];
                if (listViewItem == null) return;

                if (listView2 != null)
                {
                    foreach (ListViewItem lst in listView2.Items)
                    {
                        if (listViewItem.SubItems[0].Text == lst.SubItems[0].Text)
                        {
                            lst.SubItems[2].Text = (int.Parse(lst.SubItems[2].Text) + numericUpDownSoLuongXuat.Value).ToString();
                            AdjustListViewColumns(listView2);
                            numericUpDownSoLuongXuat.Value = 1;
                            return;
                        }
                    }
                }

                ListViewItem item = new ListViewItem(listViewItem.SubItems[0].Text);
                item.SubItems.Add(listViewItem.SubItems[1].Text);
                item.SubItems.Add(numericUpDownSoLuongXuat.Value.ToString());
                listView2.Items.Add(item);


                AdjustListViewColumns(listView2);

                numericUpDownSoLuongXuat.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi, Không thể thêm sản phẩm, " + ex.Message, "Error");
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xoá", "Warning");
                return;
            }
            listView2.Items.Remove(listView2.SelectedItems[0]);
            AdjustListViewColumns(listView2);
            numericUpDownSoLuongXuat.Value = 1;
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để sửa", "Warning");
                return;
            }
            listView2.SelectedItems[0].SubItems[2].Text = numericUpDownSoLuongXuat.Value.ToString();

            AdjustListViewColumns(listView2);
            numericUpDownSoLuongXuat.Value = 1;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                numericUpDownSoLuongXuat.Value = 1;
                return;
            }
            numericUpDownSoLuongXuat.Value = int.Parse(listView2.SelectedItems[0].SubItems[2].Text);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDownSoLuongXuat.Value = 1;
        }

        private void listView2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                numericUpDownSoLuongXuat.Value = 1;
                return;
            }
            numericUpDownSoLuongXuat.Value = int.Parse(listView2.SelectedItems[0].SubItems[2].Text);
        }

        private void btnXuatHang_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView2.Items.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm sản phẩm để xuất hàng", "Warning");
                    return;
                }
                // Xử lý logic nhập hàng ở đây

                PhieuXuat phieuXuat = new PhieuXuat
                {
                    MaPX = GetNextId(),
                    NgayXuat = DateTime.Now,
                    MaKH = comboBoxKhachHang.SelectedValue.ToString(),
                    MaKho = comboBoxKho.SelectedValue.ToString()
                };

                if (_xnkServices.Add<PhieuXuat>(phieuXuat))
                {
                    foreach (ListViewItem item in listView2.Items)
                    {
                        ChiTietPhieuXuat chiTietPhieuXuat = new ChiTietPhieuXuat
                        {
                            MaPX = phieuXuat.MaPX,
                            MaHH = item.SubItems[0].Text,
                            SoLuong = int.Parse(item.SubItems[2].Text)
                        };
                        if (!_xnkServices.Add<ChiTietPhieuXuat>(chiTietPhieuXuat))
                        {
                            MessageBox.Show("Tạo chi tiết phiếu xuất thất bại!", "Error");
                            return;
                        }
                    }
                    MessageBox.Show("Xuất hàng thành công!", "Success");
                    listView2.Items.Clear();
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                }
                else
                {
                    MessageBox.Show("Tạo phiếu xuất thất bại!", "Error");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất hàng: " + ex.Message, "Error");
            }
        }


        private string GetNextId()
        {
            using (var db = new XNKContextDB())
            {
                // Lấy bản ghi có Id lớn nhất hiện có 
                var last = db.PhieuXuats
                             .OrderByDescending(p => p.MaPX)
                             .FirstOrDefault();

                if (last == null)
                    return "PX00001";

                // Cắt phần số phía sau
                string numberPart = last.MaPX.Substring(2);
                int number = int.Parse(numberPart) + 1;
                return $"PX{number:D5}";
            }
        }

        
    }
}
