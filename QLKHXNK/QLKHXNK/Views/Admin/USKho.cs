using QLKHXNK.Models.Entities;
using QLKHXNK.Services;
using System;
using System.CodeDom.Compiler;
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
    public partial class USKho : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USKho()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USKho_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã Kho");
            listView1.Columns.Add("Tên Kho");
            listView1.Columns.Add("Diện tích");
            listView1.Columns.Add("Loại kho");
            listView1.Columns.Add("Sức chứa");
            listView1.Columns.Add("Trạng thái");
            listView1.Columns.Add("Địa chỉ");
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
            var dsKho = _xnkServices.GetAll<Kho>();

            // Duyệt danh sách để thêm từng dòng
            foreach (var k in dsKho)
            {
                ListViewItem item = new ListViewItem(k.MaKho);
                item.SubItems.Add(k.TenKho);
                item.SubItems.Add(k.DienTich.ToString());
                item.SubItems.Add(k.LoaiKho);
                item.SubItems.Add(k.SucChua.ToString());
                item.SubItems.Add(k.TrangThai);
                item.SubItems.Add(k.DiaChi);
                item.SubItems.Add(k.GhiChu);
                listView1.Items.Add(item);
            }
        }

        private string GetNextId()
        {
            using (var db = new XNKContextDB())
            {
                // Lấy bản ghi có Id lớn nhất hiện có 
                var last = db.Khoes
                             .OrderByDescending(p => p.MaKho)
                             .FirstOrDefault();

                if (last == null)
                    return "KHO00001";

                // Cắt phần số phía sau
                string numberPart = last.MaKho.Substring(3);
                int number = int.Parse(numberPart) + 1;
                return $"KHO{number:D5}";
            }
        }

        private void cleanText()
        {
            textBoxMaKho.Text = GetNextId();
            textBoxTenKho.Text = "";
            textBoxDienTich.Text = "";
            comboBoxLoaiKho.SelectedIndex = 0;
            numericUpDownSucChua.Value = 1;
            comboBoxTrangThai.SelectedIndex = 0;
            textBoxDiaChi.Text = "";
            textBoxGhiChu.Text = "";
        }

        private void textBoxDienTich_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu người dùng nhập ký tự không phải là số hoặc dấu chấm
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '.')
            {
                e.Handled = true;  // Chặn ký tự không hợp lệ
            }
        }

        private void textBoxDienTich_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == null) return;

            // Tạm thời gỡ bỏ sự kiện để tránh vòng lặp vô hạn khi chúng ta thay đổi Text
            txt.TextChanged -= textBoxDienTich_TextChanged;

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
            txt.TextChanged += textBoxDienTich_TextChanged;
        }

        private void textBoxDienTich_Validating(object sender, CancelEventArgs e)
        {
            string DT = textBoxDienTich.Text.Trim();

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
                MessageBox.Show("Diện tích quá lớn. ",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                e.Cancel = true; // Giữ người dùng ở lại để sửa
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Kho kho = new Kho();
                kho.MaKho = textBoxMaKho.Text;
                kho.TenKho = textBoxTenKho.Text.Trim();
                kho.DienTich = (decimal)(string.IsNullOrWhiteSpace(textBoxDienTich.Text) ? (decimal?)null : decimal.Parse(textBoxDienTich.Text));
                kho.LoaiKho = comboBoxLoaiKho.Text;
                kho.SucChua = (int)numericUpDownSucChua.Value;
                kho.TrangThai = comboBoxTrangThai.Text;
                kho.DiaChi = textBoxDiaChi.Text.Trim();
                kho.GhiChu = textBoxGhiChu.Text.Trim();

                if (_xnkServices.Add<Kho>(kho))
                {
                    MessageBox.Show("Thêm kho thành công!",
                                    "Thành công",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    LoadListViewData();
                    AdjustListViewColumns(listView1);
                    cleanText();
                }
                else
                {
                    MessageBox.Show("Thêm kho thất bại!",
                                    "Lỗi",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn kho để làm mới.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }
            try
            {
                string maKho = listView1.SelectedItems[0].SubItems[0].Text;
                var kho = _xnkServices.GetById<Kho>(maKho);
                if (kho != null)
                {
                    kho.TenKho = textBoxTenKho.Text.Trim();
                    kho.DienTich = (decimal)(string.IsNullOrWhiteSpace(textBoxDienTich.Text) ? (decimal?)null : decimal.Parse(textBoxDienTich.Text));
                    kho.LoaiKho = comboBoxLoaiKho.Text;
                    kho.SucChua = (int)numericUpDownSucChua.Value;
                    kho.TrangThai = comboBoxTrangThai.Text;
                    kho.DiaChi = textBoxDiaChi.Text.Trim();
                    kho.GhiChu = textBoxGhiChu.Text.Trim();
                    if (_xnkServices.Update<Kho>(kho))
                    {
                        MessageBox.Show("Cập nhật kho thành công!",
                                        "Thành công",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        LoadListViewData();
                        AdjustListViewColumns(listView1);
                        cleanText();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật kho thất bại!",
                                        "Lỗi",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn kho để xóa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }
            try
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa kho này?",
                                             "Xác nhận",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Kho maKho = _xnkServices.GetById<Kho>(listView1.SelectedItems[0].SubItems[0].Text);
                    if (_xnkServices.Delete<Kho>(maKho))
                    {
                        MessageBox.Show("Xóa kho thành công!",
                                        "Thành công",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        LoadListViewData();
                        AdjustListViewColumns(listView1);
                        cleanText();
                    }
                    else if(_xnkServices.SoftDelete<Kho>(maKho))
                    {
                        MessageBox.Show("Kho đang được sử dụng, đã chuyển sang trạng thái xóa mềm!",
                                        "Thành công",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        LoadListViewData();
                        AdjustListViewColumns(listView1);
                        cleanText();
                    }
                    else
                    {
                        MessageBox.Show("Xóa kho thất bại!",
                                        "Lỗi",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 0)
            {
                cleanText();
                return;
            }
            textBoxMaKho.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBoxTenKho.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBoxDienTich.Text = listView1.SelectedItems[0].SubItems[2].Text;
            comboBoxLoaiKho.Text = listView1.SelectedItems[0].SubItems[3].Text;
            numericUpDownSucChua.Value = int.Parse(listView1.SelectedItems[0].SubItems[4].Text);
            comboBoxTrangThai.Text = listView1.SelectedItems[0].SubItems[5].Text;
            textBoxDiaChi.Text = listView1.SelectedItems[0].SubItems[6].Text;
            textBoxGhiChu.Text = listView1.SelectedItems[0].SubItems[7].Text;
        }
    }
}
