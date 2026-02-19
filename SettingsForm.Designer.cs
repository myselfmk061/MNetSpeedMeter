namespace MNetSpeedMeter
{
    partial class SettingsForm
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
            groupBox1 = new GroupBox();
            radio500ms = new RadioButton();
            radio1sec = new RadioButton();
            radio2sec = new RadioButton();
            groupBox2 = new GroupBox();
            chkShowDownload = new CheckBox();
            chkShowUpload = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radio2sec);
            groupBox1.Controls.Add(radio1sec);
            groupBox1.Controls.Add(radio500ms);
            groupBox1.Location = new Point(39, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(250, 125);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Update Interval";
            // 
            // radio500ms
            // 
            radio500ms.AutoSize = true;
            radio500ms.Location = new Point(5, 26);
            radio500ms.Name = "radio500ms";
            radio500ms.Size = new Size(113, 24);
            radio500ms.TabIndex = 0;
            radio500ms.Text = "Fast (0.5 sec)";
            radio500ms.UseVisualStyleBackColor = true;
            // 
            // radio1sec
            // 
            radio1sec.AutoSize = true;
            radio1sec.Checked = true;
            radio1sec.Location = new Point(5, 48);
            radio1sec.Name = "radio1sec";
            radio1sec.Size = new Size(127, 24);
            radio1sec.TabIndex = 1;
            radio1sec.TabStop = true;
            radio1sec.Text = "Normal (1 sec)";
            radio1sec.UseVisualStyleBackColor = true;
            // 
            // radio2sec
            // 
            radio2sec.AutoSize = true;
            radio2sec.Location = new Point(5, 69);
            radio2sec.Name = "radio2sec";
            radio2sec.Size = new Size(109, 24);
            radio2sec.TabIndex = 2;
            radio2sec.Text = "Slow (2 sec)";
            radio2sec.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(chkShowUpload);
            groupBox2.Controls.Add(chkShowDownload);
            groupBox2.Location = new Point(39, 143);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(250, 106);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Display Options";
            // 
            // chkShowDownload
            // 
            chkShowDownload.AutoSize = true;
            chkShowDownload.Checked = true;
            chkShowDownload.CheckState = CheckState.Checked;
            chkShowDownload.Location = new Point(12, 26);
            chkShowDownload.Name = "chkShowDownload";
            chkShowDownload.Size = new Size(186, 24);
            chkShowDownload.TabIndex = 0;
            chkShowDownload.Text = "Show Download Speed";
            chkShowDownload.UseVisualStyleBackColor = true;
            // 
            // chkShowUpload
            // 
            chkShowUpload.AutoSize = true;
            chkShowUpload.Checked = true;
            chkShowUpload.CheckState = CheckState.Checked;
            chkShowUpload.Location = new Point(12, 52);
            chkShowUpload.Name = "chkShowUpload";
            chkShowUpload.Size = new Size(166, 24);
            chkShowUpload.TabIndex = 1;
            chkShowUpload.Text = "Show Upload Speed";
            chkShowUpload.UseVisualStyleBackColor = true;

            // 
            // btnSave
            // 
            btnSave.Location = new Point(39, 263);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(195, 263);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.TextAlign = ContentAlignment.BottomCenter;
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(332, 303);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private RadioButton radio2sec;
        private RadioButton radio1sec;
        private RadioButton radio500ms;
        private GroupBox groupBox2;
        private CheckBox chkShowUpload;
        private CheckBox chkShowDownload;
        private Button btnSave;
        private Button btnCancel;
    }
}