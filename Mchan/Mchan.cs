using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using CoreTweet;
using CoreTweet.Streaming;



namespace Mchan
{

    public partial class Mchan : Form
    {
        private Dictionary<int, UserData> userList = null;
        private Dictionary<string, PlayerData> playerDataList = null;

        private BindingSource bindingSource = new BindingSource();
        private Tokens tokens = null;
        private bool InitSetting = true;

        /* 設定項目 */

        // まっちんぐちゃんのUserId
        private long mchanUserId = 2905316520;
        // プレイヤーの残存時間　120分
        private TimeSpan timeSpan = new TimeSpan(TimeSpan.TicksPerMinute * 600);
        // 取得ツイート数
        private int count = 20;
        // EFZRevival.exeのパス
        private string efzrPath = @"F:\tasogare\EFZR\EfzRevival.exe";
        // EFZRevival.iniのパス
        private string efzriniPath = @"F:\tasogare\EFZR\EfzRevival.ini";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Mchan()
        {
            InitializeComponent();
            MaximizeBox = false;            

            InitFileCheck();
            InitSetting = InitSettingCheck();
            

        }

        /// <summary>
        /// ユーザデータの存在確認
        /// </summary>
        private void InitFileCheck()
        {
            if (!System.IO.File.Exists(DBAccess.FileName))
            { 
                DBAccess.DBInit();
            }
        }

        /// <summary>
        /// アカウント認証済みか確認
        /// </summary>
        /// <returns></returns>
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
                Invoke(dlg, new object[] { text });
            }
            else
            {
                messageLabel.Text = text;
            }
            
        }
        
        
        delegate void SetPlayerListBoxCallback(Dictionary<string, PlayerData> playerDataList);

        private void SetPlayerListBox(Dictionary<string, PlayerData> playerDataList)
        {
            if (playerListBox.InvokeRequired)
            {
                SetPlayerListBoxCallback dlg = new SetPlayerListBoxCallback(SetPlayerListBox);
                Invoke(dlg, new object[] { playerDataList });
            }
            else
            {
                playerListBox.DisplayMember = "ScreenName";
                playerListBox.ValueMember = "ScreenName";

                playerListBox.DataSource = playerDataList.Values.ToList();

            }
        }

        /// <summary>
        /// ツイート取得開始
        /// </summary>
        /// <returns></returns>
        private async Task StartAnalysis()
        {
            Task task = Task.Run(() =>
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
                            getUserIp(status.Text), status.Text, getMatchMode(status.Text), status.CreatedAt + new TimeSpan(TimeSpan.TicksPerHour * 9));

                        playerDataList.Add(playerData.ScreenName, playerData);

                    }                    
                    else if(MatchMode.Close == getMatchMode(status.Text))
                    {
                        playerDataList.Remove(status.InReplyToScreenName);
                    }
                    
                }

                SetPlayerListBox(playerDataList);
            }
            );

            await task;
            
        }

        /// <summary>
        /// ツイートからScreenName取得。未使用
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// ツイートからユーザ名取得
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// ツイートからIP:Port取得
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// ツイートからモード取得
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 認証アカウントのリストを更新
        /// </summary>
        private void UserListUpdate()
        {
            userList = DBAccess.UserList;
            userListPullDown.DisplayMember = "ScreenName";
            userListPullDown.DataSource = userList.Values.ToList();
        }

        
        private async void Mchan_Shown(object sender, EventArgs e)
        {
            if (InitSetting)
            {
                new IDManager().ShowDialog();
                
            }
          
            UserListUpdate();
            userListPullDown.SelectedIndex = Properties.Settings.Default.UserListIndex;

            await GetIPAddress();
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
            createAtLabel.Text = player.CreateAt.ToString("G");
        }

        private async void userListPullDown_TextChanged(object sender, EventArgs e)
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
                tokens.ScreenName = res.ScreenName;
                user.ScreenName = res.ScreenName;
                user.Name = res.Name;

                DBAccess.DBUpdate(user);
                userList = DBAccess.UserList;

                await StartAnalysis();
            }
            catch (TwitterException ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("アプリ連携が解除されています。認証をやり直してください。");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            playerListBox.Focus();
        }

        private void replyButton_Click(object sender, EventArgs e)
        {
            var sd = (Button)sender;
            new TweetDisplay(playerListBox.SelectedValue.ToString(), sd.Name, tokens).ShowDialog();
        }

        private async void hostButton_Click(object sender, EventArgs e)
        {
            var sd = (Button)sender;
            var result = new TweetDisplay(
                Properties.Settings.Default.IPAddress + ":" + 
                Properties.Settings.Default.Port
                , sd.Name
                , tokens).ShowDialog();

            if(DialogResult.OK == result)
            {
                await EfzRun();
            }
            
        }

        /// <summary>
        /// EFZRevivalをHostモードで起動
        /// </summary>
        /// <returns></returns>
        private async Task EfzRun()
        {
            Task task = Task.Run(() =>
            {
                Process efzr = new Process();

                efzr.StartInfo.FileName = efzrPath;
                efzr.Start();
                Task.WaitAll(Task.Delay(TimeSpan.FromMilliseconds(500)));
                
            });
            await task;
            SendKeys.Send("1");
        }

        /// <summary>
        /// グローバルIPアドレス、使用ポート番号の取得
        /// </summary>
        /// <returns></returns>
        private async Task GetIPAddress()
        {
            Task task1 = Task.Run(() =>
            {
                UPnPControlPoint p = new UPnPControlPoint();
                var result = p.GetExternalIPAddress();

                Properties.Settings.Default.IPAddress = result;
            });

            Task task2 = Task.Run(() =>
            {
                string result = "";
                string pattern = @"^Port\s*=\s*[0-9]{1,5}";
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None);
                System.Text.RegularExpressions.Match match;

                using (System.IO.StreamReader sr = new System.IO.StreamReader(
                    efzriniPath,
                    Encoding.UTF8))
                {
                    while(!sr.EndOfStream)
                    {
                        string text = sr.ReadLine();
                        match = regex.Match(text);

                        if (match.Success)
                        {
                            result = match.ToString();
                            break;
                        }
                    }

                }
                pattern = @"[0-9]{1,5}";
                regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None);
                match = regex.Match(result);
                if (match.Success)
                {
                    Properties.Settings.Default.Port = match.ToString();
                }

            });

            Task combineTask = Task.WhenAll(task1, task2);
            await combineTask;
        }
    }

}