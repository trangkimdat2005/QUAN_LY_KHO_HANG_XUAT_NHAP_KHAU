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
    public partial class USPhieuNhap : UserControl
    {
        private readonly IXNKServices _xnkServices;
        public USPhieuNhap()
        {
            InitializeComponent();
            _xnkServices = new XNKServices();
        }

        private void USPhieuNhap_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Mã phiếu nhập");
            listView1.Columns.Add("Ngày nhập");
            listView1.Columns.Add("Nhà cung cấp");
            listView1.Columns.Add("Kho");
            listView1.Columns.Add("Tổng số lượng");
            listView1.Columns.Add("Ghi chú");


            var nhaCungCaps = _xnkServices.GetAll<NhaCungCap>();
            comboBoxNhaCungCap.DataSource = nhaCungCaps;
            comboBoxNhaCungCap.DisplayMember = "TenNCC";
            comboBoxNhaCungCap.ValueMember = "MaNCC";
            // Load dữ liệu cho comboBoxKho
            var khos = _xnkServices.GetAll<Kho>();
            comboBoxKho.DataSource = khos;
            comboBoxKho.DisplayMember = "TenKho";
            comboBoxKho.ValueMember = "MaKho";

            cleanText();
            LoadListViewData();
            AdjustListViewColumns(listView1);
            dateTimePickerNgayNhap.MaxDate = DateTime.Today;
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
            var dsPhieuNhap = _xnkServices.GetAll<PhieuNhap>();

            // Duyệt danh sách để thêm từng dòng
            foreach (var pn in dsPhieuNhap)
            {
                ListViewItem item = new ListViewItem(pn.MaPN);
                item.SubItems.Add(pn.NgayNhap.ToShortDateString());
                item.SubItems.Add(_xnkServices.GetById<NhaCungCap>(pn.MaNCC).TenNCC);
                item.SubItems.Add(_xnkServices.GetById<Kho>(pn.MaKho).TenKho);
                item.SubItems.Add(pn.TongSoLuong.ToString());
                item.SubItems.Add(pn.GhiChu);
                listView1.Items.Add(item);
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

        private void cleanText()
        {
            textBoxMaPN.Text = GetNextId();
            dateTimePickerNgayNhap.Value = DateTime.Today;
            comboBoxKho.Text = "";
            comboBoxNhaCungCap.Text = "";
            textBoxGhiChu.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                PhieuNhap pn = new PhieuNhap
                {
                    MaPN = textBoxMaPN.Text,
                    NgayNhap = dateTimePickerNgayNhap.Value,
                    MaNCC = comboBoxNhaCungCap.SelectedValue.ToString(),
                    MaKho = comboBoxKho.SelectedValue.ToString(),
                    TongSoLuong = 0,
                    GhiChu = textBoxGhiChu.Text
                };
                if (_xnkServices.Add<PhieuNhap>(pn))
                {
                    MessageBox.Show("Thêm phiếu nhập thành công!");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Thêm phiếu nhập thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm phiếu nhập: " + ex.Message, "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập để làm mới!", "Thông báo");
                return;
            }
            try
            {
                string maPN = listView1.SelectedItems[0].SubItems[0].Text;
                PhieuNhap pnToUpdate = _xnkServices.GetById<PhieuNhap>(maPN);
                pnToUpdate.NgayNhap = dateTimePickerNgayNhap.Value;
                pnToUpdate.MaNCC = comboBoxNhaCungCap.SelectedValue.ToString();
                pnToUpdate.MaKho = comboBoxKho.SelectedValue.ToString();
                pnToUpdate.GhiChu = textBoxGhiChu.Text;
                if (_xnkServices.Update<PhieuNhap>(pnToUpdate))
                {
                    MessageBox.Show("Cập nhật phiếu nhập thành công!", "Thông báo");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Cập nhật phiếu nhập thất bại!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật phiếu nhập: " + ex.Message, "Lỗi");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập để xóa!", "Thông báo");
                return;
            }
            try
            {
                string maPN = listView1.SelectedItems[0].SubItems[0].Text;
                PhieuNhap pnToDelete = _xnkServices.GetById<PhieuNhap>(maPN);
                if (_xnkServices.Delete<PhieuNhap>(pnToDelete))
                {
                    MessageBox.Show("Xoá phiếu nhập thành công!", "Thông báo");
                    cleanText();
                    LoadListViewData();
                }
                else if(_xnkServices.SoftDelete<PhieuNhap>(pnToDelete))
                {
                    MessageBox.Show("Phiếu nhập có liên quan đến dữ liệu khác, đã xoá mềm phiếu nhập thành công!", "Thông báo");
                    cleanText();
                    LoadListViewData();
                }
                else
                {
                    MessageBox.Show("Xoá phiếu nhập thất bại!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xoá phiếu nhập: " + ex.Message, "Lỗi");
            }
        }
    }
}
