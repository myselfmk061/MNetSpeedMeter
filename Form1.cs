using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace MNetSpeedMeter
{
    public partial class Form1 : Form
    {
        private long lastBytesReceived = 0;
        private long lastBytesSent = 0;

        private int updateInterval = 1000;
        private bool showDownload = true;
        private bool showUpload = true;

        // Statistics tracking
        private long totalDownloaded = 0;
        private long totalUploaded = 0;
        private DateTime sessionStartTime;

        // Speed display window
        private SpeedDisplayForm speedDisplay = null;
        private bool showSpeedWindow = true;

        public Form1()
        {
            InitializeComponent();

            // Custom icon load karo
            try
            {
                string iconPath = System.IO.Path.Combine(
                    Application.StartupPath, "speedometer.ico");

                if (System.IO.File.Exists(iconPath))
                {
                    notifyIcon1.Icon = new Icon(iconPath);
                    this.Icon = new Icon(iconPath);
                }
                else
                {
                    notifyIcon1.Icon = SystemIcons.Application;
                }
            }
            catch
            {
                // Agar file na mile to default use karo
                notifyIcon1.Icon = SystemIcons.Application;
            }

            notifyIcon1.Visible = true;
            notifyIcon1.Text = "MNetSpeedMeter - Starting...";

            this.Load += Form1_Load;
            timer1.Tick += Timer1_Tick;
            notifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;

            SetupContextMenu();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sessionStartTime = DateTime.Now;


            // Form hide karo
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            // Speed display window show karo
            speedDisplay = new SpeedDisplayForm();
            speedDisplay.Show();

            // Initial network data
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    try
                    {
                        IPv4InterfaceStatistics stats = ni.GetIPv4Statistics();
                        lastBytesReceived += stats.BytesReceived;
                        lastBytesSent += stats.BytesSent;
                    }
                    catch { }
                }
            }

            // Welcome notification
            notifyIcon1.ShowBalloonTip(2000, "MNetSpeedMeter",
                "Monitoring your network speed!", ToolTipIcon.Info);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            long currentBytesReceived = 0;
            long currentBytesSent = 0;

            // Sabhi network interfaces se data collect karo
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    try
                    {
                        IPv4InterfaceStatistics stats = ni.GetIPv4Statistics();
                        currentBytesReceived += stats.BytesReceived;
                        currentBytesSent += stats.BytesSent;
                    }
                    catch { }
                }
            }

            // Speed calculate karo
            long downloadSpeed = currentBytesReceived - lastBytesReceived;
            long uploadSpeed = currentBytesSent - lastBytesSent;

            // Update last values
            lastBytesReceived = currentBytesReceived;
            lastBytesSent = currentBytesSent;

            // data track karne ke liye
            totalDownloaded += downloadSpeed;
            totalUploaded += uploadSpeed;

            // Format speed
            string downText = FormatBytes(downloadSpeed);
            string upText = FormatBytes(uploadSpeed);

            // Settings ke hisaab se text banao
            string displayText = "";

            if (showDownload && showUpload)
                displayText = $"↓ {downText}/s  ↑ {upText}/s";
            else if (showDownload)
                displayText = $"↓ {downText}/s";
            else if (showUpload)
                displayText = $"↑ {upText}/s";
            else
                displayText = "MNetSpeedMeter";

            if (displayText.Length <= 63)
                notifyIcon1.Text = displayText;
            else
                notifyIcon1.Text = $"D:{downText}/s U:{upText}/s";

            // Dynamic icon update -
            UpdateIconWithSpeed(downloadSpeed, downText);

            // Speed display window update karo - YE ADD KARO
            if (speedDisplay != null && speedDisplay.Visible)
            {
                speedDisplay.UpdateSpeed(downText, upText);
            }
        }

        private string FormatBytes(long bytes)
        {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;

            if (bytes >= GB)
                return $"{bytes / (double)GB:0.00} GB";
            else if (bytes >= MB)
                return $"{bytes / (double)MB:0.00} MB";
            else if (bytes >= KB)
                return $"{bytes / (double)KB:0.00} KB";
            else
                return $"{bytes} B";
        }

        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(
                "MNetSpeedMeter v1.0\n\n" +
                "Real-time network speed monitor\n\n" +
                "Right-click icon for options",
                "About MNetSpeedMeter",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void SetupContextMenu()
        {
            contextMenuStrip1.Items.Clear();

            // About
            var aboutItem = new ToolStripMenuItem("About");
            aboutItem.Click += (s, e) => NotifyIcon1_DoubleClick(s, e);

            // Settings
            var settingsItem = new ToolStripMenuItem("Settings");
            settingsItem.Click += SettingsItem_Click;

            // Statistics
            var statsItem = new ToolStripMenuItem("Statistics");
            statsItem.Click += StatsItem_Click;

            // Show/Hide Speed Window
            var speedWindowItem = new ToolStripMenuItem("Show Speed Window");
            speedWindowItem.CheckOnClick = true;
            speedWindowItem.Checked = true;
            speedWindowItem.Click += SpeedWindowItem_Click;

            // Start with Windows
            var startupItem = new ToolStripMenuItem("Start with Windows");
            startupItem.CheckOnClick = true;
            startupItem.Checked = IsInStartup();
            startupItem.Click += StartupItem_Click;

            // Exit
            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += ExitItem_Click;

            // Add items
            contextMenuStrip1.Items.Add(aboutItem);
            contextMenuStrip1.Items.Add(settingsItem);
            contextMenuStrip1.Items.Add(statsItem);
            contextMenuStrip1.Items.Add(speedWindowItem);
            contextMenuStrip1.Items.Add(new ToolStripSeparator());
            contextMenuStrip1.Items.Add(startupItem);
            contextMenuStrip1.Items.Add(new ToolStripSeparator());
            contextMenuStrip1.Items.Add(exitItem);
        }

        // Dynamic icon banane ke liye
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private void UpdateIconWithSpeed(long downloadSpeed, string speedText)
        {
            // Icon size
            int iconSize = 16;

            // Bitmap banao
            Bitmap bitmap = new Bitmap(iconSize, iconSize);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Speed ke hisaab se color choose karo
            Color speedColor;
            if (downloadSpeed > 1024 * 1024) // > 1 MB/s
                speedColor = Color.LimeGreen;
            else if (downloadSpeed > 100 * 1024) // > 100 KB/s
                speedColor = Color.Gold;
            else if (downloadSpeed > 0)
                speedColor = Color.OrangeRed;
            else
                speedColor = Color.Gray;

            // Background
            g.Clear(Color.Transparent);

            // Circle draw karo
            using (SolidBrush brush = new SolidBrush(speedColor))
            {
                g.FillEllipse(brush, 0, 0, iconSize - 1, iconSize - 1);
            }

            // Border
            using (Pen pen = new Pen(Color.White, 1))
            {
                g.DrawEllipse(pen, 0, 0, iconSize - 1, iconSize - 1);
            }

            // Icon set karo
            IntPtr hIcon = bitmap.GetHicon();
            Icon newIcon = Icon.FromHandle(hIcon);

            // Purana icon dispose karo
            Icon oldIcon = notifyIcon1.Icon;
            notifyIcon1.Icon = newIcon;

            if (oldIcon != null)
            {
                DestroyIcon(oldIcon.Handle);
                oldIcon.Dispose();
            }

            DestroyIcon(hIcon);
            g.Dispose();
            bitmap.Dispose();
        }

        private void SpeedWindowItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            if (speedDisplay == null)
            {
                speedDisplay = new SpeedDisplayForm();
            }

            if (item.Checked)
            {
                speedDisplay.Show();
            }
            else
            {
                speedDisplay.Hide();
            }
        }
        private void StatsItem_Click(object sender, EventArgs e)
        {
            // Session duration calculate karo
            TimeSpan sessionDuration = DateTime.Now - sessionStartTime;

            // Average speed calculate karo
            double avgDownloadSpeed = 0;
            double avgUploadSpeed = 0;

            if (sessionDuration.TotalSeconds > 0)
            {
                avgDownloadSpeed = totalDownloaded / sessionDuration.TotalSeconds;
                avgUploadSpeed = totalUploaded / sessionDuration.TotalSeconds;
            }

            // Statistics message banao
            string statsMessage =
                "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                "   SESSION STATISTICS\n" +
                "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +

                $"⏱️  Duration: {sessionDuration.Hours:D2}:{sessionDuration.Minutes:D2}:{sessionDuration.Seconds:D2}\n\n" +

                $"📥 Total Downloaded:\n" +
                $"     {FormatBytes(totalDownloaded)}\n\n" +

                $"📤 Total Uploaded:\n" +
                $"     {FormatBytes(totalUploaded)}\n\n" +

                $"📊 Total Data Usage:\n" +
                $"     {FormatBytes(totalDownloaded + totalUploaded)}\n\n" +

                "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                "   AVERAGE SPEEDS\n" +
                "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +

                $"📥 Avg Download: {FormatBytes((long)avgDownloadSpeed)}/s\n\n" +

                $"📤 Avg Upload: {FormatBytes((long)avgUploadSpeed)}/s\n\n" +

                "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━";

            // MessageBox show karo
            MessageBox.Show(
                statsMessage,
                "MNetSpeedMeter - Statistics",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // YE NAYA - Reset option
            var result = MessageBox.Show(
                "Do you want to reset statistics?",
                "Reset Statistics",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                totalDownloaded = 0;
                totalUploaded = 0;
                sessionStartTime = DateTime.Now;

                MessageBox.Show(
                    "Statistics reset successfully!",
                    "Reset Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void SettingsItem_Click(object sender, EventArgs e)
        {
            // Settings form banao
            SettingsForm settingsForm = new SettingsForm();

            // Current settings pass karo
            settingsForm.UpdateInterval = updateInterval;
            settingsForm.ShowDownload = showDownload;
            settingsForm.ShowUpload = showUpload;

            // Form show karo aur wait karo Save button ke liye
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                // User ne Save kiya - new settings apply karo
                updateInterval = settingsForm.UpdateInterval;
                showDownload = settingsForm.ShowDownload;
                showUpload = settingsForm.ShowUpload;

                // Timer ka interval change karo
                timer1.Interval = updateInterval;

                MessageBox.Show("Settings saved!", "Success");
            }
        }
        private void StartupItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            if (item.Checked)
                AddToStartup();
            else
                RemoveFromStartup();
        }

        private void AddToStartup()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                key.SetValue("MNetSpeedMeter",
                    $"\"{Application.ExecutablePath}\"");

                MessageBox.Show(
                    "MNetSpeedMeter will now start automatically with Windows!",
                    "Startup Enabled",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not add to startup:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RemoveFromStartup()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                key.DeleteValue("MNetSpeedMeter", false);

                MessageBox.Show(
                    "MNetSpeedMeter removed from Windows startup!",
                    "Startup Disabled",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not remove from startup:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool IsInStartup()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);

                return key.GetValue("MNetSpeedMeter") != null;
            }
            catch
            {
                return false;
            }
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            // Confirm karo
            var result = MessageBox.Show(
                "Are you sure you want to exit MNetSpeedMeter?",
                "Exit Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Speed display band karo - YE ADD KARO
                if (speedDisplay != null)
                {
                    speedDisplay.Close();
                    speedDisplay.Dispose();
                }

                notifyIcon1.Visible = false;
                Application.Exit();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Close button pe minimize karo
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}