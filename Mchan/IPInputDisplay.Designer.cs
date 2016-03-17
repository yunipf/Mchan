namespace Mchan
{
    partial class IPInputDisplay
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.joinButton = new System.Windows.Forms.Button();
            this.ipInputBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.spectateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(14, 80);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(78, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "キャンセル";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // joinButton
            // 
            this.joinButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.joinButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.joinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.joinButton.Location = new System.Drawing.Point(126, 80);
            this.joinButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(48, 23);
            this.joinButton.TabIndex = 1;
            this.joinButton.Text = "乱入";
            this.joinButton.UseVisualStyleBackColor = false;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // ipInputBox
            // 
            this.ipInputBox.BackColor = System.Drawing.Color.DimGray;
            this.ipInputBox.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ipInputBox.ForeColor = System.Drawing.Color.White;
            this.ipInputBox.Location = new System.Drawing.Point(12, 43);
            this.ipInputBox.Name = "ipInputBox";
            this.ipInputBox.Size = new System.Drawing.Size(219, 22);
            this.ipInputBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP:Port";
            // 
            // spectateButton
            // 
            this.spectateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.spectateButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.spectateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.spectateButton.Location = new System.Drawing.Point(182, 80);
            this.spectateButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.spectateButton.Name = "spectateButton";
            this.spectateButton.Size = new System.Drawing.Size(48, 23);
            this.spectateButton.TabIndex = 4;
            this.spectateButton.Text = "観戦";
            this.spectateButton.UseVisualStyleBackColor = false;
            this.spectateButton.Click += new System.EventHandler(this.spectateButton_Click);
            // 
            // IPInputDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(243, 120);
            this.Controls.Add(this.spectateButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ipInputBox);
            this.Controls.Add(this.joinButton);
            this.Controls.Add(this.cancelButton);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IPInputDisplay";
            this.Text = "IPを入力して接続";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.TextBox ipInputBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button spectateButton;
    }
}