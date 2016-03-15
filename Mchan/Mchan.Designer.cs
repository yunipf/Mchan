namespace Mchan
{
    partial class Mchan
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.userListPullDown = new System.Windows.Forms.ComboBox();
            this.managerButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.Label();
            this.hostButton = new System.Windows.Forms.Button();
            this.replyButton = new System.Windows.Forms.Button();
            this.spectateButton = new System.Windows.Forms.Button();
            this.joinButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.createAtLabel = new System.Windows.Forms.Label();
            this.playerListBox = new System.Windows.Forms.ListBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.joinManualButton = new System.Windows.Forms.Button();
            this.clientButton = new System.Windows.Forms.Button();
            this.offlineButton = new System.Windows.Forms.Button();
            this.configButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // userListPullDown
            // 
            this.userListPullDown.BackColor = System.Drawing.Color.DimGray;
            this.userListPullDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userListPullDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.userListPullDown.ForeColor = System.Drawing.Color.White;
            this.userListPullDown.FormattingEnabled = true;
            this.userListPullDown.Location = new System.Drawing.Point(75, 10);
            this.userListPullDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.userListPullDown.Name = "userListPullDown";
            this.userListPullDown.Size = new System.Drawing.Size(136, 20);
            this.userListPullDown.TabIndex = 6;
            this.userListPullDown.TextChanged += new System.EventHandler(this.userListPullDown_TextChanged);
            // 
            // managerButton
            // 
            this.managerButton.BackColor = System.Drawing.Color.DimGray;
            this.managerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.managerButton.Location = new System.Drawing.Point(219, 8);
            this.managerButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.managerButton.Name = "managerButton";
            this.managerButton.Size = new System.Drawing.Size(54, 23);
            this.managerButton.TabIndex = 7;
            this.managerButton.Text = "設定";
            this.managerButton.UseVisualStyleBackColor = false;
            this.managerButton.Click += new System.EventHandler(this.managerButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "アカウント";
            // 
            // messageLabel
            // 
            this.messageLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.messageLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.messageLabel.ForeColor = System.Drawing.Color.White;
            this.messageLabel.Location = new System.Drawing.Point(7, 25);
            this.messageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(204, 175);
            this.messageLabel.TabIndex = 8;
            // 
            // hostButton
            // 
            this.hostButton.BackColor = System.Drawing.Color.DimGray;
            this.hostButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hostButton.Location = new System.Drawing.Point(7, 197);
            this.hostButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.hostButton.Name = "hostButton";
            this.hostButton.Size = new System.Drawing.Size(54, 23);
            this.hostButton.TabIndex = 5;
            this.hostButton.Text = "募集";
            this.hostButton.UseVisualStyleBackColor = false;
            this.hostButton.Click += new System.EventHandler(this.hostButton_Click);
            // 
            // replyButton
            // 
            this.replyButton.BackColor = System.Drawing.Color.DimGray;
            this.replyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.replyButton.Location = new System.Drawing.Point(7, 203);
            this.replyButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.replyButton.Name = "replyButton";
            this.replyButton.Size = new System.Drawing.Size(42, 25);
            this.replyButton.TabIndex = 4;
            this.replyButton.Text = "返信";
            this.replyButton.UseVisualStyleBackColor = false;
            this.replyButton.Click += new System.EventHandler(this.replyButton_Click);
            // 
            // spectateButton
            // 
            this.spectateButton.BackColor = System.Drawing.Color.DimGray;
            this.spectateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.spectateButton.Location = new System.Drawing.Point(69, 157);
            this.spectateButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.spectateButton.Name = "spectateButton";
            this.spectateButton.Size = new System.Drawing.Size(54, 23);
            this.spectateButton.TabIndex = 3;
            this.spectateButton.Text = "観戦";
            this.spectateButton.UseVisualStyleBackColor = false;
            this.spectateButton.Click += new System.EventHandler(this.spectateButton_Click);
            // 
            // joinButton
            // 
            this.joinButton.BackColor = System.Drawing.Color.DimGray;
            this.joinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.joinButton.Location = new System.Drawing.Point(7, 157);
            this.joinButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(54, 23);
            this.joinButton.TabIndex = 2;
            this.joinButton.Text = "乱入";
            this.joinButton.UseVisualStyleBackColor = false;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.createAtLabel);
            this.groupBox1.Controls.Add(this.messageLabel);
            this.groupBox1.Controls.Add(this.replyButton);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(249, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 234);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "コメント";
            // 
            // createAtLabel
            // 
            this.createAtLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.createAtLabel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.createAtLabel.ForeColor = System.Drawing.Color.White;
            this.createAtLabel.Location = new System.Drawing.Point(57, 204);
            this.createAtLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.createAtLabel.Name = "createAtLabel";
            this.createAtLabel.Size = new System.Drawing.Size(155, 27);
            this.createAtLabel.TabIndex = 13;
            this.createAtLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // playerListBox
            // 
            this.playerListBox.BackColor = System.Drawing.Color.DimGray;
            this.playerListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playerListBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.playerListBox.ForeColor = System.Drawing.Color.White;
            this.playerListBox.FormattingEnabled = true;
            this.playerListBox.ItemHeight = 16;
            this.playerListBox.Location = new System.Drawing.Point(7, 18);
            this.playerListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.playerListBox.Name = "playerListBox";
            this.playerListBox.Size = new System.Drawing.Size(212, 114);
            this.playerListBox.TabIndex = 1;
            this.playerListBox.SelectedIndexChanged += new System.EventHandler(this.playerList_SelectedIndexChanged);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.DimGray;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Location = new System.Drawing.Point(69, 197);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(54, 23);
            this.closeButton.TabIndex = 11;
            this.closeButton.Text = "締切";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.joinManualButton);
            this.groupBox2.Controls.Add(this.clientButton);
            this.groupBox2.Controls.Add(this.playerListBox);
            this.groupBox2.Controls.Add(this.closeButton);
            this.groupBox2.Controls.Add(this.hostButton);
            this.groupBox2.Controls.Add(this.spectateButton);
            this.groupBox2.Controls.Add(this.joinButton);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 51);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 234);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "募集一覧";
            // 
            // joinManualButton
            // 
            this.joinManualButton.BackColor = System.Drawing.Color.DimGray;
            this.joinManualButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.joinManualButton.Location = new System.Drawing.Point(145, 157);
            this.joinManualButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.joinManualButton.Name = "joinManualButton";
            this.joinManualButton.Size = new System.Drawing.Size(74, 23);
            this.joinManualButton.TabIndex = 13;
            this.joinManualButton.Text = "IP入力";
            this.joinManualButton.UseVisualStyleBackColor = false;
            this.joinManualButton.Click += new System.EventHandler(this.joinManualButton_Click);
            // 
            // clientButton
            // 
            this.clientButton.BackColor = System.Drawing.Color.DimGray;
            this.clientButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clientButton.Location = new System.Drawing.Point(145, 197);
            this.clientButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.clientButton.Name = "clientButton";
            this.clientButton.Size = new System.Drawing.Size(74, 23);
            this.clientButton.TabIndex = 12;
            this.clientButton.Text = "クラ専募集";
            this.clientButton.UseVisualStyleBackColor = false;
            this.clientButton.Click += new System.EventHandler(this.clientButton_Click);
            // 
            // offlineButton
            // 
            this.offlineButton.BackColor = System.Drawing.Color.DimGray;
            this.offlineButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.offlineButton.Location = new System.Drawing.Point(374, 8);
            this.offlineButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.offlineButton.Name = "offlineButton";
            this.offlineButton.Size = new System.Drawing.Size(94, 23);
            this.offlineButton.TabIndex = 13;
            this.offlineButton.Text = "オフライン起動";
            this.offlineButton.UseVisualStyleBackColor = false;
            this.offlineButton.Click += new System.EventHandler(this.offlineButton_Click);
            // 
            // configButton
            // 
            this.configButton.BackColor = System.Drawing.Color.DimGray;
            this.configButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.configButton.Location = new System.Drawing.Point(298, 8);
            this.configButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.configButton.Name = "configButton";
            this.configButton.Size = new System.Drawing.Size(68, 23);
            this.configButton.TabIndex = 14;
            this.configButton.Text = "コンフィグ";
            this.configButton.UseVisualStyleBackColor = false;
            this.configButton.Click += new System.EventHandler(this.configButton_Click);
            // 
            // Mchan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(480, 297);
            this.Controls.Add(this.configButton);
            this.Controls.Add(this.offlineButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.managerButton);
            this.Controls.Add(this.userListPullDown);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Mchan";
            this.Text = "Mchan v0.1b";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Mchan_FormClosed);
            this.Shown += new System.EventHandler(this.Mchan_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox userListPullDown;
        private System.Windows.Forms.Button managerButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Button hostButton;
        private System.Windows.Forms.Button replyButton;
        private System.Windows.Forms.Button spectateButton;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox playerListBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label createAtLabel;
        private System.Windows.Forms.Button offlineButton;
        private System.Windows.Forms.Button joinManualButton;
        private System.Windows.Forms.Button clientButton;
        private System.Windows.Forms.Button configButton;
    }
}

