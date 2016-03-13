using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoreTweet;
using CoreTweet.Streaming;


namespace Mchan
{
    public partial class Mchan : Form
    {
        private Dictionary<int, UserData> userList = null;
        private Dictionary<string, PlayerData> playerDataList = null;
        //private List<PlayerData> playerList = null;
        private Tokens tokens = null;
        private bool InitSetting = true;

        /* 設定項目 */

        // まっちんぐちゃんのUserId
        private long mchanUserId = 2905316520;
        // プレイヤーの残存時間　120分
        private TimeSpan timeSpan = new TimeSpan(TimeSpan.TicksPerMinute * 480);
        // 取得ツイート数
        private int count = 20;


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
        /*
        PlayerListに必要なパラメータ
        screenName
        name
        
        ip:port
        erevm or ereve or erevc
        message


        */
        delegate void SetPlayerListBoxCallback(Dictionary<string, PlayerData> playerDataList);

        private void SetPlayerListBox(Dictionary<string, PlayerData> playerDataList)
        {
            if (playerListBox.InvokeRequired)
            {
                SetPlayerListBoxCallback dlg = new SetPlayerListBoxCallback(SetPlayerListBox);
                this.Invoke(dlg, new object[] { playerDataList });
            }
            else
            {
                playerListBox.DisplayMember = "ScreenName";
                playerListBox.DataSource = playerDataList.Values.ToList();
            }
        }

        // bot動作開始メソッド

        /*
        private void StartAnalysis()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    // 現在時刻の取得
                    var now = DateTimeOffset.UtcNow;
                    // 一時間前の時刻にする
                    now -= timeSpan;
                    var res = tokens.Statuses.UserTimeline(screen_name => "e_f_z_match" , count => this.count);
                    foreach(Status status in res.Reverse())
                    {
                        string text = status.InReplyToScreenName;
                        var createAt = status.CreatedAt;


                        SetMessageLabel(createAt.ToString());
                        //messageLabel.Text = text;

                        Task.WaitAll(Task.Delay(TimeSpan.FromMilliseconds(2000)));
                    }
                    Task.WaitAll(Task.Delay(TimeSpan.FromMilliseconds(10000)));
                }
            });

        }
        */
        private void StartAnalysis()
        {
            Task.Run(() =>
            {
                playerDataList = new Dictionary<string, PlayerData>();
                // 現在時刻の取得
                var now = DateTimeOffset.UtcNow;
                now -= timeSpan;
                var res = tokens.Statuses.UserTimeline(user_id => mchanUserId, count => this.count);

                foreach (Status status in res.Reverse())
                {
                    if (status.CreatedAt < now)
                    {
                        continue;
                    }

                    if (MatchMode.Host == getMatchMode(status.Text) || MatchMode.Client == getMatchMode(status.Text))
                    {
                        var playerData = new PlayerData(status.InReplyToScreenName, getName(status.Text),
                            getUserIp(status.Text), status.Text, getMatchMode(status.Text), status.CreatedAt);
                        playerDataList.Add(playerData.ScreenName, playerData);

                    }
                    /*
                    else if(MatchMode.Close == getMatchMode(status.Text))
                    {
                        playerDataList.Remove(status.InReplyToScreenName);
                    }
                    */
                }

                SetPlayerListBox(this.playerDataList);
            }
            );
            /*
            playerDataList = new Dictionary<string, PlayerData>();
            // 現在時刻の取得
            var now = DateTimeOffset.UtcNow;
            now -= timeSpan;
            var res = tokens.Statuses.UserTimeline(user_id => mchanUserId, count => this.count);
            
            foreach (Status status in res.Reverse())
            {
                if(status.CreatedAt < now)
                {
                    continue;
                }

                if(MatchMode.Host == getMatchMode(status.Text) || MatchMode.Client == getMatchMode(status.Text))
                {
                    var playerData = new PlayerData(status.InReplyToScreenName, getName(status.Text), 
                        getUserIp(status.Text), status.Text, getMatchMode(status.Text), status.CreatedAt);
                    playerDataList.Add(playerData.ScreenName, playerData);

                }
                /*
                else if(MatchMode.Close == getMatchMode(status.Text))
                {
                    playerDataList.Remove(status.InReplyToScreenName);
                }
                
            }

            SetPlayerListBox(this.playerDataList);
            */
        }

        private string getScreenName(string text)
        {
            string result = "";
            string pattern = @"(?<=@).*(?=\))";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None);
            System.Text.RegularExpressions.Match match = regex.Match(text);

            if (match.Success)
            {
                result = match.ToString();
            }

            return result;

        }

        private string getName(string text)
        {
            string result = "";
            string pattern = @"^.*(?=\()";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None);
            System.Text.RegularExpressions.Match match = regex.Match(text);

            if (match.Success)
            {
                result = match.ToString();
            }

            return result;
        }

        private string getUserIp(string text)
        {
            string result = "";
            string pattern = @"[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}:[0-9]{1,5}";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None);
            System.Text.RegularExpressions.Match match = regex.Match(text);

            if (match.Success)
            {
                result = match.ToString();
            }
            
            return result;
        }

        private MatchMode getMatchMode(string text)
        {
            string result;
            MatchMode matchMode;
            string pattern = @"(erevm|ereve|erevc)";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None);
            System.Text.RegularExpressions.Match match = regex.Match(text);

            if (match.Success)
            {
                result = match.ToString();
                switch (result)
                {
                    case "erevm":
                        matchMode = MatchMode.Host;
                        break;
                    case "ereve":
                        matchMode = MatchMode.Close;
                        break;
                    case "erevc":
                        matchMode = MatchMode.Client;
                        break;
                    default:
                        matchMode = MatchMode.None;
                        break;
                }

            }else
            {
                matchMode = MatchMode.None;
            }

            return matchMode;
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
            var player = (PlayerData)playerListBox.SelectedItem;
            messageLabel.Text = player.Message;
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