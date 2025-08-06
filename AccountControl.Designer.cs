namespace TOTPAuthenticator
{
    partial class AccountControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.nameLabel = new System.Windows.Forms.Label();
            this.totpLabel = new System.Windows.Forms.Label();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.countdownPanel = new System.Windows.Forms.Panel();
            this.customStringLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nameLabel.Location = new System.Drawing.Point(63, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(202, 35);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Account Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // totpLabel
            // 
            this.totpLabel.AutoSize = true;
            this.totpLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totpLabel.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.totpLabel.Location = new System.Drawing.Point(63, 35);
            this.totpLabel.Name = "totpLabel";
            this.totpLabel.Size = new System.Drawing.Size(202, 45);
            this.totpLabel.TabIndex = 1;
            this.totpLabel.Text = "000000";
            this.totpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.totpLabel.Click += new System.EventHandler(this.totpLabel_Click);
            // 
            // editButton
            // 
            this.editButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.editButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editButton.FlatAppearance.BorderSize = 0;
            this.editButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editButton.Location = new System.Drawing.Point(271, 3);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(74, 29);
            this.editButton.TabIndex = 2;
            this.editButton.Text = "編輯";
            this.editButton.UseVisualStyleBackColor = false;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.deleteButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deleteButton.FlatAppearance.BorderSize = 0;
            this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButton.Location = new System.Drawing.Point(271, 38);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(74, 39);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "刪除";
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // countdownPanel
            // 
            this.countdownPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.countdownPanel.Location = new System.Drawing.Point(3, 3);
            this.countdownPanel.Name = "countdownPanel";
            this.tableLayoutPanel1.SetRowSpan(this.countdownPanel, 2);
            this.countdownPanel.Size = new System.Drawing.Size(54, 74);
            this.countdownPanel.TabIndex = 4;
            this.countdownPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.countdownPanel_Paint);
            // 
            // customStringLabel
            // 
            this.customStringLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.customStringLabel, 3);
            this.customStringLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customStringLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.customStringLabel.Location = new System.Drawing.Point(3, 80);
            this.customStringLabel.Name = "customStringLabel";
            this.customStringLabel.Padding = new System.Windows.Forms.Padding(5);
            this.customStringLabel.Size = new System.Drawing.Size(342, 31);
            this.customStringLabel.TabIndex = 5;
            this.customStringLabel.Text = "Custom String";
            this.customStringLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.customStringLabel.Click += new System.EventHandler(this.customStringLabel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Controls.Add(this.countdownPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.nameLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.editButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.totpLabel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.deleteButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.customStringLabel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(350, 111);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // AccountControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.MinimumSize = new System.Drawing.Size(350, 0);
            this.Name = "AccountControl";
            this.Size = new System.Drawing.Size(350, 111);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label totpLabel;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Panel countdownPanel;
        private System.Windows.Forms.Label customStringLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
