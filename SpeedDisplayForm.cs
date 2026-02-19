using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MNetSpeedMeter
{
    public partial class SpeedDisplayForm : Form
    {
        // Window ko click-through banane ke liye
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        const int GWL_EXSTYLE = -20;
        const int WS_EX_LAYERED = 0x80000;
        const int WS_EX_TRANSPARENT = 0x20;
        const int WS_EX_TOOLWINDOW = 0x80;

        private Label lblDownload;
        private Label lblUpload;

        public SpeedDisplayForm()
        {
            InitializeComponent();
            SetupForm();
            SetupLabels();
        }

        private void SetupForm()
        {
            // Form properties
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Opacity = 0.9;
            this.Size = new Size(180, 40);

            // Position - taskbar ke upar right corner
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(
                workingArea.Right - this.Width - 10,
                workingArea.Bottom - this.Height - 5
            );

            // Window style set karo
            this.Load += (s, e) =>
            {
                // Tool window - Alt+Tab mein nahi dikhega
                int exStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
                SetWindowLong(this.Handle, GWL_EXSTYLE, exStyle | WS_EX_TOOLWINDOW);
            };

            // Double click pe hide karo
            this.DoubleClick += (s, e) => this.Hide();

            // Drag karne ke liye
            this.MouseDown += Form_MouseDown;
        }

        private void SetupLabels()
        {
            // Download label
            lblDownload = new Label();
            lblDownload.Text = "↓ 0 B/s";
            lblDownload.ForeColor = Color.LimeGreen;
            lblDownload.BackColor = Color.Transparent;
            lblDownload.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblDownload.Location = new Point(5, 2);
            lblDownload.Size = new Size(170, 18);
            lblDownload.MouseDown += Form_MouseDown;
            this.Controls.Add(lblDownload);

            // Upload label
            lblUpload = new Label();
            lblUpload.Text = "↑ 0 B/s";
            lblUpload.ForeColor = Color.Orange;
            lblUpload.BackColor = Color.Transparent;
            lblUpload.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblUpload.Location = new Point(5, 20);
            lblUpload.Size = new Size(170, 18);
            lblUpload.MouseDown += Form_MouseDown;
            this.Controls.Add(lblUpload);
        }

        // Drag functionality
        private Point dragOffset;
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragOffset = new Point(e.X, e.Y);
                this.MouseMove += Form_MouseMove;
                this.MouseUp += Form_MouseUp;
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point newLocation = this.PointToScreen(new Point(e.X, e.Y));
                this.Location = new Point(
                    newLocation.X - dragOffset.X,
                    newLocation.Y - dragOffset.Y
                );
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            this.MouseMove -= Form_MouseMove;
            this.MouseUp -= Form_MouseUp;
        }

        // Speed update karo
        public void UpdateSpeed(string download, string upload)
        {
            if (lblDownload != null)
                lblDownload.Text = $"↓ {download}/s";

            if (lblUpload != null)
                lblUpload.Text = $"↑ {upload}/s";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}