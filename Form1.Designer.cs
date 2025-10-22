
namespace TOTPAuthenticator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFromQRCodeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFromGoogleAuthenticatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addManuallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAccountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(400, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFromQRCodeFileToolStripMenuItem,
            this.addFromGoogleAuthenticatorToolStripMenuItem,
            this.addManuallyToolStripMenuItem,
            this.exportAccountsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.fileToolStripMenuItem.Text = "檔案";
            // 
            // addFromQRCodeFileToolStripMenuItem
            //
            this.addFromQRCodeFileToolStripMenuItem.Name = "addFromQRCodeFileToolStripMenuItem";
            this.addFromQRCodeFileToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.addFromQRCodeFileToolStripMenuItem.Text = "從 QR Code 圖檔新增";
            this.addFromQRCodeFileToolStripMenuItem.Click += new System.EventHandler(this.addFromQRCodeFileToolStripMenuItem_Click);
            //
            // addFromGoogleAuthenticatorToolStripMenuItem
            //
            this.addFromGoogleAuthenticatorToolStripMenuItem.Name = "addFromGoogleAuthenticatorToolStripMenuItem";
            this.addFromGoogleAuthenticatorToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.addFromGoogleAuthenticatorToolStripMenuItem.Text = "從 Google 驗證器圖片匯入";
            this.addFromGoogleAuthenticatorToolStripMenuItem.Click += new System.EventHandler(this.addFromGoogleAuthenticatorToolStripMenuItem_Click);
            //
            // addManuallyToolStripMenuItem
            //
            this.addManuallyToolStripMenuItem.Name = "addManuallyToolStripMenuItem";
            this.addManuallyToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.addManuallyToolStripMenuItem.Text = "手動輸入金鑰新增";
            this.addManuallyToolStripMenuItem.Click += new System.EventHandler(this.addManuallyToolStripMenuItem_Click);
            //
            // exportAccountsToolStripMenuItem
            //
            this.exportAccountsToolStripMenuItem.Name = "exportAccountsToolStripMenuItem";
            this.exportAccountsToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.exportAccountsToolStripMenuItem.Text = "開啟設定檔資料夾";
            this.exportAccountsToolStripMenuItem.Click += new System.EventHandler(this.openAccountsFolderToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 60);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(376, 529);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(12, 31);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(290, 23);
            this.searchTextBox.TabIndex = 2;
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(308, 31);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 23);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "+ 新增";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(400, 601);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form";
            this.Text = "TOTPAuthenticator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem addFromQRCodeFileToolStripMenuItem;
        private ToolStripMenuItem addFromGoogleAuthenticatorToolStripMenuItem;
        private ToolStripMenuItem addManuallyToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private FlowLayoutPanel flowLayoutPanel1;
        private TextBox searchTextBox;
        private Button addButton;
        private ToolStripMenuItem exportAccountsToolStripMenuItem;
    }
}
