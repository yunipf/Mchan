using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Runtime.Serialization.Formatters.Binary;
using CoreTweet;

namespace Mchan
{
    public partial class IDManager : Form
    {
        private string settingFile = null;
        private Setting setting = null;
        private Dictionary<int, UserData> userList = null;
        public IDManager(string settingFile)
        {
            InitializeComponent();
            MaximizeBox = false;
            MinimizeBox = false;

            this.settingFile = settingFile;
            OpenSetting();
            userList = DBAccess.UserList;

        }

        /// <summary>
        /// 認証アカウント一覧の更新
        /// </summary>
        private void ListBoxUpdate()
        {
            
            userListBox.DisplayMember = "ScreenName";
            userListBox.ValueMember = "Id";
            userListBox.DataSource = userList.Values.ToList();
            

        }

        /// <summary>
        /// フォームロード時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IDManager_Load(object sender, EventArgs e)
        {
            efzPathText.Text = setting.EfzFolderPath;
            limitTimeBox.Text = setting.TimeSpan.TotalMinutes.ToString();
            ListBoxUpdate();
        }

        /// <summary>
        /// 削除ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            UserData user = (UserData)userListBox.SelectedItem;
            //string id = userListBox.SelectedValue.ToString();

            DBAccess.DBDelete(user);
            userList = DBAccess.UserList;
            ListBoxUpdate();
            
            
        }

        /// <summary>
        /// 設定ファイルから設定クラス取得
        /// </summary>
        private void OpenSetting()
        {
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(settingFile, System.IO.FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    setting = (Setting)bf.Deserialize(fs);
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                setting = new Setting();
            }

        }

        /// <summary>
        /// 設定ファイルに保存
        /// </summary>
        private void SaveSetting()
        {
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(settingFile, System.IO.FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, setting);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// アカウント追加ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addUserButton_Click(object sender, EventArgs e)
        {
            var key = new KeyData();
            string consumerKey = key.ConsumerKey;
            string consumerSecret = key.ConsumerSecret;

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

        /// <summary>
        /// フォームを閉じる時の確認イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            else if(!System.IO.File.Exists(efzPathText.Text + @"\EfzRevival.exe"))
            {
                if (!(e.CloseReason == CloseReason.ApplicationExitCall))
                {

                    var result = MessageBox.Show(
                    "フォルダが正しく指定されていません。終了してもよろしいですか？",
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


        /// <summary>
        /// フォルダ参照ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "EfzRevival.exeの入ったフォルダを指定";
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowserDialog1.ShowNewFolderButton = false;

            while(folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath + @"\EfzRevival.exe";
                if (System.IO.File.Exists(path)){
                    efzPathText.Text = folderBrowserDialog1.SelectedPath;
                    break;
                }
                else
                {
                    MessageBox.Show("EfzRevival.exeが見つかりません");
                }
            }
        }

        /// <summary>
        /// OKボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            string path = efzPathText.Text + @"\EfzRevival.exe";
            if (System.IO.File.Exists(path))
            {
                setting.EfzFolderPath = efzPathText.Text;
            }

            setting.TimeSpan = TimeSpan.FromMinutes(int.Parse(limitTimeBox.Text));
            SaveSetting();
            Close();
        }
    }
    
}
