namespace SocketGUI
{
    partial class ClientView
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
            this.sendMessageButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.IPTextbox = new System.Windows.Forms.TextBox();
            this.PortTextbox = new System.Windows.Forms.TextBox();
            this.IPLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.historyTextBox = new System.Windows.Forms.TextBox();
            this.MessageTextbox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sendMessageButton
            // 
            this.sendMessageButton.Enabled = false;
            this.sendMessageButton.Location = new System.Drawing.Point(501, 448);
            this.sendMessageButton.Name = "sendMessageButton";
            this.sendMessageButton.Size = new System.Drawing.Size(75, 23);
            this.sendMessageButton.TabIndex = 0;
            this.sendMessageButton.Text = "Send";
            this.sendMessageButton.UseVisualStyleBackColor = true;
            this.sendMessageButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(93, 88);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // IPTextbox
            // 
            this.IPTextbox.Location = new System.Drawing.Point(63, 23);
            this.IPTextbox.Name = "IPTextbox";
            this.IPTextbox.Size = new System.Drawing.Size(105, 21);
            this.IPTextbox.TabIndex = 2;
            this.IPTextbox.Text = "172.16.42.78";
            this.IPTextbox.TextChanged += new System.EventHandler(this.IPTextbox_TextChanged);
            // 
            // PortTextbox
            // 
            this.PortTextbox.Location = new System.Drawing.Point(63, 48);
            this.PortTextbox.Name = "PortTextbox";
            this.PortTextbox.Size = new System.Drawing.Size(105, 21);
            this.PortTextbox.TabIndex = 3;
            this.PortTextbox.Text = "4510";
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(8, 26);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(23, 12);
            this.IPLabel.TabIndex = 4;
            this.IPLabel.Text = "IP:";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(8, 54);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(35, 12);
            this.portLabel.TabIndex = 5;
            this.portLabel.Text = "Port:";
            // 
            // historyTextBox
            // 
            this.historyTextBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.historyTextBox.Location = new System.Drawing.Point(174, 12);
            this.historyTextBox.Multiline = true;
            this.historyTextBox.Name = "historyTextBox";
            this.historyTextBox.ReadOnly = true;
            this.historyTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.historyTextBox.Size = new System.Drawing.Size(402, 361);
            this.historyTextBox.TabIndex = 6;
            this.historyTextBox.TextChanged += new System.EventHandler(this.historyTextBox_TextChanged);
            // 
            // MessageTextbox
            // 
            this.MessageTextbox.Location = new System.Drawing.Point(15, 394);
            this.MessageTextbox.Multiline = true;
            this.MessageTextbox.Name = "MessageTextbox";
            this.MessageTextbox.Size = new System.Drawing.Size(450, 77);
            this.MessageTextbox.TabIndex = 7;
            this.MessageTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MessageTextbox_KeyDown);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(12, 88);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 9;
            this.connectButton.Text = "connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // ClientView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(588, 486);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.MessageTextbox);
            this.Controls.Add(this.historyTextBox);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.IPLabel);
            this.Controls.Add(this.PortTextbox);
            this.Controls.Add(this.IPTextbox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.sendMessageButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ClientView";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button sendMessageButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.TextBox IPTextbox;
        private System.Windows.Forms.TextBox PortTextbox;
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox historyTextBox;
        private System.Windows.Forms.TextBox MessageTextbox;
        private System.Windows.Forms.Button connectButton;
    }
}

