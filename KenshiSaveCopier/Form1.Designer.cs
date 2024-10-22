namespace KenshiSaveCopier
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
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notifyIcon1 = new NotifyIcon(components);
            comboBoxTriggerKey = new ComboBox();
            label1 = new Label();
            label_info = new Label();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "KenshiQuickSaveCopier";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // comboBoxTriggerKey
            // 
            comboBoxTriggerKey.FormattingEnabled = true;
            comboBoxTriggerKey.Location = new Point(12, 27);
            comboBoxTriggerKey.Name = "comboBoxTriggerKey";
            comboBoxTriggerKey.Size = new Size(344, 23);
            comboBoxTriggerKey.TabIndex = 0;
            comboBoxTriggerKey.SelectedIndexChanged += comboBoxTriggerKey_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(142, 15);
            label1.TabIndex = 1;
            label1.Text = "Kenshi Quick Save Button";
            // 
            // label_info
            // 
            label_info.AutoSize = true;
            label_info.Location = new Point(12, 209);
            label_info.Name = "label_info";
            label_info.Size = new Size(0, 15);
            label_info.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(368, 233);
            Controls.Add(label_info);
            Controls.Add(label1);
            Controls.Add(comboBoxTriggerKey);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            Resize += Form1_Resize;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private ComboBox comboBoxTriggerKey;
        private Label label1;
        private Label label_info;
    }
}
