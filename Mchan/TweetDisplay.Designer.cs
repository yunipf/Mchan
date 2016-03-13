namespace Mchan
{
    partial class TweetDisplay
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
            this.tweetTextBox = new System.Windows.Forms.TextBox();
            this.tweetButton = new System.Windows.Forms.Button();
            this.lengthLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tweetTextBox
            // 
            this.tweetTextBox.BackColor = System.Drawing.Color.DimGray;
            this.tweetTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tweetTextBox.ForeColor = System.Drawing.Color.White;
            this.tweetTextBox.Location = new System.Drawing.Point(13, 12);
            this.tweetTextBox.Multiline = true;
            this.tweetTextBox.Name = "tweetTextBox";
            this.tweetTextBox.Size = new System.Drawing.Size(271, 161);
            this.tweetTextBox.TabIndex = 0;
            this.tweetTextBox.TextChanged += new System.EventHandler(this.tweetTextBox_TextChanged);
            // 
            // tweetButton
            // 
            this.tweetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tweetButton.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tweetButton.Location = new System.Drawing.Point(201, 191);
            this.tweetButton.Name = "tweetButton";
            this.tweetButton.Size = new System.Drawing.Size(83, 27);
            this.tweetButton.TabIndex = 1;
            this.tweetButton.Text = "ツイート";
            this.tweetButton.UseVisualStyleBackColor = true;
            this.tweetButton.Click += new System.EventHandler(this.tweetButton_Click);
            // 
            // lengthLabel
            // 
            this.lengthLabel.Location = new System.Drawing.Point(12, 191);
            this.lengthLabel.Name = "lengthLabel";
            this.lengthLabel.Size = new System.Drawing.Size(62, 27);
            this.lengthLabel.TabIndex = 2;
            this.lengthLabel.Text = "label1";
            // 
            // TweetDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(296, 232);
            this.Controls.Add(this.lengthLabel);
            this.Controls.Add(this.tweetButton);
            this.Controls.Add(this.tweetTextBox);
            this.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TweetDisplay";
            this.Text = "TweetDisplay";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tweetTextBox;
        private System.Windows.Forms.Button tweetButton;
        private System.Windows.Forms.Label lengthLabel;
    }
}