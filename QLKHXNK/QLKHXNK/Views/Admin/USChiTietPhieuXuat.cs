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

            var PhieuXuats = _xnkServices.GetAll<PhieuXuat>();
            comboBoxMaPX.DataSource = PhieuXuats;
            comboBoxMaPX.DisplayMember = "MaPX";
            comboBoxMaPX.ValueMember = "MaPX";

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
            var dsChiTietPhieuXuat = _xnkServices.GetAll<ChiTietPhieuXuat>();

            // Duyệt danh sách để thêm từng dòng
            foreach (var ctpx in dsChiTietPhieuXuat)
            {
                ListViewItem item = new ListViewItem(ctpx.MaPX);
                item.SubItems.Add(_xnkServices.GetById<HangHoa>(ctpx.MaHH).TenHH);
                item.SubItems.Add(ctpx.SoLuong.ToString());
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ChiTietPhieuXuat ctpx = new ChiTietPhieuXuat
                {
                    MaPX = comboBoxMaPX.SelectedValue.ToString(),
                    MaHH = comboBoxHangHoa.SelectedValue.ToString(),
                    SoLuong = int.Parse(numericUpDownSoLuong.Text)
                };
                if (_xnkServices.Add<ChiTietPhieuXuat>(ctpx))
                {
                    MessageBox.Show("Thêm chi tiết phiếu xuất thành công!");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Thêm chi tiết phiếu xuất thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cleanText()
        {
            comboBoxMaPX.Text = "";
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
                comboBoxMaPX.Text = listView1.SelectedItems[0].SubItems[0].Text;
                comboBoxHangHoa.Text = listView1.SelectedItems[0].SubItems[1].Text;
                numericUpDownSoLuong.Value = int.Parse(listView1.SelectedItems[0].SubItems[2].Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    return;
                }
                ChiTietPhieuXuat ctpx = new ChiTietPhieuXuat
                {
                    MaPX = comboBoxMaPX.SelectedValue.ToString(),
                    MaHH = comboBoxHangHoa.SelectedValue.ToString(),
                    SoLuong = int.Parse(numericUpDownSoLuong.Text)
                };
                ChiTietPhieuXuat chiTietPhieuXuatToUpdate = _xnkServices.GetChiTietPhieuXuatByIds(ctpx.MaPX, ctpx.MaHH);
                chiTietPhieuXuatToUpdate.SoLuong = ctpx.SoLuong;
                if (_xnkServices.Update<ChiTietPhieuXuat>(chiTietPhieuXuatToUpdate))
                {
                    MessageBox.Show("Cập nhật chi tiết phiếu xuất thành công!");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Cập nhật chi tiết phiếu xuất thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn chi tiết phiếu xuất để xoá!");
                return;
            }
            try
            {
                ChiTietPhieuXuat ctpx = new ChiTietPhieuXuat
                {
                    MaPX = listView1.SelectedItems[0].SubItems[0].Text,
                    MaHH = _xnkServices.GetAll<HangHoa>().FirstOrDefault(hh => hh.TenHH == listView1.SelectedItems[0].SubItems[1].Text).MaHH,
                    SoLuong = int.Parse(listView1.SelectedItems[0].SubItems[2].Text)
                };
                if (_xnkServices.Delete<ChiTietPhieuXuat>(_xnkServices.GetChiTietPhieuXuatByIds(ctpx.MaPX, ctpx.MaHH)))
                {
                    MessageBox.Show("Xoá chi tiết phiếu xuất thành công!");
                    cleanText();
                    LoadListViewData();
                }
                else if (_xnkServices.SoftDelete<ChiTietPhieuXuat>(_xnkServices.GetChiTietPhieuXuatByIds(ctpx.MaPX, ctpx.MaHH)))
                {
                    MessageBox.Show("Xoá chi tiết phiếu xuất thành công (Soft Delete)!");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Xoá chi tiết phiếu xuất thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
