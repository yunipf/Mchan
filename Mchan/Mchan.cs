using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mchan
{
    public partial class Mchan : Form
    {
        private Dictionary<int, UserData> userList = null;
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
            userListPullDown.DataSource = userList.Values;
        }


        private void Mchan_Shown(object sender, EventArgs e)
        {
            if (InitSetting)
            {
                new IDManager().ShowDialog();
                
            }
          
            UserListUpdate();           
        }

        private void managerButton_Click(object sender, EventArgs e)
        {
            new IDManager().ShowDialog();
            UserListUpdate();
        }
    }

}