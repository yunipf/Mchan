using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoreTweet;

namespace Mchan
{
    public partial class TweetDisplay : Form
    {
        private Tokens tokens = null;
        public TweetDisplay()
        {
            InitializeComponent();
        }

        public TweetDisplay(string text, string buttonName, Tokens tokens)
        {
            InitializeComponent();
            this.tokens = tokens;
            
            switch (buttonName)
            {
                case "replyButton":
                    tweetTextBox.Text = "@" + text + " ";
                    break;
                case "closeButton":
                    tweetTextBox.Text = "ereve";
                    break;
                case "hostButton":
                    tweetTextBox.Text = text + " erevm";
                    break;
                case "clientButton":
                    tweetTextBox.Text = "erevc";
                    break;
                default:
                    break;
            }
        }


        private void tweetTextBox_TextChanged(object sender, EventArgs e)
        {
            lengthLabel.Text = (140 - tweetTextBox.Text.Length).ToString();
        }

        private void tweetButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "@" + tokens.ScreenName + " からツイートしますか？" ,
                "確認",
                MessageBoxButtons.OKCancel);

            if(result == DialogResult.OK)
            {
                try
                {
                    tokens.Statuses.Update(status => tweetTextBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DialogResult = DialogResult.OK;
                Dispose();
            }
        }
    }
}
