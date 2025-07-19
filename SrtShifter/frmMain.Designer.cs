namespace SrtShifter
{
    partial class frmMain
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
            btnSelectVideoFile = new Button();
            txtVideoFilePath = new TextBox();
            txtSrtFilePath = new TextBox();
            btnSelectSrtFile = new Button();
            txtLogText = new TextBox();
            SuspendLayout();
            // 
            // btnSelectVideoFile
            // 
            btnSelectVideoFile.Location = new Point(12, 12);
            btnSelectVideoFile.Name = "btnSelectVideoFile";
            btnSelectVideoFile.Size = new Size(192, 34);
            btnSelectVideoFile.TabIndex = 0;
            btnSelectVideoFile.Text = "Select Video File...";
            btnSelectVideoFile.UseVisualStyleBackColor = true;
            // 
            // txtVideoFilePath
            // 
            txtVideoFilePath.Location = new Point(210, 12);
            txtVideoFilePath.Name = "txtVideoFilePath";
            txtVideoFilePath.Size = new Size(578, 31);
            txtVideoFilePath.TabIndex = 1;
            // 
            // txtSrtFilePath
            // 
            txtSrtFilePath.Location = new Point(210, 52);
            txtSrtFilePath.Name = "txtSrtFilePath";
            txtSrtFilePath.Size = new Size(578, 31);
            txtSrtFilePath.TabIndex = 3;
            // 
            // btnSelectSrtFile
            // 
            btnSelectSrtFile.Location = new Point(12, 52);
            btnSelectSrtFile.Name = "btnSelectSrtFile";
            btnSelectSrtFile.Size = new Size(192, 34);
            btnSelectSrtFile.TabIndex = 2;
            btnSelectSrtFile.Text = "Select Srt File...";
            btnSelectSrtFile.UseVisualStyleBackColor = true;
            // 
            // txtLogText
            // 
            txtLogText.Location = new Point(15, 92);
            txtLogText.Multiline = true;
            txtLogText.Name = "txtLogText";
            txtLogText.ScrollBars = ScrollBars.Both;
            txtLogText.Size = new Size(773, 346);
            txtLogText.TabIndex = 4;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtLogText);
            Controls.Add(txtSrtFilePath);
            Controls.Add(btnSelectSrtFile);
            Controls.Add(txtVideoFilePath);
            Controls.Add(btnSelectVideoFile);
            Name = "frmMain";
            Text = "SRT File Shifter";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSelectVideoFile;
        private TextBox txtVideoFilePath;
        private TextBox txtSrtFilePath;
        private Button btnSelectSrtFile;
        private TextBox txtLogText;
    }
}
