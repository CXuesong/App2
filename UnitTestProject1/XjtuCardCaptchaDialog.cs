using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App2;

namespace UnitTestProject1
{
    public partial class XjtuCardCaptchaDialog : Form
    {
        public string UserInput { get; set; }

        public XjtuCardCaptchaDialog()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            UserInput = CaptchaCodeBox.Text;
            this.Close();
        }

        public string Run(Stream imageStream, SiteManager site)
        {
            CaptchaPictureBox.Image = Image.FromStream(imageStream);
            UserInput = null;
            this.Show();
            Application.Run(this);
            return UserInput;
        }
    }
}
