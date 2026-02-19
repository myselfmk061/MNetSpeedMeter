using System;
using System.Windows.Forms;

namespace MNetSpeedMeter
{
    public partial class SettingsForm : Form
    {
        // Public properties - Form1 access karega
        public int UpdateInterval { get; set; } = 1000;
        public bool ShowDownload { get; set; } = true;
        public bool ShowUpload { get; set; } = true;

        public SettingsForm()
        {
            InitializeComponent();

            // Form load hone pe settings show karo
            this.Load += SettingsForm_Load;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Interval radio button set karo
            if (UpdateInterval == 500)
                radio500ms.Checked = true;
            else if (UpdateInterval == 2000)
                radio2sec.Checked = true;
            else
                radio1sec.Checked = true;

            // Checkboxes set karo
            chkShowDownload.Checked = ShowDownload;
            chkShowUpload.Checked = ShowUpload;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Radio button se interval lo
            if (radio500ms.Checked)
                UpdateInterval = 500;
            else if (radio2sec.Checked)
                UpdateInterval = 2000;
            else
                UpdateInterval = 1000;

            // Checkboxes se values lo
            ShowDownload = chkShowDownload.Checked;
            ShowUpload = chkShowUpload.Checked;

            // Form band karo with OK result
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Form band karo without saving
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}