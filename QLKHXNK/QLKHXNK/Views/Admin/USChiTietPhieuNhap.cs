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
    public partial class USChiTietPhieuNhap : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USChiTietPhieuNhap()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USChiTietPhieuNhap_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã phiếu nhập");
            listView1.Columns.Add("Tên hàng hoá");
            listView1.Columns.Add("Số lượng");

            var PhieuNhaps = _xnkServices.GetAll<PhieuNhap>();
            comboBoxMaPhieuNhap.DataSource = PhieuNhaps;
            comboBoxMaPhieuNhap.DisplayMember = "MaPN";
            comboBoxMaPhieuNhap.ValueMember = "MaPN";

            var HangHoas = _xnkServices.GetAll<HangHoa>();
            comboBoxHangHoa.DataSource = HangHoas;
            comboBoxHangHoa.DisplayMember = "TenHH";
            comboBoxHangHoa.ValueMember = "MaHH";

            cleanText();

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
            var DSChiTietPhieuNhap = _xnkServices.GetAll<ChiTietPhieuNhap>();

            // Duyệt danh sách để thêm từng dòng
            foreach (var ctpn in DSChiTietPhieuNhap)
            {
                ListViewItem item = new ListViewItem(ctpn.MaPN);
                item.SubItems.Add(_xnkServices.GetById<HangHoa>(ctpn.MaHH).TenHH);
                item.SubItems.Add(ctpn.SoLuong.ToString());
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ChiTietPhieuNhap ctpn = new ChiTietPhieuNhap
                {
                    MaPN = comboBoxMaPhieuNhap.SelectedValue.ToString(),
                    MaHH = comboBoxHangHoa.SelectedValue.ToString(),
                    SoLuong = int.Parse(numericUpDownSoLuong.Text)
                };
                if (_xnkServices.Add<ChiTietPhieuNhap>(ctpn))
                {
                    MessageBox.Show("Thêm chi tiết phiếu nhập thành công!");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Thêm chi tiết phiếu nhập thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn chi tiết phiếu nhập để cập nhật!");
                return;
            }
            try
            {
                ChiTietPhieuNhap ctpn = new ChiTietPhieuNhap
                {
                    MaPN = comboBoxMaPhieuNhap.SelectedValue.ToString(),
                    MaHH = comboBoxHangHoa.SelectedValue.ToString(),
                    SoLuong = int.Parse(numericUpDownSoLuong.Text)
                };
                ChiTietPhieuNhap chiTietPhieuNhapToUpdate = _xnkServices.GetChiTietPhieuNhapByIds(ctpn.MaPN, ctpn.MaHH);
                chiTietPhieuNhapToUpdate.SoLuong = ctpn.SoLuong;
                if (_xnkServices.Update<ChiTietPhieuNhap>(chiTietPhieuNhapToUpdate))
                {
                    MessageBox.Show("Cập nhật chi tiết phiếu nhập thành công!");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Cập nhật chi tiết phiếu nhập thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cleanText()
        {
            comboBoxMaPhieuNhap.Text = "";
            comboBoxHangHoa.Text = "";
            numericUpDownSoLuong.Value = 1;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                cleanText();
                return;
            }
            else
            {
                comboBoxMaPhieuNhap.Text = listView1.SelectedItems[0].SubItems[0].Text;
                comboBoxHangHoa.Text = listView1.SelectedItems[0].SubItems[1].Text;
                numericUpDownSoLuong.Value = int.Parse(listView1.SelectedItems[0].SubItems[2].Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn chi tiết phiếu nhập để xoá!");
                return;
            }
            try
            {
                ChiTietPhieuNhap chiTietPhieuNhap = _xnkServices.GetChiTietPhieuNhapByIds(listView1.SelectedItems[0].SubItems[0].Text, listView1.SelectedItems[0].SubItems[1].Text);
                if (_xnkServices.Delete<ChiTietPhieuNhap>(chiTietPhieuNhap))
                {
                    MessageBox.Show("Xoá chi tiết phiếu nhập thành công!");
                    cleanText();
                    LoadListViewData();
                }
                else if (_xnkServices.SoftDelete<ChiTietPhieuNhap>(chiTietPhieuNhap))
                {
                    MessageBox.Show("Xoá chi tiết phiếu nhập thành công (Soft Delete)!");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Xoá chi tiết phiếu nhập thất bại!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
