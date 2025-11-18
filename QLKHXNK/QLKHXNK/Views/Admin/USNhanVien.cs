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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace QLKHXNK.Views.Admin
{
    public partial class USNhanVien : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USNhanVien()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USNhanVien_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã nhân viên");
            listView1.Columns.Add("Tên nhân viên");
            listView1.Columns.Add("Giới tính");
            listView1.Columns.Add("Số điện thoại");
            listView1.Columns.Add("Email");
            listView1.Columns.Add("Chức vụ");
            listView1.Columns.Add("Lương");
            listView1.Columns.Add("Trạng thái");

            // Load dữ liệu cho comboBoxKho
            var khos = _xnkServices.GetAll<Kho>();
            comboBoxKho.DataSource = khos;
            comboBoxKho.DisplayMember = "TenKho";
            comboBoxKho.ValueMember = "MaKho";

            cleanText();
            LoadListViewData();
            AdjustListViewColumns(listView1);
            dateTimePickerNgayVaoLam.MaxDate = DateTime.Today;
            CapNhatQuyTacNgaySinh();
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
            var dsNhanVien = _xnkServices.GetAll<NhanVien>();

            // Duyệt danh sách để thêm từng dòng
            foreach (var nv in dsNhanVien)
            {
                ListViewItem item = new ListViewItem(nv.MaNV);
                item.SubItems.Add(nv.TenNV);
                item.SubItems.Add(nv.GioiTinh);
                item.SubItems.Add(nv.SoDienThoai);
                item.SubItems.Add(nv.Email);
                item.SubItems.Add(nv.ChucVu);
                item.SubItems.Add(nv.LuongCoBan.ToString());
                item.SubItems.Add(nv.TrangThai);
                listView1.Items.Add(item);
            }
        }

        private string GetNextId()
        {
            using (var db = new XNKContextDB())
            {
                // Lấy bản ghi có Id lớn nhất hiện có 
                var last = db.NhanViens
                             .OrderByDescending(p => p.MaNV)
                             .FirstOrDefault();

                if (last == null)
                    return "NV00001";

                // Cắt phần số phía sau
                string numberPart = last.MaNV.Substring(2);
                int number = int.Parse(numberPart) + 1;
                return $"NV{number:D5}";
            }
        }

        private void cleanText()
        {
            textBoxMaNhanVien.Text = GetNextId();
            textBoxTenNhanVien.Text = "";
            textBoxSoDienThoai.Text = "";
            textBoxEmail.Text = "";
            comboBoxGioiTinh.Text = "";
            comboBoxChucVu.Text = "";
            textBoxLuong.Text = "";
            dateTimePickerNgayVaoLam.Value = DateTime.Today;
            CapNhatQuyTacNgaySinh();
            textBoxLuong.Text = "";
            comboBoxTrangThai.SelectedIndex = 0;
            comboBoxKho.Text = "";
            textBoxGhiChu.Text = "";
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
            string STD = textBoxSoDienThoai.Text.Trim();

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

        private void textBoxLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu người dùng nhập ký tự không phải là số hoặc dấu chấm
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '.')
            {
                e.Handled = true;  // Chặn ký tự không hợp lệ
            }
        }

        private void textBoxLuong_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == null) return;

            // Tạm thời gỡ bỏ sự kiện để tránh vòng lặp vô hạn khi chúng ta thay đổi Text
            txt.TextChanged -= textBoxLuong_TextChanged;

            string currentText = txt.Text;
            string newValue = currentText;




            // 4. Cập nhật lại Text và vị trí con trỏ
            if (txt.Text != newValue)
            {
                txt.Text = newValue;
                // Di chuyển con trỏ về cuối chuỗi
                txt.SelectionStart = txt.Text.Length;
            }

            // Gắn lại sự kiện
            txt.TextChanged += textBoxLuong_TextChanged;
        }

        private void textBoxLuong_Validating(object sender, CancelEventArgs e)
        {
            string DT = textBoxLuong.Text.Trim();

            try
            {
                if (DT.Length > 0)
                {
                    if (decimal.Parse(DT) > 100000000000)
                    {
                        throw new FormatException();
                    }
                }
                e.Cancel = false; // Cho phép người dùng rời khỏi ô
            }
            catch (FormatException)
            {
                MessageBox.Show("Lương quá lớn. ",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                e.Cancel = true; // Giữ người dùng ở lại để sửa
            }
        }

        private void dateTimePickerNgayVaoLam_ValueChanged(object sender, EventArgs e)
        {
            DateTime maxDate = dateTimePickerNgayVaoLam.Value.AddYears(-18);
            dateTimePickerNgaySinh.MaxDate = maxDate;
        }

        private void CapNhatQuyTacNgaySinh()
        {
            DateTime ngayVaoLam = dateTimePickerNgayVaoLam.Value;
            DateTime maxNgaySinh = ngayVaoLam.AddYears(-18);
            dateTimePickerNgaySinh.MaxDate = maxNgaySinh;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                NhanVien nhanVien = new NhanVien
                {
                    MaNV = textBoxMaNhanVien.Text,
                    TenNV = textBoxTenNhanVien.Text,
                    GioiTinh = comboBoxGioiTinh.Text,
                    NgaySinh = dateTimePickerNgaySinh.Value,
                    ChucVu = comboBoxChucVu.Text,
                    SoDienThoai = textBoxSoDienThoai.Text,
                    Email = textBoxEmail.Text,
                    NgayVaoLam = dateTimePickerNgayVaoLam.Value,
                    LuongCoBan = (decimal)(string.IsNullOrWhiteSpace(textBoxLuong.Text) ? (decimal?)null : decimal.Parse(textBoxLuong.Text)),
                    TrangThai = comboBoxTrangThai.Text,
                    GhiChu = textBoxGhiChu.Text,
                    MaKho = comboBoxKho.SelectedValue?.ToString()
                };
                if (_xnkServices.Add<NhanVien>(nhanVien))
                {
                    MessageBox.Show("Thêm nhân viên thành công!", "Success");
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại!", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message, "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                _xnkServices.GetById<NhanVien>(listView1.SelectedItems[0].SubItems[0].Text);
                NhanVien updatedNhanVien = new NhanVien
                {
                    MaNV = textBoxMaNhanVien.Text,
                    TenNV = textBoxTenNhanVien.Text,
                    GioiTinh = comboBoxGioiTinh.Text,
                    NgaySinh = dateTimePickerNgaySinh.Value,
                    ChucVu = comboBoxChucVu.Text,
                    SoDienThoai = textBoxSoDienThoai.Text,
                    Email = textBoxEmail.Text,
                    NgayVaoLam = dateTimePickerNgayVaoLam.Value,
                    LuongCoBan = (decimal)(string.IsNullOrWhiteSpace(textBoxLuong.Text) ? (decimal?)null : decimal.Parse(textBoxLuong.Text)),
                    TrangThai = comboBoxTrangThai.Text,
                    GhiChu = textBoxGhiChu.Text,
                    MaKho = comboBoxKho.SelectedValue?.ToString()
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật nhân viên: " + ex.Message, "Error");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xoá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                _xnkServices.GetById<NhanVien>(listView1.SelectedItems[0].SubItems[0].Text);
                if (_xnkServices.Delete<NhanVien>(_xnkServices.GetById<NhanVien>(listView1.SelectedItems[0].SubItems[0].Text)))
                {
                    MessageBox.Show("Xoá nhân viên thành công!", "Success");
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else if (_xnkServices.SoftDelete<NhanVien>(_xnkServices.GetById<NhanVien>(listView1.SelectedItems[0].SubItems[0].Text)))
                {
                    MessageBox.Show("Xoá nhân viên thành công!", "Success");
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else
                {
                    MessageBox.Show("Xoá nhân viên thất bại!", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xoá nhân viên: " + ex.Message, "Error");
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 0)
            {
                cleanText();
                return;
            }
            var selectedItem = listView1.SelectedItems[0];
            textBoxMaNhanVien.Text = selectedItem.SubItems[0].Text;
            textBoxTenNhanVien.Text = selectedItem.SubItems[1].Text;
            comboBoxGioiTinh.Text = selectedItem.SubItems[2].Text;
            textBoxSoDienThoai.Text = selectedItem.SubItems[3].Text;
            textBoxEmail.Text = selectedItem.SubItems[4].Text;
            comboBoxChucVu.Text = selectedItem.SubItems[5].Text;
            textBoxLuong.Text = selectedItem.SubItems[6].Text;
            comboBoxTrangThai.Text = selectedItem.SubItems[7].Text;
            dateTimePickerNgaySinh.Value = _xnkServices.GetById<NhanVien>(selectedItem.SubItems[0].Text).NgaySinh;
            dateTimePickerNgayVaoLam.Value = _xnkServices.GetById<NhanVien>(selectedItem.SubItems[0].Text).NgayVaoLam;
            comboBoxKho.Text = _xnkServices.GetById<NhanVien>(selectedItem.SubItems[0].Text).Kho?.TenKho;
            textBoxGhiChu.Text = _xnkServices.GetById<NhanVien>(selectedItem.SubItems[0].Text).GhiChu;
        }
    }
}
