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
            listView1.Columns.Add("Xuất xứ");
            listView1.Columns.Add("Số lượng");

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
            var dsHangHoa = _xnkServices.GetAll<HangHoa>();

            // Duyệt danh sách để thêm từng dòng
            foreach (var hh in dsHangHoa)
            {
                ListViewItem item = new ListViewItem(hh.MaHH);
                item.SubItems.Add(hh.TenHH);
                item.SubItems.Add(hh.DonViTinh);
                item.SubItems.Add(hh.XuatXu);
                item.SubItems.Add(hh.SoLuongTon.ToString());
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                HangHoa newHangHoa = new HangHoa
                {
                    MaHH = textBoxMaHH.Text,
                    TenHH = textBoxTenHH.Text,
                    DonViTinh = comboBoxDonViTinh.Text,
                    XuatXu = textBoxXuatXu.Text,
                    SoLuongTon = (int)numericUpDownSoLuongTon.Value
                };
                if (_xnkServices.Add<HangHoa>(newHangHoa))
                {
                    MessageBox.Show("Thêm hàng hoá thành công!", "Success");
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else
                {
                    MessageBox.Show("Thêm hàng hoá thất bại!", "Error");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm hàng hoá: " + ex.Message, "Error");
            }
        }

        private string GetNextId()
        {
            using (var db = new XNKContextDB())
            {
                // Lấy bản ghi có Id lớn nhất hiện có 
                var last = db.HangHoas
                             .OrderByDescending(p => p.MaHH)
                             .FirstOrDefault();

                if (last == null)
                    return "HH00001";

                // Cắt phần số phía sau
                string numberPart = last.MaHH.Substring(2);
                int number = int.Parse(numberPart) + 1;
                return $"HH{number:D5}";
            }
        }
        private void cleanText()
        {
            textBoxMaHH.Text = GetNextId();
            textBoxTenHH.Text = "";
            comboBoxDonViTinh.Text = "";
            textBoxXuatXu.Text = "";
            numericUpDownSoLuongTon.Value = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                HangHoa updatedHangHoa = new HangHoa
                {
                    MaHH = textBoxMaHH.Text,
                    TenHH = textBoxTenHH.Text,
                    DonViTinh = comboBoxDonViTinh.Text,
                    XuatXu = textBoxXuatXu.Text,
                    SoLuongTon = (int)numericUpDownSoLuongTon.Value
                };

                HangHoa hangHoaToUpdate = _xnkServices.GetById<HangHoa>(updatedHangHoa.MaHH);
                hangHoaToUpdate.TenHH = updatedHangHoa.TenHH;
                hangHoaToUpdate.DonViTinh = updatedHangHoa.DonViTinh;
                hangHoaToUpdate.XuatXu = updatedHangHoa.XuatXu;
                hangHoaToUpdate.SoLuongTon = updatedHangHoa.SoLuongTon;

                if (_xnkServices.Update<HangHoa>(hangHoaToUpdate))
                {
                    MessageBox.Show("Cập nhật hàng hoá thành công!", "Success");
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else
                {
                    MessageBox.Show("Cập nhật hàng hoá thất bại!", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật hàng hoá: " + ex.Message, "Error");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hàng hoá để xoá!");
                return;
            }
            try
            {
                HangHoa hangHoaToDelete = _xnkServices.GetById<HangHoa>(textBoxMaHH.Text);
                if (_xnkServices.Delete<HangHoa>(hangHoaToDelete))
                {
                    MessageBox.Show("Xoá hàng hoá thành công!", "Success");
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else if (_xnkServices.SoftDelete<HangHoa>(hangHoaToDelete))
                {
                    MessageBox.Show("Xoá hàng hoá thành công!", "Success");
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else
                {
                    MessageBox.Show("Xoá hàng hoá thất bại!", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xoá hàng hoá: " + ex.Message, "Error");
            }
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
                textBoxMaHH.Text = listView1.SelectedItems[0].SubItems[0].Text;
                textBoxTenHH.Text = listView1.SelectedItems[0].SubItems[1].Text;
                comboBoxDonViTinh.Text = listView1.SelectedItems[0].SubItems[2].Text;
                textBoxXuatXu.Text = listView1.SelectedItems[0].SubItems[3].Text;
                numericUpDownSoLuongTon.Value = int.Parse(listView1.SelectedItems[0].SubItems[4].Text);
            }
        }
    }
}
