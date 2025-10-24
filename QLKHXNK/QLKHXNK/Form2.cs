using QLKHXNK.Views.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKHXNK
{
    public partial class Form2 : Form
    {

        [DllImport("user32.dll")]
        private static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

        private const int SB_HORZ = 0;
        private const int SB_VERT = 1;


        public Form2()
        {
            InitializeComponent();
        }

        // Xử lý sự kiện Load của Form
        private void Form2_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Scroll += (s, ev) => HideScrollBars(flowLayoutPanel1);
            flowLayoutPanel1.Layout += (s, ev) => HideScrollBars(flowLayoutPanel1);
            flowLayoutPanel1.AutoScroll = true;
        }

        // Ẩn thanh cuộn của FlowLayoutPanel
        private void HideScrollBars(FlowLayoutPanel panel)
        {
            ShowScrollBar(panel.Handle, SB_VERT, 0); // 0 = ẩn
            ShowScrollBar(panel.Handle, SB_HORZ, 0);
        }

        // Hiển thị UserControl con trong panel3
        private void OpenChildUserControl(UserControl childUserControl)
        {
            // Xóa UserControl cũ (nếu có)
            if (this.panel3.Controls.Count > 0)
                this.panel3.Controls.RemoveAt(0);

            childUserControl.Dock = DockStyle.Fill; // chiếm toàn bộ panel

            // Thêm vào panel
            this.panel3.Controls.Add(childUserControl);
            this.panel3.Tag = childUserControl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildUserControl(new USHangHoa());
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildUserControl(new USNhaCungCap());
        }
    }
}
