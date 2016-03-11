using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using CoreTweet;

namespace Mchan
{
    public partial class IDManager : Form
    {
        private Dictionary<int, UserData> userList = null;
        public IDManager()
        {
            InitializeComponent();
            MaximizeBox = false;
            MinimizeBox = false;

            userList = DBAccess.UserList;
        }

        private void ListBoxUpdate()
        {
            userListBox.DisplayMember = "ScreenName";
            userListBox.DataSource = userList.Values;
        }

        private void IDManager_Load(object sender, EventArgs e)
        {
            ListBoxUpdate();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            string consumerKey = Properties.Settings.Default.ConsumerKey;
            string consumerSecret = Properties.Settings.Default.ConsumerSecret;
            var session = OAuth.Authorize(consumerKey, consumerSecret);
            string url = session.AuthorizeUri.AbsoluteUri;
            System.Diagnostics.Process.Start(url);

            string pinCode = Interaction.InputBox("PINコードを入力", "", "", -1, -1);
            try
            {
                var tokens = OAuth.GetTokens(session, pinCode);
                var res = tokens.Account.VerifyCredentials();
                // ユーザデータの保存
                string id = res.Id.ToString();
                string screenName = res.ScreenName;
                string name = res.Name;
                string accessToken = tokens.AccessToken;
                string accessTokenSecret = tokens.AccessTokenSecret;

                UserData userData = new UserData(id, screenName, name, accessToken, accessTokenSecret);
                DBAccess.DBInsert(userData);
                userList = DBAccess.UserList;
                ListBoxUpdate();

            }
            catch (TwitterException ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("認証に失敗しました。");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void IDManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(userList.Count == 0)
            {
                if(!(e.CloseReason == CloseReason.ApplicationExitCall))
                {
                    
                    var result = MessageBox.Show(
                    "アカウントが登録されていません。終了してもよろしいですか？",
                    "確認",
                    MessageBoxButtons.OKCancel);

                    if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        Application.Exit();
                    }
                }              
            }
        }
    }
    
}
