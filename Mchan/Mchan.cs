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
using System.Runtime.Serialization.Formatters.Binary;
using CoreTweet;
using CoreTweet.Streaming;



namespace Mchan
{

    public partial class Mchan : Form
    {
        // アカウント一覧
        private Dictionary<int, UserData> userList = null;
        // 募集中のプレイヤー一覧
        private ConcurrentDictionary<string, PlayerData> playerDataList = null;
        private Process efzr = null;
        private Tokens tokens = null;
        private bool InitSetting = true;

        private Setting setting = null;

        /* 設定項目 */

        // まっちんぐちゃんのUserId
        private long mchanUserId = 2905316520;
        // 取得ツイート数
        private int count = 20;
        // 設定ファイル
        private string settingFile = @"Setting";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Mchan()
        {
            InitializeComponent();
            MaximizeBox = false;            

            InitFileCheck();
            InitSetting = InitSettingCheck();
            OpenSetting();



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
            catch (System.IO.FileNotFoundException)
            {
                setting = new Setting();
                SaveSetting();
            }
            
        }
        
        /// <summary>
        /// 設定ファイルに保存
        /// </summary>
        private void SaveSetting()
        {
            try
            {
                using(System.IO.FileStream fs = new System.IO.FileStream(settingFile, System.IO.FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, setting);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 募集一覧の更新
        /// </summary>
        private void SetPlayerListBox()
        {

            Invoke(
                (MethodInvoker)delegate ()
                {
                    playerListBox.DisplayMember = "ScreenName";
                    playerListBox.ValueMember = "ScreenName";

                    playerListBox.DataSource = playerDataList.Values.ToList();
                    if (playerDataList.Count == 0)
                    {
                        setButtonEnabled(false);
                        messageLabel.Text = "誰もいねぇ";
                        createAtLabel.Text = "えいえんはあるよ";
                    }
                    else
                    {
                        setButtonEnabled(true);
                    }
                }
            );


        }

        /// <summary>
        /// 各ボタンの活性・非活性の切り替え
        /// </summary>
        /// <param name="enabled"></param>
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
            // ユーザタイムラインを取得し、募集中のプレイヤーを一覧に追加
            Task task1 = Task.Run(() =>
            {
            playerDataList = new ConcurrentDictionary<string, PlayerData>();

            var limitTime = DateTimeOffset.UtcNow - setting.TimeSpan;
                var res = tokens.Statuses.UserTimeline(user_id => mchanUserId, count => this.count)
                .Where((Status status) => status.CreatedAt > limitTime)
                .OrderBy((Status status) => status.CreatedAt);

                foreach (Status status in res) OperatePlayerList(status);

                
                SetPlayerListBox();
            }
            );

            await task1;

            // 定期的に募集一覧のプレイヤーのcreateAtを確認し、設定した時間を過ぎたものを削除する
            Task task2 = Task.Run(() =>
            {
                while (true)
                {
                    Task.WaitAll(Task.Delay(TimeSpan.FromMinutes(10)));
                    PlayerData removedValue;
                    var limitTime = DateTimeOffset.UtcNow - setting.TimeSpan;
                    var res = from dic in playerDataList
                              where dic.Value.CreateAt < limitTime
                              select dic.Key;

                    foreach (string key in res)
                    {
                        playerDataList.TryRemove(key, out removedValue);
                    }
                    
                    SetPlayerListBox();
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
                .Select((StatusMessage m) => m.Status)
                .Subscribe(
                    (Status status) => {
                        OperatePlayerList(status);
                        SetPlayerListBox();
                    },
                    (Exception ex) => MessageBox.Show(ex.Message),
                    () => MessageBox.Show("終わり")
                 );
            
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
               
            }
            else if (MatchMode.Close == getMatchMode(status.Text))
            {
                PlayerData data;
                playerDataList.TryRemove(status.InReplyToScreenName, out data);
                
                
            }
            
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



        //using System.Runtime.InteropServices;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);


        /// <summary>
        /// EFZRevivalを起動
        /// </summary>
        /// <returns></returns>
        private async Task EfzRun()
        {
            Task task = Task.Run(() =>
            {
                if (efzr == null)
                {
                    efzr = new Process();
                    efzr.StartInfo.FileName = setting.EfzFolderPath + @"\EfzRevival.exe";
                    efzr.StartInfo.WorkingDirectory = setting.EfzFolderPath;
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
            // UPnPを使用してグローバルIP取得
            Task task1 = Task.Run(() =>
            {
                UPnPControlPoint p = new UPnPControlPoint();
                var result = p.GetExternalIPAddress();
                
                setting.IpAddress = result;
            });

            // EfzRevival.iniからポート番号抽出
            Task task2 = Task.Run(() =>
            {
                string result = "";
                string pattern = @"^Port\s*=\s*[0-9]{1,5}";
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None);
                System.Text.RegularExpressions.Match match;

                try
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(
                    setting.EfzFolderPath + @"\EfzRevival.ini",
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
                        setting.Port = match.ToString();
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

        /// <summary>
        /// 設定画面を開く。必ずこのメソッドを通して開くこと。
        /// </summary>
        private void ShowSettingDisplay()
        {
            DialogResult result = new IDManager(settingFile).ShowDialog();
            if (result == DialogResult.OK)
            {
                OpenSetting();
                UserListUpdate();
            }
        }


        /* イベント */


        /// <summary>
        /// クラ専募集ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clientButton_Click(object sender, EventArgs e)
        {
            var sd = (Button)sender;
            var result = new TweetDisplay("", sd.Name, tokens).ShowDialog();
        }

        /// <summary>
        /// 締切ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            var sd = (Button)sender;
            var result = new TweetDisplay("", sd.Name, tokens).ShowDialog();
        }

        /// <summary>
        /// メインフォームが開かれた時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Mchan_Shown(object sender, EventArgs e)
        {
            /*
            if (InitSetting)
            {
                MessageBox.Show("Twitterアカウントを追加し、EfzRevivalフォルダを指定してください","初期設定",MessageBoxButtons.OK);
                ShowSettingDisplay();

            }
            else if (!System.IO.File.Exists(setting.EfzFolderPath + @"\EfzRevival.exe"))
            {
                MessageBox.Show("EfzRevival.exeが見つかりません");
                ShowSettingDisplay();
            }
            else
            {
                UserListUpdate();
            }
            userListPullDown.SelectedIndex = setting.UserListIndex;
            */

            await GetIPAddress();
        }

        /// <summary>
        /// アカウント管理ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void managerButton_Click(object sender, EventArgs e)
        {
            ShowSettingDisplay();
        }

        
        /// <summary>
        /// メインフォームを終了したときのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mchan_FormClosed(object sender, FormClosedEventArgs e)
        {
            int userListIndex = userListPullDown.SelectedIndex;

            setting.UserListIndex = userListIndex;
            SaveSetting();
        }

        /// <summary>
        /// 募集一覧からプレイヤーを選択した時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var player = (PlayerData)playerListBox.SelectedItem;
            messageLabel.Text = player.Message;
            createAtLabel.Text = (player.CreateAt + TimeSpan.FromHours(9)).ToString("G");
            if (player.ScreenName.Equals(tokens.ScreenName))
            {
                setButtonEnabled(false);
            }
            else
            {
                setButtonEnabled(true);
            }
        }

        /// <summary>
        /// 使用アカウントを選択した時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void userListPullDown_TextChanged(object sender, EventArgs e)
        {
            if(!(userListPullDown.SelectedIndex == -1))
            {
                // トークンの作成
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                playerListBox.Focus();
            }
            
        }

        /// <summary>
        /// 返信ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void replyButton_Click(object sender, EventArgs e)
        {
            var sd = (Button)sender;
            new TweetDisplay(playerListBox.SelectedValue.ToString(), sd.Name, tokens).ShowDialog();
        }

        /// <summary>
        /// 募集ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void hostButton_Click(object sender, EventArgs e)
        {
            var sd = (Button)sender;
            var result = new TweetDisplay(
                setting.IpAddress + ":" +
                setting.Port
                , sd.Name
                , tokens).ShowDialog();

            if (DialogResult.OK == result)
            {
                await EfzRun();
                SendKeys.Send("1");
            }

        }

        /// <summary>
        /// IP入力ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void joinManualButton_Click(object sender, EventArgs e)
        {
            string ip = getUserIp(Clipboard.GetText());
            DialogResult result = new IPInputDisplay(ip).ShowDialog();
            if (result == DialogResult.OK)
            {
                await EfzRun();
                SendKeys.Send("3");
            }

        }


        /// <summary>
        /// 乱入ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void joinButton_Click(object sender, EventArgs e)
        {
            var player = (PlayerData)playerListBox.SelectedItem;
            Clipboard.SetDataObject(player.Ip, false);
            await EfzRun();
            SendKeys.Send("3");
        }

        /// <summary>
        /// 観戦ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void spectateButton_Click(object sender, EventArgs e)
        {
            var player = (PlayerData)playerListBox.SelectedItem;
            Clipboard.SetDataObject(player.Ip, false);
            await EfzRun();
            SendKeys.Send("4");
        }

        /// <summary>
        /// オフライン起動押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void offlineButton_Click(object sender, EventArgs e)
        {
            await EfzRun();
            SendKeys.Send("5");

        }

        /// <summary>
        /// コンフィグボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configButton_Click(object sender, EventArgs e)
        {
            Process config = new Process();
            config.StartInfo.FileName = setting.EfzFolderPath + @"\config.exe";
            config.StartInfo.WorkingDirectory = setting.EfzFolderPath;
            config.Start();
        }

        private void Mchan_Load(object sender, EventArgs e)
        {
            if (InitSetting)
            {
                MessageBox.Show("Twitterアカウントを追加し、EfzRevivalフォルダを指定してください", "初期設定", MessageBoxButtons.OK);
                ShowSettingDisplay();

            }
            else if (!System.IO.File.Exists(setting.EfzFolderPath + @"\EfzRevival.exe"))
            {
                MessageBox.Show("EfzRevival.exeが見つかりません");
                ShowSettingDisplay();
            }
            else
            {
                UserListUpdate();
            }
            userListPullDown.SelectedIndex = setting.UserListIndex;
        }
    }

}