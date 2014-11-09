using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FTP_CLIENT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        FtpClient ftp = new FtpClient();
        string serverDir = "";

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            /* Указание переменных */
            ftp.Host = textHost.Text;
            ftp.UserName = TextLogin.Text;
            ftp.Password = TextPass.Text;
            if (ftp.Host == "")
            { MessageBox.Show("Укажите имя сервера"); return; }
        }
    }
}
