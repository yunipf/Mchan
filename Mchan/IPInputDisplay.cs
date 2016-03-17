using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mchan
{
    public partial class IPInputDisplay : Form
    {
        public IPInputDisplay(string ip)
        {
            InitializeComponent();
            
            ipInputBox.Text = ip;
        }

        private void joinButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(ipInputBox.Text, false);
            Dispose();           
        }



        private void cancelButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void spectateButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(ipInputBox.Text, false);
            Dispose();
        }
    }
}
