using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TOTPAuthenticator
{
    public partial class AccountControl : UserControl
    {
        private Account? _account;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Account? Account
        {
            get { return _account; }
            set
            {
                _account = value;
                UpdateData();
            }
        }

        public event EventHandler? EditClicked;
        public event EventHandler? DeleteClicked;

        public AccountControl()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
            this.Padding = new Padding(10);
            this.Margin = new Padding(5);
            this.Size = new Size(350, 80);
        }

        private void UpdateData()
        {
            if (_account != null)
            {
                nameLabel.Text = string.IsNullOrEmpty(_account.Issuer) ? _account.Name : $"{_account.Issuer} - {_account.Name}";
                customStringLabel.Text = _account.CustomString;
            }
        }

        public void UpdateTotp(string totp, int remainingSeconds)
        {
            totpLabel.Text = totp;
            countdownPanel.Tag = remainingSeconds;
            countdownPanel.Invalidate(); // Redraw the countdown circle
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            EditClicked?.Invoke(this, EventArgs.Empty);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, EventArgs.Empty);
        }

        private void countdownPanel_Paint(object sender, PaintEventArgs e)
        {
            if (countdownPanel.Tag is int remainingSeconds)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                float percentage = remainingSeconds / 30f;
                float sweepAngle = percentage * 360;

                using (Pen pen = new Pen(remainingSeconds <= 5 ? Color.Red : Color.Green, 4))
                {
                    e.Graphics.DrawArc(pen, 2, 2, Math.Min(countdownPanel.Width, countdownPanel.Height) - 5, Math.Min(countdownPanel.Width, countdownPanel.Height) - 5, -90, sweepAngle);
                }

                using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    e.Graphics.DrawString($"{remainingSeconds}s", new Font("Segoe UI", 10, FontStyle.Bold), Brushes.White, new Rectangle(0, 0, countdownPanel.Width, countdownPanel.Height), sf);
                }
            }
        }

        private void totpLabel_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(totpLabel.Text);
            ShowCopyMessage(totpLabel);
        }

        private void customStringLabel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(customStringLabel.Text))
                return;
            Clipboard.SetText(customStringLabel.Text);
            ShowCopyMessage(customStringLabel);
        }

        private async void ShowCopyMessage(Control control)
        {
            var originalText = control.Text;
            control.Text = "已複製!";
            await Task.Delay(1000);
            control.Text = originalText;
        }
    }
}
