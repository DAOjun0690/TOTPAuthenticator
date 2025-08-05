
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
            this.addManuallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsListBox = new System.Windows.Forms.ListBox();
            this.totpLabel = new System.Windows.Forms.Label();
            this.countdownLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.copyButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFromQRCodeFileToolStripMenuItem,
            this.addManuallyToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addFromQRCodeFileToolStripMenuItem
            // 
            this.addFromQRCodeFileToolStripMenuItem.Name = "addFromQRCodeFileToolStripMenuItem";
            this.addFromQRCodeFileToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.addFromQRCodeFileToolStripMenuItem.Text = "Add from QR Code File";
            this.addFromQRCodeFileToolStripMenuItem.Click += new System.EventHandler(this.addFromQRCodeFileToolStripMenuItem_Click);
            // 
            // addManuallyToolStripMenuItem
            // 
            this.addManuallyToolStripMenuItem.Name = "addManuallyToolStripMenuItem";
            this.addManuallyToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.addManuallyToolStripMenuItem.Text = "Add Manually";
            this.addManuallyToolStripMenuItem.Click += new System.EventHandler(this.addManuallyToolStripMenuItem_Click);
            // 
            // accountsListBox
            // 
            this.accountsListBox.FormattingEnabled = true;
            this.accountsListBox.ItemHeight = 15;
            this.accountsListBox.Location = new System.Drawing.Point(12, 27);
            this.accountsListBox.Name = "accountsListBox";
            this.accountsListBox.Size = new System.Drawing.Size(776, 199);
            this.accountsListBox.TabIndex = 1;
            this.accountsListBox.SelectedIndexChanged += new System.EventHandler(this.accountsListBox_SelectedIndexChanged);
            // 
            // totpLabel
            // 
            this.totpLabel.AutoSize = true;
            this.totpLabel.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.totpLabel.Location = new System.Drawing.Point(333, 252);
            this.totpLabel.Name = "totpLabel";
            this.totpLabel.Size = new System.Drawing.Size(111, 45);
            this.totpLabel.TabIndex = 2;
            this.totpLabel.Text = "000000";
            // 
            // countdownLabel
            // 
            this.countdownLabel.AutoSize = true;
            this.countdownLabel.Location = new System.Drawing.Point(383, 309);
            this.countdownLabel.Name = "countdownLabel";
            this.countdownLabel.Size = new System.Drawing.Size(19, 15);
            this.countdownLabel.TabIndex = 3;
            this.countdownLabel.Text = "30";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // copyButton
            // 
            this.copyButton.Location = new System.Drawing.Point(450, 268);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(75, 23);
            this.copyButton.TabIndex = 4;
            this.copyButton.Text = "Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.countdownLabel);
            this.Controls.Add(this.totpLabel);
            this.Controls.Add(this.accountsListBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "TOTP Authenticator";
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
        private ToolStripMenuItem addManuallyToolStripMenuItem;
        private ListBox accountsListBox;
        private Label totpLabel;
        private Label countdownLabel;
        private System.Windows.Forms.Timer timer1;
        private Button copyButton;
    }
}
