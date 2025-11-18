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
    public partial class USPhieuXuat : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USPhieuXuat()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USPhieuXuat_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã phiếu xuất");
            listView1.Columns.Add("Ngày xuất");
            listView1.Columns.Add("Khách hàng");
            listView1.Columns.Add("Kho");
            listView1.Columns.Add("Tổng số lượng");
            listView1.Columns.Add("Ghi chú");

            var khachHangs = _xnkServices.GetAll<KhachHang>();
            comboBoxKhachHang.DataSource = khachHangs;
            comboBoxKhachHang.DisplayMember = "TenKH";
            comboBoxKhachHang.ValueMember = "MaKH";


            // Load dữ liệu cho comboBoxKho
            var khos = _xnkServices.GetAll<Kho>();
            comboBoxKho.DataSource = khos;
            comboBoxKho.DisplayMember = "TenKho";
            comboBoxKho.ValueMember = "MaKho";

            cleanText();
            LoadListViewData();
            AdjustListViewColumns(listView1);
            dateTimePickerNgayXuat.MaxDate = DateTime.Today;
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
            var dsPhieuXuat = _xnkServices.GetAll<PhieuXuat>();

            // Duyệt danh sách để thêm từng dòng
            foreach (var px in dsPhieuXuat)
            {
                ListViewItem item = new ListViewItem(px.MaPX);
                item.SubItems.Add(px.NgayXuat.ToShortDateString());
                item.SubItems.Add(_xnkServices.GetById<KhachHang>(px.MaKH).TenKH);
                item.SubItems.Add(_xnkServices.GetById<Kho>(px.MaKho).TenKho);
                item.SubItems.Add(px.TongSoLuong.ToString());
                item.SubItems.Add(px.GhiChu);
                listView1.Items.Add(item);
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

        private void cleanText()
        {
            textBoxMaPX.Text = GetNextId();
            dateTimePickerNgayXuat.Value = DateTime.Today;
            comboBoxKho.Text = "";
            comboBoxKhachHang.Text = "";
            textBoxGhiChu.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                PhieuXuat px = new PhieuXuat
                {
                    MaPX = textBoxMaPX.Text,
                    NgayXuat = dateTimePickerNgayXuat.Value,
                    MaKH = comboBoxKhachHang.SelectedValue.ToString(),
                    MaKho = comboBoxKho.SelectedValue.ToString(),
                    TongSoLuong = 0, // Mặc định là 0, sẽ cập nhật khi thêm chi tiết
                    GhiChu = textBoxGhiChu.Text
                };

                if (_xnkServices.Add<PhieuXuat>(px))
                {
                    MessageBox.Show("Thêm phiếu xuất thành công!");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Thêm phiếu xuất thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm phiếu xuất: " + ex.Message, "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất để sửa.");
                return;
            }
            try
            {
                PhieuXuat px = _xnkServices.GetById<PhieuXuat>(textBoxMaPX.Text);
                px.NgayXuat = dateTimePickerNgayXuat.Value;
                px.MaKH = comboBoxKhachHang.SelectedValue.ToString();
                px.MaKho = comboBoxKho.SelectedValue.ToString();
                px.GhiChu = textBoxGhiChu.Text;
                if (_xnkServices.Update<PhieuXuat>(px))
                {
                    MessageBox.Show("Cập nhật phiếu xuất thành công!", "Thông báo");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Cập nhật phiếu xuất thất bại!", "Lỗi");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật phiếu xuất: " + ex.Message, "Lỗi");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất để xóa.");
                return;
            }
            try
            {
                PhieuXuat phieuXuat = _xnkServices.GetById<PhieuXuat>(listView1.SelectedItems[0].SubItems[0].Text);
                if (_xnkServices.Delete<PhieuXuat>(phieuXuat))
                {
                    MessageBox.Show("Xóa phiếu xuất thành công!", "Thông báo");
                    cleanText();
                    LoadListViewData();
                }
                else if (_xnkServices.SoftDelete<PhieuXuat>(phieuXuat))
                {
                    MessageBox.Show("Phiếu xuất có liên quan đến dữ liệu khác, đã xoá mềm phiếu xuất thành công!", "Thông báo");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Xóa phiếu xuất thất bại!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa phiếu xuất: " + ex.Message, "Lỗi");
            }
        }
    }
}
