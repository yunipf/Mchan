using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mchan
{
    /// <summary>
    /// 募集モード
    /// Host:erevm
    /// Close:ereve
    /// Client:erevc
    /// </summary>
    enum MatchMode
    {
        Host,
        Close,
        Client,
        None
    };

    class PlayerData
    {
        
        private string screenName;
        private string name;
        private string ip;
        private string message;
        private MatchMode mode;
        private DateTimeOffset createAt;

        public PlayerData(string screenName, string name, string ip, string message, MatchMode mode, DateTimeOffset createAt)
        {
            ScreenName = screenName;
            Name = name;
            Ip = ip;
            Message = message;
            Mode = mode;
            CreateAt = createAt;
        }

        public string ScreenName
        {
            get
            {
                return screenName;
            }

            set
            {
                screenName = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Ip
        {
            get
            {
                return ip;
            }

            set
            {
                ip = value;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }

        internal MatchMode Mode
        {
            get
            {
                return mode;
            }

            set
            {
                mode = value;
            }
        }

        public DateTimeOffset CreateAt
        {
            get
            {
                return createAt;
            }

            set
            {
                createAt = value;
            }
        }
    }
}
