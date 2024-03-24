namespace Dcm2Img {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            textBox1 = new TextBox();
            ButtonSelectFolder = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            ButtonStart = new Button();
            progressBar1 = new ProgressBar();
            label1 = new Label();
            textBox2 = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(14, 17);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(985, 31);
            textBox1.TabIndex = 0;
            // 
            // ButtonSelectFolder
            // 
            ButtonSelectFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ButtonSelectFolder.Location = new Point(1005, 15);
            ButtonSelectFolder.Name = "ButtonSelectFolder";
            ButtonSelectFolder.Size = new Size(112, 34);
            ButtonSelectFolder.TabIndex = 1;
            ButtonSelectFolder.Text = "Browse";
            ButtonSelectFolder.UseVisualStyleBackColor = true;
            ButtonSelectFolder.Click += ButtonSelectFolder_Click;
            // 
            // folderBrowserDialog1
            // 
            folderBrowserDialog1.ShowHiddenFiles = true;
            folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // ButtonStart
            // 
            ButtonStart.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ButtonStart.Location = new Point(983, 716);
            ButtonStart.Name = "ButtonStart";
            ButtonStart.Size = new Size(134, 47);
            ButtonStart.TabIndex = 2;
            ButtonStart.Text = "Convert";
            ButtonStart.UseVisualStyleBackColor = true;
            ButtonStart.Click += ButtonStart_Click;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(14, 106);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1103, 34);
            progressBar1.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 78);
            label1.Name = "label1";
            label1.Size = new Size(341, 25);
            label1.TabIndex = 5;
            label1.Text = "Select directory to search for dicom files...";
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox2.Location = new Point(14, 146);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(1103, 564);
            textBox2.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new Point(828, 729);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 7;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1129, 775);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(progressBar1);
            Controls.Add(ButtonStart);
            Controls.Add(ButtonSelectFolder);
            Controls.Add(textBox1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dicom to Image";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button ButtonSelectFolder;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button ButtonStart;
        private ProgressBar progressBar1;
        private Label label1;
        private TextBox textBox2;
        private Button button1;
    }
}
