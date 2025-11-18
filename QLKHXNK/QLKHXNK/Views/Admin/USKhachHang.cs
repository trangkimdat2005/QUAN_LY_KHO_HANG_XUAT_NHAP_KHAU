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
    public partial class USKhachHang : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USKhachHang()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USKhachHang_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã khách hàng");
            listView1.Columns.Add("Tên khách hàng");
            listView1.Columns.Add("Số điện thoại");
            listView1.Columns.Add("loại khách hàng");

            cleanText();
            LoadListViewData();
            AdjustListViewColumns(listView1);
        }

        // Điều chỉnh chiều rộng các cột khi kích thước ListView thay đổi
        private void AdjustListViewColumns(ListView listView)
        {
            if (listView.Columns.Count == 0) return;

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
            var dsKhachHang = _xnkServices.GetAll<KhachHang>();

            // Duyệt danh sách để thêm từng dòng
            foreach (var kh in dsKhachHang)
            {
                ListViewItem item = new ListViewItem(kh.MaKH);
                item.SubItems.Add(kh.TenKH);
                item.SubItems.Add(kh.SoDienThoai);
                item.SubItems.Add(kh.LoaiKH);
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                KhachHang newKH = new KhachHang();

                newKH.MaKH = textBoxMaKH.Text;
                newKH.TenKH = textBoxTenKH.Text;
                
                if (textBoxSDT.Text != "")
                {
                    newKH.SoDienThoai = textBoxSDT.Text;
                }
                if(textBoxEmail.Text != "")
                {
                    newKH.Email = textBoxEmail.Text;
                }

                newKH.DiaChi = textBoxDiaChi.Text;
                newKH.MaSoThue = textBoxMaSoThue.Text;
                newKH.LoaiKH = comboBoxLoaiKH.SelectedItem.ToString();
                newKH.GhiChu = textBoxGhiChu.Text;

                _xnkServices.Add<KhachHang>(newKH);
                MessageBox.Show("Thêm Khách Hàng Thành Công");
                cleanText();
                LoadListViewData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo khách hàng mới: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
        }

        private string GetNextId()
        {
            using (var db = new XNKContextDB())
            {
                // Lấy bản ghi có Id lớn nhất hiện có 
                var last = db.KhachHangs
                             .OrderByDescending(p => p.MaKH)
                             .FirstOrDefault();

                if (last == null)
                    return "KH00001";

                // Cắt phần số phía sau
                string numberPart = last.MaKH.Substring(2);
                int number = int.Parse(numberPart) + 1;
                return $"KH{number:D5}";
            }
        }

        private void cleanText()
        {
            textBoxMaKH.Text = GetNextId();
            textBoxTenKH.Text = "";
            textBoxSDT.Text = "";
            textBoxEmail.Text = "";
            textBoxDiaChi.Text = "";
            textBoxGhiChu.Text = "";
            textBoxMaSoThue.Text = "";
            comboBoxLoaiKH.SelectedIndex = 0;

        }

        private void txtChiNhapSo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSoDienThoai_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == null) return;

            // Tạm thời gỡ bỏ sự kiện để tránh vòng lặp vô hạn khi chúng ta thay đổi Text
            txt.TextChanged -= txtSoDienThoai_TextChanged;

            string currentText = txt.Text;
            string newValue = currentText;

            // 1. Chỉ giữ lại số (sử dụng Linq)
            newValue = new string(newValue.Where(char.IsDigit).ToArray());

            // 2. Kiểm tra số 0
            if (newValue.Length > 0 && newValue[0] != '0')
            {
                newValue = ""; // Xóa nếu ký tự đầu tiên không phải 0
            }

            // 3. Giới hạn 10 số
            if (newValue.Length > 10)
            {
                newValue = newValue.Substring(0, 10);
            }

            // 4. Cập nhật lại Text và vị trí con trỏ
            if (txt.Text != newValue)
            {
                txt.Text = newValue;
                // Di chuyển con trỏ về cuối chuỗi
                txt.SelectionStart = txt.Text.Length;
            }

            // Gắn lại sự kiện
            txt.TextChanged += txtSoDienThoai_TextChanged;
        }

        private void txtSoDienThoai_Validating(object sender, CancelEventArgs e)
        {
            string STD = textBoxSDT.Text.Trim();

            try
            {
                if (STD.Length > 0)
                {
                    if (STD.Length != 10)
                    {
                        throw new FormatException();
                    }
                }
                e.Cancel = false; // Cho phép người dùng rời khỏi ô
            }
            catch (FormatException)
            {
                MessageBox.Show("Số điện thoại không đủ độ dài.\n(ví dụ: 0396290084)",
                                "Lỗi định dạng",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                e.Cancel = true; // Giữ người dùng ở lại để sửa
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            string email = textBoxEmail.Text.Trim(); // Lấy email và bỏ khoảng trắng thừa

            try
            {
                if (email.Length > 0)
                {
                    var mailAddress = new System.Net.Mail.MailAddress(email);
                }

                e.Cancel = false; // Cho phép người dùng rời khỏi ô
            }
            catch (FormatException)
            {
                // Bắt lỗi FormatException nếu email sai định dạng
                MessageBox.Show("Email không đúng định dạng.\n(ví dụ: user@example.com)",
                                "Lỗi định dạng",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                e.Cancel = true; // Giữ người dùng ở lại để sửa
            }
        }

        private void txtMaSoThue_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == null) return;

            // Tạm thời gỡ bỏ sự kiện để tránh vòng lặp vô hạn khi chúng ta thay đổi Text
            txt.TextChanged -= txtMaSoThue_TextChanged;

            string currentText = txt.Text;
            string newValue = currentText;

            // 1. Chỉ giữ lại số (sử dụng Linq)
            newValue = new string(newValue.Where(char.IsDigit).ToArray());



            // 3. Giới hạn 12 số
            if (newValue.Length > 12)
            {
                newValue = newValue.Substring(0, 12);
            }

            // 4. Cập nhật lại Text và vị trí con trỏ
            if (txt.Text != newValue)
            {
                txt.Text = newValue;
                // Di chuyển con trỏ về cuối chuỗi
                txt.SelectionStart = txt.Text.Length;
            }

            // Gắn lại sự kiện
            txt.TextChanged += txtMaSoThue_TextChanged;
        }

        private void txtMaSoThue_Validating(object sender, CancelEventArgs e)
        {
            string MST = textBoxMaSoThue.Text.Trim();

            try
            {
                if (MST.Length > 0)
                {
                    if (MST.Length < 10 && MST.Length > 12)
                    {
                        throw new FormatException();
                    }
                }
                e.Cancel = false; // Cho phép người dùng rời khỏi ô
            }
            catch (FormatException)
            {
                MessageBox.Show("Mã số thuế không đúng định dạng. ",
                                "Lỗi định dạng",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                e.Cancel = true; // Giữ người dùng ở lại để sửa
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
                KhachHang khachHang = _xnkServices.GetById<KhachHang>(listView1.SelectedItems[0].SubItems[0].Text);

                textBoxMaKH.Text = khachHang.MaKH;
                textBoxTenKH.Text = khachHang.TenKH;
                textBoxSDT.Text = khachHang.SoDienThoai;
                textBoxEmail.Text = khachHang.Email;
                textBoxDiaChi.Text = khachHang.DiaChi;
                textBoxGhiChu.Text = khachHang.GhiChu;
                textBoxMaSoThue.Text = khachHang.MaSoThue;
                comboBoxLoaiKH.SelectedIndex = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            try
            {
                KhachHang khachHang = _xnkServices.GetById<KhachHang>(textBoxMaKH.Text);

                khachHang.TenKH = textBoxTenKH.Text;
                if (textBoxSDT.Text != "")
                {
                    khachHang.SoDienThoai = textBoxSDT.Text;
                }
                if (textBoxEmail.Text != "")
                {
                    khachHang.Email = textBoxEmail.Text;
                }

                khachHang.DiaChi = textBoxDiaChi.Text;
                khachHang.MaSoThue = textBoxMaSoThue.Text;
                khachHang.LoaiKH = comboBoxLoaiKH.SelectedItem.ToString();
                khachHang.GhiChu = textBoxGhiChu.Text;

                if (_xnkServices.Update<KhachHang>(khachHang))
                {
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Success");
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else
                {
                    MessageBox.Show("Cập nhật khách hàng thất bại!", "Error");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật khách hàng: " + ex.Message, "Error");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xoá!");
                return;
            }
            try
            {
                KhachHang hangHoaToDelete = _xnkServices.GetById<KhachHang>(textBoxMaKH.Text);
                if (_xnkServices.Delete<KhachHang>(hangHoaToDelete))
                {
                    MessageBox.Show("Xoá khách hàng thành công!", "Success");
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else if (_xnkServices.SoftDelete<KhachHang>(hangHoaToDelete))
                {
                    MessageBox.Show("Xoá khách hàng thành công!", "Success");
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else
                {
                    MessageBox.Show("Xoá khách hàng thất bại!", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xoá khách hàng: " + ex.Message, "Error");
            }
        }

        private void comboBoxLoaiKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxLoaiKH.SelectedItem.Equals("Doanh nghiệp"))
            {
                textBoxMaSoThue.Enabled = true;
            }
            else
            {
                textBoxMaSoThue.Enabled = false;
                textBoxMaSoThue.Text = "";
            }
        }
    }
}
