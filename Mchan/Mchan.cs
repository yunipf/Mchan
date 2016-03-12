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
    public partial class Mchan : Form
    {
        private Dictionary<int, UserData> userList = null;
        private Tokens tokens = null;
        private bool InitSetting = true;
        


        public Mchan()
        {
            InitializeComponent();
            MaximizeBox = false;

            InitFileCheck();
            InitSetting = InitSettingCheck();
            

        }

        // ユーザデータの存在確認
        private void InitFileCheck()
        {
            if (!System.IO.File.Exists(DBAccess.FileName))
            { 
                DBAccess.DBInit();
            }
        }

        // アカウントが登録してあるか確認
        private bool InitSettingCheck()
        {
            bool bl = false;
            if(DBAccess.DBSelectAll() < 1)
            {
                bl = true;
            }
            else
            {
                bl = false;
            }
            return bl;
        }

        // Windowsフォームコントロールに対して非同期な呼び出しを行うためのデリゲート
        delegate void SetMessageLabelCallback(string text);

        private void SetMessageLabel(string text)
        {
            if (messageLabel.InvokeRequired)
            {
                SetMessageLabelCallback dlg = new SetMessageLabelCallback(SetMessageLabel);
                this.Invoke(dlg, new object[] { text });
            }
            else
            {
                messageLabel.Text = text;
            }
            
        }

        // bot動作開始メソッド
        private void StartAnalysis()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var res = tokens.Statuses.UserTimeline(screen_name => "e_f_z_match" , count => 20);
                    foreach(Status status in res)
                    {
                        string text = status.Text;
                        SetMessageLabel(text);
                        //messageLabel.Text = text;
                        Task.WaitAll(Task.Delay(TimeSpan.FromMilliseconds(2000)));
                    }
                    Task.WaitAll(Task.Delay(TimeSpan.FromMilliseconds(10000)));
                }
            });

        }

        private void UserListUpdate()
        {
            userList = DBAccess.UserList;
            userListPullDown.DisplayMember = "ScreenName";
            userListPullDown.DataSource = userList.Values.ToList();
        }


        private void Mchan_Shown(object sender, EventArgs e)
        {
            if (InitSetting)
            {
                new IDManager().ShowDialog();
                
            }
          
            UserListUpdate();
            userListPullDown.SelectedIndex = Properties.Settings.Default.UserListIndex;
        }

        private void managerButton_Click(object sender, EventArgs e)
        {
            new IDManager().ShowDialog();
            UserListUpdate();
        }

        private void Mchan_FormClosed(object sender, FormClosedEventArgs e)
        {
            int userListIndex = userListPullDown.SelectedIndex;
            Properties.Settings.Default.UserListIndex = userListIndex;
            Properties.Settings.Default.Save();
        }

        private void playerList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void userListPullDown_TextChanged(object sender, EventArgs e)
        {
            UserData user = (UserData)userListPullDown.SelectedItem;
            string accessToken = user.AccessToken;
            string accessTokenSecret = user.AccessTokenSecret;
            var key = new KeyData();
            string consumerKey = key.ConsumerKey;
            string consumerSecret = key.ConsumerSecret;
            tokens = Tokens.Create(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            try
            {
                var res = tokens.Account.VerifyCredentials();
                user.ScreenName = res.ScreenName;
                user.Name = res.Name;

                DBAccess.DBUpdate(user);
                userList = DBAccess.UserList;

                StartAnalysis();
            }
            catch (TwitterException ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("アプリ連携が解除されています。認証をやり直してください。");
            }
        }
    }

}