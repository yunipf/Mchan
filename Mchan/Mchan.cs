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
        Tokens tokens = null;
        bool InitSetting = true;


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


        // bot動作開始メソッド
        public void StartAnalysis()
        {

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
            }
            catch (TwitterException ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("アプリ連携が解除されています。認証をやり直してください。");
            }
        }
    }

}