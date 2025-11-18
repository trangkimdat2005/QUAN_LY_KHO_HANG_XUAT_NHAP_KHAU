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
    public partial class USNhaCungCap : UserControl
    {
        private readonly IXNKServices _xnkServices;

        public USNhaCungCap()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USNhaCungCap_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã nhà cung cấp");
            listView1.Columns.Add("Tên nhà cung cấp");
            listView1.Columns.Add("Số điện thoại");
            listView1.Columns.Add("Email");
            listView1.Columns.Add("Địa chỉ");
            listView1.Columns.Add("Quốc gia");
            listView1.Columns.Add("Ghi chú");

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
            var dsNhaChungCap = _xnkServices.GetAll<NhaCungCap>();

            // Duyệt danh sách để thêm từng dòng
            foreach (var ncc in dsNhaChungCap)
            {
                ListViewItem item = new ListViewItem(ncc.MaNCC);
                item.SubItems.Add(ncc.TenNCC);
                item.SubItems.Add(ncc.SoDienThoai);
                item.SubItems.Add(ncc.Email);
                item.SubItems.Add(ncc.DiaChi);
                item.SubItems.Add(ncc.QuocGia);
                item.SubItems.Add(ncc.GhiChu);
                listView1.Items.Add(item);
            }
        }

        private string GetNextId()
        {
            using (var db = new XNKContextDB())
            {
                // Lấy bản ghi có Id lớn nhất hiện có 
                var last = db.NhaCungCaps
                             .OrderByDescending(p => p.MaNCC)
                             .FirstOrDefault();

                if (last == null)
                    return "NCC00001";

                // Cắt phần số phía sau
                string numberPart = last.MaNCC.Substring(3);
                int number = int.Parse(numberPart) + 1;
                return $"NCC{number:D5}";
            }
        }

        private void cleanText()
        {
            textBoxMaNCC.Text = GetNextId();
            textBoxTenNCC.Text = "";
            textBoxSDT.Text = "";
            textBoxEmail.Text = "";
            textBoxDiaChi.Text = "";
            textBoxGhiChu.Text = "";
            textBoxQuocGia.Text = "";
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                NhaCungCap ncc = new NhaCungCap
                {
                    MaNCC = textBoxMaNCC.Text.Trim(),
                    TenNCC = textBoxTenNCC.Text.Trim(),
                    SoDienThoai = textBoxSDT.Text.Trim(),
                    Email = textBoxEmail.Text.Trim(),
                    DiaChi = textBoxDiaChi.Text.Trim(),
                    QuocGia = textBoxQuocGia.Text.Trim(),
                    GhiChu = textBoxGhiChu.Text.Trim()
                };
                if (_xnkServices.Add<NhaCungCap>(ncc))
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cleanText();
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                }
                else
                {
                    MessageBox.Show("Thêm nhà cung cấp thất bại!", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 0)
            {
                cleanText();
                return;
            }
            textBoxMaNCC.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBoxTenNCC.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBoxSDT.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBoxEmail.Text = listView1.SelectedItems[0].SubItems[3].Text;
            textBoxDiaChi.Text = listView1.SelectedItems[0].SubItems[4].Text;
            textBoxQuocGia.Text = listView1.SelectedItems[0].SubItems[5].Text;
            textBoxGhiChu.Text = listView1.SelectedItems[0].SubItems[6].Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để cập nhật.", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                NhaCungCap nhaCungCap = _xnkServices.GetById<NhaCungCap>(textBoxMaNCC.Text.Trim());
                nhaCungCap.TenNCC = textBoxTenNCC.Text.Trim();
                nhaCungCap.SoDienThoai = textBoxSDT.Text.Trim();
                nhaCungCap.Email = textBoxEmail.Text.Trim();
                nhaCungCap.DiaChi = textBoxDiaChi.Text.Trim();
                nhaCungCap.QuocGia = textBoxQuocGia.Text.Trim();
                nhaCungCap.GhiChu = textBoxGhiChu.Text.Trim();
                if (_xnkServices.Update<NhaCungCap>(nhaCungCap))
                {
                    MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cleanText();
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                }
                else
                {
                    MessageBox.Show("Cập nhật nhà cung cấp thất bại!", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để làm mới.", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                _xnkServices.GetById<NhaCungCap>(listView1.SelectedItems[0].SubItems[0].Text);
                if (_xnkServices.Delete<NhaCungCap>(_xnkServices.GetById<NhaCungCap>(textBoxMaNCC.Text.Trim())))
                {
                    MessageBox.Show("Xoá nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else if (_xnkServices.SoftDelete<NhaCungCap>(_xnkServices.GetById<NhaCungCap>(textBoxMaNCC.Text.Trim())))
                {
                    MessageBox.Show("Xoá nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else
                {
                    MessageBox.Show("Xoá nhà cung cấp thất bại!", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xoá nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
