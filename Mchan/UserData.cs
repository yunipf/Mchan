using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mchan
{
    class UserData
    {
        private string id;
        private string screenName;
        private string name;
        private string accessToken;
        private string accessTokenSecret;

        public UserData(string id, string screenName, string name, string accessToken, string accessTokenSecret)
        {
            Id = id;
            ScreenName = screenName;
            Name = name;
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
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

        public string AccessToken
        {
            get
            {
                return accessToken;
            }

            set
            {
                accessToken = value;
            }
        }

        public string AccessTokenSecret
        {
            get
            {
                return accessTokenSecret;
            }

            set
            {
                accessTokenSecret = value;
            }
        }

        
    }
}
