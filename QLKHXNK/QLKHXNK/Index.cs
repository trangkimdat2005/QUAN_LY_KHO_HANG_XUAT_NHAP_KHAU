using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKHXNK
{
    public partial class Index : Form
    {
        public string userName = "";
        public string passwordHash = "";
        private int _smoothTargetY = 0;
        private System.Windows.Forms.Timer _smoothTimer;

        public Index()
        {
            InitializeComponent();
        }

        private void Index_Load(object sender, EventArgs e)
        {
            using (var dlg = new FormDangNhap())
            {
                
                
            }
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.WrapContents = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
