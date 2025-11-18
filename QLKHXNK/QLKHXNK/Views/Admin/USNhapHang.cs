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
    public partial class USNhapHang : UserControl
    {
        private readonly IXNKServices _xnkServices;

        //private List<HangHoa> hangHoas;

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

            

            LoadListViewData();
            AdjustListViewColumns(listView1);
            AdjustListViewColumns(listView2);
            numericUpDownSoLuongNhap.Value = 1;

            // Load dữ liệu cho comboBoxNhaCungCap
            var nhaCungCaps = _xnkServices.GetAll<NhaCungCap>();
            comboBoxNhaCungCap.DataSource = nhaCungCaps;
            comboBoxNhaCungCap.DisplayMember = "TenNCC";
            comboBoxNhaCungCap.ValueMember = "MaNCC";
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
                            lst.SubItems[2].Text = (int.Parse(lst.SubItems[2].Text) + numericUpDownSoLuongNhap.Value).ToString();
                            AdjustListViewColumns(listView2);
                            numericUpDownSoLuongNhap.Value = 1;
                            return;
                        }
                    }
                }

                ListViewItem item = new ListViewItem(listViewItem.SubItems[0].Text);
                item.SubItems.Add(listViewItem.SubItems[1].Text);
                item.SubItems.Add(numericUpDownSoLuongNhap.Value.ToString());
                listView2.Items.Add(item);


                AdjustListViewColumns(listView2);

                numericUpDownSoLuongNhap.Value = 1;
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
            numericUpDownSoLuongNhap.Value = 1;
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            if(listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để sửa", "Warning");
                return;
            }
            listView2.SelectedItems[0].SubItems[2].Text = numericUpDownSoLuongNhap.Value.ToString();

            AdjustListViewColumns(listView2);
            numericUpDownSoLuongNhap.Value = 1;
        }

        private void selectHangNhap(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                numericUpDownSoLuongNhap.Value = 1;
                return;
            }
            numericUpDownSoLuongNhap.Value = int.Parse(listView2.SelectedItems[0].SubItems[2].Text);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDownSoLuongNhap.Value = 1;
        }

        private void listView2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                numericUpDownSoLuongNhap.Value = 1;
                return;
            }
            numericUpDownSoLuongNhap.Value = int.Parse(listView2.SelectedItems[0].SubItems[2].Text);
        }

        private void textBoxGiaNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView2.Items.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm sản phẩm để nhập hàng", "Warning");
                    return;
                }
                // Xử lý logic nhập hàng ở đây


                PhieuNhap phieuNhap = new PhieuNhap
                {
                    MaPN = GetNextId(),
                    NgayNhap = DateTime.Now,
                    MaNCC = comboBoxNhaCungCap.SelectedValue.ToString(),
                    MaKho = comboBoxKho.SelectedValue.ToString()
                };

                if (_xnkServices.Add<PhieuNhap>(phieuNhap))
                {
                    foreach (ListViewItem item in listView2.Items)
                    {
                        ChiTietPhieuNhap chiTietPhieuNhap = new ChiTietPhieuNhap
                        {
                            MaPN = phieuNhap.MaPN,
                            MaHH = item.SubItems[0].Text,
                            SoLuong = int.Parse(item.SubItems[2].Text)
                        };
                        if (!_xnkServices.Add<ChiTietPhieuNhap>(chiTietPhieuNhap))
                        {
                            MessageBox.Show("Lỗi khi thêm chi tiết phiếu nhập cho hàng " + item.SubItems[1].Text, "Error");
                            return;
                        }
                    }
                }

                listView2.Items.Clear();

                MessageBox.Show("Nhập hàng thành công!", "Success");
                LoadListViewData();
                AdjustListViewColumns(listView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nhập hàng: " + ex.Message, "Error");
            }
        }


        private string GetNextId()
        {
            using (var db = new XNKContextDB())
            {
                // Lấy bản ghi có Id lớn nhất hiện có 
                var last = db.PhieuNhaps
                             .OrderByDescending(p => p.MaPN)
                             .FirstOrDefault();

                if (last == null)
                    return "PN00001";

                // Cắt phần số phía sau
                string numberPart = last.MaPN.Substring(2);
                int number = int.Parse(numberPart) + 1;
                return $"PN{number:D5}";
            }
        }
    }
}
