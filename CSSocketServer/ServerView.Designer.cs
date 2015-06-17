namespace SocketGUI
{
    partial class ServerView
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listenButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.IPTextbox = new System.Windows.Forms.TextBox();
            this.PortTextbox = new System.Windows.Forms.TextBox();
            this.IPLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.MessageTextbox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.historyTextBox = new System.Windows.Forms.RichTextBox();
            this.onlineListLabel = new System.Windows.Forms.Label();
            this.onlineClientListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listenButton
            // 
            this.listenButton.Location = new System.Drawing.Point(10, 105);
            this.listenButton.Name = "listenButton";
            this.listenButton.Size = new System.Drawing.Size(75, 23);
            this.listenButton.TabIndex = 0;
            this.listenButton.Text = "Listen";
            this.listenButton.UseVisualStyleBackColor = true;
            this.listenButton.Click += new System.EventHandler(this.listenButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(143, 105);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // IPTextbox
            // 
            this.IPTextbox.Enabled = false;
            this.IPTextbox.Location = new System.Drawing.Point(85, 23);
            this.IPTextbox.Name = "IPTextbox";
            this.IPTextbox.Size = new System.Drawing.Size(133, 21);
            this.IPTextbox.TabIndex = 2;
            // 
            // PortTextbox
            // 
            this.PortTextbox.Enabled = false;
            this.PortTextbox.Location = new System.Drawing.Point(85, 67);
            this.PortTextbox.Name = "PortTextbox";
            this.PortTextbox.Size = new System.Drawing.Size(133, 21);
            this.PortTextbox.TabIndex = 3;
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(8, 26);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(59, 12);
            this.IPLabel.TabIndex = 4;
            this.IPLabel.Text = "Local IP:";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(8, 70);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(71, 12);
            this.portLabel.TabIndex = 5;
            this.portLabel.Text = "Local Port:";
            // 
            // MessageTextbox
            // 
            this.MessageTextbox.Location = new System.Drawing.Point(15, 394);
            this.MessageTextbox.Multiline = true;
            this.MessageTextbox.Name = "MessageTextbox";
            this.MessageTextbox.Size = new System.Drawing.Size(450, 77);
            this.MessageTextbox.TabIndex = 7;
            this.MessageTextbox.TextChanged += new System.EventHandler(this.MessageTextbox_TextChanged);
            this.MessageTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MessageTextbox_KeyDown);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(471, 448);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 9;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // historyTextBox
            // 
            this.historyTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.historyTextBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.historyTextBox.Location = new System.Drawing.Point(231, 20);
            this.historyTextBox.Name = "historyTextBox";
            this.historyTextBox.ReadOnly = true;
            this.historyTextBox.Size = new System.Drawing.Size(315, 335);
            this.historyTextBox.TabIndex = 10;
            this.historyTextBox.Text = "";
            this.historyTextBox.TextChanged += new System.EventHandler(this.historyTextBox_TextChanged);
            // 
            // onlineListLabel
            // 
            this.onlineListLabel.AutoSize = true;
            this.onlineListLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.onlineListLabel.Location = new System.Drawing.Point(12, 151);
            this.onlineListLabel.Name = "onlineListLabel";
            this.onlineListLabel.Size = new System.Drawing.Size(77, 17);
            this.onlineListLabel.TabIndex = 12;
            this.onlineListLabel.Text = "online client";
            // 
            // onlineClientListBox
            // 
            this.onlineClientListBox.FormattingEnabled = true;
            this.onlineClientListBox.ItemHeight = 12;
            this.onlineClientListBox.Location = new System.Drawing.Point(15, 171);
            this.onlineClientListBox.Name = "onlineClientListBox";
            this.onlineClientListBox.Size = new System.Drawing.Size(203, 184);
            this.onlineClientListBox.TabIndex = 13;
            this.onlineClientListBox.SelectedIndexChanged += new System.EventHandler(this.onlineClientListBox_SelectedIndexChanged);
            // 
            // ServerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(560, 490);
            this.Controls.Add(this.onlineClientListBox);
            this.Controls.Add(this.onlineListLabel);
            this.Controls.Add(this.historyTextBox);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.MessageTextbox);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.IPLabel);
            this.Controls.Add(this.PortTextbox);
            this.Controls.Add(this.IPTextbox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.listenButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ServerView";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button listenButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.TextBox IPTextbox;
        private System.Windows.Forms.TextBox PortTextbox;
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox MessageTextbox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.RichTextBox historyTextBox;
        private System.Windows.Forms.Label onlineListLabel;
        private System.Windows.Forms.ListBox onlineClientListBox;
    }
}

