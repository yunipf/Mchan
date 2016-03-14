using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CoreTweet;
using CoreTweet.Streaming;



namespace Mchan
{

    public partial class Mchan : Form
    {
        private Dictionary<int, UserData> userList = null;
        //private Dictionary<string, PlayerData> playerDataList = null;
        private ConcurrentDictionary<string, PlayerData> playerDataList = null;
        //private BindingSource bindingSource = new BindingSource();
        private Process efzr = null;
        private Tokens tokens = null;
        private bool InitSetting = true;

        /* 設定項目 */

        // まっちんぐちゃんのUserId
        private long mchanUserId = 2905316520;
        // プレイヤーの残存時間　120分
        private TimeSpan timeSpan = TimeSpan.FromMinutes(180);
        // 取得ツイート数
        private int count = 20;
        // EFZフォルダパス
        private string efzFolderPath = @"F:\tasogare\EFZR\";

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
        
        
        delegate void SetPlayerListBoxCallback(ConcurrentDictionary<string, PlayerData> playerDataList);

        private void SetPlayerListBox(ConcurrentDictionary<string, PlayerData> playerDataList)
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
                if(playerDataList.Count == 0)
                {
                    setButtonEnabled(false);
                }
                else
                {
                    setButtonEnabled(true);
                }

            }
        }

        private void setButtonEnabled(bool enabled)
        {
            joinButton.Enabled = enabled;
            spectateButton.Enabled = enabled;
            replyButton.Enabled = enabled;
        }

        /// <summary>
        /// ツイート取得開始
        /// </summary>
        /// <returns></returns>
        private async Task StartAnalysis()
        {
            Task task1 = Task.Run(() =>
            {
            playerDataList = new ConcurrentDictionary<string, PlayerData>();

            var limitTime = DateTimeOffset.UtcNow - timeSpan;
                var res = tokens.Statuses.UserTimeline(user_id => mchanUserId, count => this.count)
                .Where((Status status) => status.CreatedAt > limitTime)
                .OrderBy((Status status) => status.CreatedAt);

                foreach (Status status in res) OperatePlayerList(status);
                
                /*
                foreach (Status status in res.Reverse())
                {
                    if (status.CreatedAt < limitTime)
                    {
                        continue;
                    }

                    OperatePlayerList(status);
                    /*
                    if (MatchMode.Host == getMatchMode(status.Text) || MatchMode.Client == getMatchMode(status.Text))
                    {
                        var playerData = new PlayerData(status.InReplyToScreenName, getName(status.Text),
                            getUserIp(status.Text), status.Text, getMatchMode(status.Text), status.CreatedAt + new TimeSpan(TimeSpan.TicksPerHour * 9));
                        
                        playerDataList.Remove(playerData.ScreenName);
                        playerDataList.Add(playerData.ScreenName, playerData);

                    }                    
                    else if(MatchMode.Close == getMatchMode(status.Text))
                    {
                        playerDataList.Remove(status.InReplyToScreenName);
                    }
                    
                    
                }*/

                SetPlayerListBox(playerDataList);
            }
            );

            await task1;

            Task task2 = Task.Run(() =>
            {
                while (true)
                {
                    Task.WaitAll(Task.Delay(TimeSpan.FromMinutes(10)));
                    PlayerData removedValue;
                    var limitTime = DateTimeOffset.UtcNow - timeSpan;
                    var res = from dic in playerDataList
                              where dic.Value.CreateAt < limitTime
                              select dic.Key;

                    foreach (string key in res)
                    {
                        playerDataList.TryRemove(key, out removedValue);
                    }
                    SetPlayerListBox(playerDataList);
                }
                
            });

            // ストリーミングでツイート取得
            var observable = tokens.Streaming.FilterAsObservable(follow => mchanUserId);
            observable.Catch(
                observable.DelaySubscription(TimeSpan.FromSeconds(10)).Retry()
            )
            .Repeat()
            .Where((StreamingMessage m) => m.Type == MessageType.Create)
                .Cast<StatusMessage>()
                //.Select((StatusMessage m) => m.Status.Text)
                .Select((StatusMessage m) => m.Status)
                .Subscribe(
                    //(string s) => MessageBox.Show(),
                    //(Status status) => OperatePlayerList(status),
                    (Status status) => {
                        OperatePlayerList(status);
                        SetPlayerListBox(playerDataList);
                    },
                    (Exception ex) => MessageBox.Show(ex.Message),
                    () => MessageBox.Show("終わり")
                 );

            /*
            tokens.Streaming.FilterAsObservable(follow => "128628857")
                .Where((StreamingMessage m) => m.Type == MessageType.Create)
                .Cast<StatusMessage>()
                .Select((StatusMessage m) => m.Status.Text)
                .Subscribe(
                    (string s) => MessageBox.Show(s),
                    (Exception ex) => MessageBox.Show(ex.Message),
                    () => MessageBox.Show("終わり")
                );
            */
        }

        /// <summary>
        /// playerListへのPlayerDataの追加、及び削除を行う
        /// </summary>
        /// <param name="status"></param>
        private void OperatePlayerList(Status status)
        {

            if (MatchMode.Host == getMatchMode(status.Text) || MatchMode.Client == getMatchMode(status.Text))
            {
               
                var playerData = new PlayerData(status.InReplyToScreenName, getName(status.Text),
                    getUserIp(status.Text), status.Text, getMatchMode(status.Text), status.CreatedAt);

                playerDataList.AddOrUpdate(playerData.ScreenName, playerData, (key, value) => playerData);
                    /*
                    value = playerData;
                    return value;
                    */
                
                /*
                playerDataList.Remove(playerData.ScreenName);
                playerDataList.Add(playerData.ScreenName, playerData);
                */
            }
            else if (MatchMode.Close == getMatchMode(status.Text))
            {
                PlayerData data;
                playerDataList.TryRemove(status.InReplyToScreenName, out data);
                
                //playerDataList.Remove(status.InReplyToScreenName);
            }
            //SetPlayerListBox(playerDataList);
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
            createAtLabel.Text = (player.CreateAt + TimeSpan.FromHours(9)).ToString("G");
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
                SendKeys.Send("1");
            }
            
        }

        private async void joinButton_Click(object sender, EventArgs e)
        {
            var player = (PlayerData)playerListBox.SelectedItem;
            Clipboard.SetDataObject(player.Ip, false);
            await EfzRun();
            SendKeys.Send("3");
        }

        private async void spectateButton_Click(object sender, EventArgs e)
        {
            var player = (PlayerData)playerListBox.SelectedItem;
            Clipboard.SetDataObject(player.Ip, false);
            await EfzRun();
            SendKeys.Send("4");
        }

        private async void offlineButton_Click(object sender, EventArgs e)
        {
            await EfzRun();
            SendKeys.Send("5");

        }

        //using System.Runtime.InteropServices;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);


        /// <summary>
        /// EFZRevivalをHostモードで起動
        /// </summary>
        /// <returns></returns>
        private async Task EfzRun()
        {
            Task task = Task.Run(() =>
            {
                if (efzr == null)
                {
                    efzr = new Process();
                    efzr.StartInfo.FileName = efzFolderPath + @"\EfzRevival.exe";
                    efzr.StartInfo.WorkingDirectory = efzFolderPath;
                    efzr.Start();
                }
                else if (efzr.HasExited)
                {
                    efzr.Start();
                }
                else
                {
                    SetForegroundWindow(efzr.MainWindowHandle);
                }

                
                Task.WaitAll(Task.Delay(TimeSpan.FromMilliseconds(500)));
                
            });
            await task;
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

                try
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(
                    efzFolderPath + @"\EfzRevival.ini",
                    Encoding.UTF8))
                    {
                        while (!sr.EndOfStream)
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                

            });

            Task combineTask = Task.WhenAll(task1, task2);
            await combineTask;
        }

        private async void joinManualButton_Click(object sender, EventArgs e)
        {
            string ip = getUserIp(Clipboard.GetText());
            DialogResult result = new IPInputDisplay(ip).ShowDialog();
            if(result == DialogResult.OK)
            {
                await EfzRun();
                SendKeys.Send("3");
            }
            
        }

        private void clientButton_Click(object sender, EventArgs e)
        {
            var sd = (Button)sender;
            var result = new TweetDisplay("", sd.Name, tokens).ShowDialog();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            var sd = (Button)sender;
            var result = new TweetDisplay("", sd.Name, tokens).ShowDialog();
        }
    }

}