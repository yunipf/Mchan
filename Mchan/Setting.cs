using System;


namespace Mchan
{
    /// <summary>
    /// 設定クラス
    /// </summary>
    [Serializable]
    class Setting
    {
        // 設定項目
        private TimeSpan timeSpan;
        private string efzFolderPath;

        // ユーザ情報
        private int userListIndex;
        private string ipAddress;
        private string port;

        public Setting()
        {
            TimeSpan = TimeSpan.FromMinutes(180);
            EfzFolderPath = @"..\";
            UserListIndex = 0;
            IpAddress = "";
            Port = "";
        }

        public TimeSpan TimeSpan
        {
            get
            {
                return timeSpan;
            }

            set
            {
                timeSpan = value;
            }
        }

        public string EfzFolderPath
        {
            get
            {
                return efzFolderPath;
            }

            set
            {
                efzFolderPath = value;
            }
        }

        public int UserListIndex
        {
            get
            {
                return userListIndex;
            }

            set
            {
                userListIndex = value;
            }
        }

        public string IpAddress
        {
            get
            {
                return ipAddress;
            }

            set
            {
                ipAddress = value;
            }
        }

        public string Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
            }
        }
    }
}
