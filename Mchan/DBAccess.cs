using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Mchan
{
    class DBAccess
    {
        private static string fileName = "data.sqlite";
        private static Dictionary<int, UserData> userList = null;

        public static string FileName
        {
            get
            {
                return fileName;
            }
        }

        internal static Dictionary<int, UserData> UserList
        {
            get
            {
                return userList;
            }

            set
            {
                userList = value;
            }
        }

        

        public static void DBInit()
        {

            using (var conn = new SQLiteConnection("Data Source=" + fileName))
            {
                conn.Open();
                using (SQLiteCommand command = conn.CreateCommand())
                {
                    command.CommandText = "create table user(dbId INTEGER PRIMARY KEY AUTOINCREMENT,id TEXT,screenName TEXT,name TEXT,accessToken TEXT,accessTokenSecret TEXT)";
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public static int DBSelectAll()
        {
            userList = new Dictionary<int, UserData>();
            int rows = 0;
            using (var conn = new SQLiteConnection("Data Source=" + fileName))
            {
                conn.Open();
                using (SQLiteCommand command = conn.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM user";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int dbId = Convert.ToInt32(reader["dbId"].ToString());
                            string id = reader["id"].ToString();
                            string screenName = reader["screenName"].ToString();
                            string name = reader["name"].ToString();
                            string accessToken = reader["accessToken"].ToString();
                            string accessTokenSecret = reader["accessTokenSecret"].ToString();

                            UserData userData = new UserData(id, screenName, name, accessToken, accessTokenSecret);
                            userList.Add(dbId, userData);
                            rows++;
                        }
                    }
                }
                conn.Close();
            }

            return rows;
        }

        public static void DBInsert(UserData userData)
        {
            using (var conn = new SQLiteConnection("Data Source=" + fileName))
            {
                conn.Open();
                using (SQLiteTransaction sqlt = conn.BeginTransaction())
                {
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "insert into user (id,screenName,name,accessToken,accessTokenSecret) values('" + userData.Id + "','" + userData.ScreenName + "','" + userData.Name + "','" + userData.AccessToken + "','" + userData.AccessTokenSecret + "')";
                        command.ExecuteNonQuery();                      
                    }
                    sqlt.Commit();
                }
                conn.Close();
            }
            DBSelectAll();
        }

        public static void DBUpdate(UserData userData)
        {
            using (var conn = new SQLiteConnection("Data Source=" + fileName))
            {
                conn.Open();
                using (SQLiteTransaction sqlt = conn.BeginTransaction())
                {
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "update user set id = '" + userData.Id + "',screenName = '" + userData.ScreenName + "',name = '" + userData.Name + "',accessToken = '" + userData.AccessToken + "',accessTokenSecret = '" + userData.AccessTokenSecret + "' where id = '" + userData.Id + "'";
                        command.ExecuteNonQuery();
                    }
                    sqlt.Commit();
                }
                conn.Close();
            }
            DBSelectAll();
        }

        public static void DBDelete(UserData userData)
        {
            using (var conn = new SQLiteConnection("Data Source=" + fileName))
            {
                conn.Open();
                using (SQLiteTransaction sqlt = conn.BeginTransaction())
                {
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "delete from user where id = '" + userData.Id + "'";
                        command.ExecuteNonQuery();
                    }
                    sqlt.Commit();
                }
                conn.Close();
            }
            DBSelectAll();
        }

    }
}
