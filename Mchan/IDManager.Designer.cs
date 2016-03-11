namespace Mchan
{
    partial class IDManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.userListBox = new System.Windows.Forms.ListBox();
            this.addUserButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.userListBox);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(10, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 183);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "アカウント一覧";
            // 
            // userListBox
            // 
            this.userListBox.BackColor = System.Drawing.Color.DimGray;
            this.userListBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.userListBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.userListBox.FormattingEnabled = true;
            this.userListBox.ItemHeight = 16;
            this.userListBox.Location = new System.Drawing.Point(6, 18);
            this.userListBox.Margin = new System.Windows.Forms.Padding(2);
            this.userListBox.Name = "userListBox";
            this.userListBox.Size = new System.Drawing.Size(203, 148);
            this.userListBox.TabIndex = 0;
            // 
            // addUserButton
            // 
            this.addUserButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addUserButton.Location = new System.Drawing.Point(126, 239);
            this.addUserButton.Name = "addUserButton";
            this.addUserButton.Size = new System.Drawing.Size(99, 23);
            this.addUserButton.TabIndex = 1;
            this.addUserButton.Text = "アカウント追加";
            this.addUserButton.UseVisualStyleBackColor = true;
            this.addUserButton.Click += new System.EventHandler(this.addUserButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButton.Location = new System.Drawing.Point(165, 200);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(2);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(60, 23);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "削除";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // IDManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(235, 279);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addUserButton);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "IDManager";
            this.Text = "アカウント管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IDManager_FormClosing);
            this.Load += new System.EventHandler(this.IDManager_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox userListBox;
        private System.Windows.Forms.Button addUserButton;
        private System.Windows.Forms.Button deleteButton;
    }
}