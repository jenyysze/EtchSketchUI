namespace EtchSketch_UI
{
    partial class Form1
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_browse = new System.Windows.Forms.Button();
            this.textBox_filePath = new System.Windows.Forms.TextBox();
            this.button_Upload = new System.Windows.Forms.Button();
            this.button_startStop = new System.Windows.Forms.Button();
            this.button_pauseResume = new System.Windows.Forms.Button();
            this.label_Resolution = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label_Status = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox_debug = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(64, 77);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(606, 454);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Picture Preview";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button_browse
            // 
            this.button_browse.Location = new System.Drawing.Point(480, 548);
            this.button_browse.Name = "button_browse";
            this.button_browse.Size = new System.Drawing.Size(190, 52);
            this.button_browse.TabIndex = 2;
            this.button_browse.Text = "Browse";
            this.button_browse.UseVisualStyleBackColor = true;
            // 
            // textBox_filePath
            // 
            this.textBox_filePath.Location = new System.Drawing.Point(64, 559);
            this.textBox_filePath.Name = "textBox_filePath";
            this.textBox_filePath.Size = new System.Drawing.Size(386, 31);
            this.textBox_filePath.TabIndex = 3;
            // 
            // button_Upload
            // 
            this.button_Upload.Location = new System.Drawing.Point(480, 623);
            this.button_Upload.Name = "button_Upload";
            this.button_Upload.Size = new System.Drawing.Size(190, 57);
            this.button_Upload.TabIndex = 4;
            this.button_Upload.Text = "Upload";
            this.button_Upload.UseVisualStyleBackColor = true;
            // 
            // button_startStop
            // 
            this.button_startStop.Location = new System.Drawing.Point(815, 281);
            this.button_startStop.Name = "button_startStop";
            this.button_startStop.Size = new System.Drawing.Size(306, 112);
            this.button_startStop.TabIndex = 5;
            this.button_startStop.Text = "Start Print";
            this.button_startStop.UseVisualStyleBackColor = true;
            this.button_startStop.Click += new System.EventHandler(this.button_startStop_Click);
            // 
            // button_pauseResume
            // 
            this.button_pauseResume.Location = new System.Drawing.Point(815, 423);
            this.button_pauseResume.Name = "button_pauseResume";
            this.button_pauseResume.Size = new System.Drawing.Size(306, 110);
            this.button_pauseResume.TabIndex = 6;
            this.button_pauseResume.Text = "Pause Print";
            this.button_pauseResume.UseVisualStyleBackColor = true;
            // 
            // label_Resolution
            // 
            this.label_Resolution.AutoSize = true;
            this.label_Resolution.Location = new System.Drawing.Point(864, 177);
            this.label_Resolution.Name = "label_Resolution";
            this.label_Resolution.Size = new System.Drawing.Size(164, 25);
            this.label_Resolution.TabIndex = 8;
            this.label_Resolution.Text = "Print Resolution";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(867, 205);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(215, 33);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.Text = "Low";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(815, 636);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(306, 31);
            this.textBox1.TabIndex = 10;
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(810, 595);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(73, 25);
            this.label_Status.TabIndex = 11;
            this.label_Status.Text = "Status";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(808, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(313, 51);
            this.button1.TabIndex = 12;
            this.button1.Text = "Zero Motor Position";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // richTextBox_debug
            // 
            this.richTextBox_debug.Location = new System.Drawing.Point(42, 787);
            this.richTextBox_debug.Name = "richTextBox_debug";
            this.richTextBox_debug.Size = new System.Drawing.Size(2670, 937);
            this.richTextBox_debug.TabIndex = 13;
            this.richTextBox_debug.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2740, 1783);
            this.Controls.Add(this.richTextBox_debug);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label_Resolution);
            this.Controls.Add(this.button_pauseResume);
            this.Controls.Add(this.button_startStop);
            this.Controls.Add(this.button_Upload);
            this.Controls.Add(this.textBox_filePath);
            this.Controls.Add(this.button_browse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_browse;
        private System.Windows.Forms.TextBox textBox_filePath;
        private System.Windows.Forms.Button button_Upload;
        private System.Windows.Forms.Button button_startStop;
        private System.Windows.Forms.Button button_pauseResume;
        private System.Windows.Forms.Label label_Resolution;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox_debug;
    }
}

