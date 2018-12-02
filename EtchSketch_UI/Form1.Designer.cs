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
            this.components = new System.ComponentModel.Container();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_browse = new System.Windows.Forms.Button();
            this.textBox_filePath = new System.Windows.Forms.TextBox();
            this.button_Upload = new System.Windows.Forms.Button();
            this.button_startStop = new System.Windows.Forms.Button();
            this.button_pauseResume = new System.Windows.Forms.Button();
            this.label_Resolution = new System.Windows.Forms.Label();
            this.comboBox_Resolution = new System.Windows.Forms.ComboBox();
            this.textBox_Status = new System.Windows.Forms.TextBox();
            this.label_Status = new System.Windows.Forms.Label();
            this.button_zeroMotor = new System.Windows.Forms.Button();
            this.richTextBox_debug = new System.Windows.Forms.RichTextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBox_serialPort = new System.Windows.Forms.ComboBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.textBox_Baud = new System.Windows.Forms.TextBox();
            this.timerDraw = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(63, 133);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(606, 454);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Picture Preview";
            // 
            // button_browse
            // 
            this.button_browse.Location = new System.Drawing.Point(479, 604);
            this.button_browse.Name = "button_browse";
            this.button_browse.Size = new System.Drawing.Size(190, 52);
            this.button_browse.TabIndex = 2;
            this.button_browse.Text = "Browse";
            this.button_browse.UseVisualStyleBackColor = true;
            this.button_browse.Click += new System.EventHandler(this.button_browse_Click);
            // 
            // textBox_filePath
            // 
            this.textBox_filePath.Location = new System.Drawing.Point(63, 615);
            this.textBox_filePath.Name = "textBox_filePath";
            this.textBox_filePath.Size = new System.Drawing.Size(386, 31);
            this.textBox_filePath.TabIndex = 3;
            // 
            // button_Upload
            // 
            this.button_Upload.Location = new System.Drawing.Point(479, 679);
            this.button_Upload.Name = "button_Upload";
            this.button_Upload.Size = new System.Drawing.Size(190, 57);
            this.button_Upload.TabIndex = 4;
            this.button_Upload.Text = "Upload";
            this.button_Upload.UseVisualStyleBackColor = true;
            this.button_Upload.Click += new System.EventHandler(this.button_Upload_Click);
            // 
            // button_startStop
            // 
            this.button_startStop.Location = new System.Drawing.Point(814, 337);
            this.button_startStop.Name = "button_startStop";
            this.button_startStop.Size = new System.Drawing.Size(306, 112);
            this.button_startStop.TabIndex = 5;
            this.button_startStop.Text = "Start Print";
            this.button_startStop.UseVisualStyleBackColor = true;
            this.button_startStop.Click += new System.EventHandler(this.button_startStop_Click);
            // 
            // button_pauseResume
            // 
            this.button_pauseResume.Location = new System.Drawing.Point(814, 479);
            this.button_pauseResume.Name = "button_pauseResume";
            this.button_pauseResume.Size = new System.Drawing.Size(306, 110);
            this.button_pauseResume.TabIndex = 6;
            this.button_pauseResume.Text = "Pause Print";
            this.button_pauseResume.UseVisualStyleBackColor = true;
            this.button_pauseResume.Click += new System.EventHandler(this.button_pauseResume_Click);
            // 
            // label_Resolution
            // 
            this.label_Resolution.AutoSize = true;
            this.label_Resolution.Location = new System.Drawing.Point(863, 233);
            this.label_Resolution.Name = "label_Resolution";
            this.label_Resolution.Size = new System.Drawing.Size(164, 25);
            this.label_Resolution.TabIndex = 8;
            this.label_Resolution.Text = "Print Resolution";
            // 
            // comboBox_Resolution
            // 
            this.comboBox_Resolution.FormattingEnabled = true;
            this.comboBox_Resolution.Location = new System.Drawing.Point(866, 261);
            this.comboBox_Resolution.Name = "comboBox_Resolution";
            this.comboBox_Resolution.Size = new System.Drawing.Size(215, 33);
            this.comboBox_Resolution.TabIndex = 9;
            this.comboBox_Resolution.Text = "Low";
            // 
            // textBox_Status
            // 
            this.textBox_Status.Location = new System.Drawing.Point(814, 692);
            this.textBox_Status.Name = "textBox_Status";
            this.textBox_Status.Size = new System.Drawing.Size(306, 31);
            this.textBox_Status.TabIndex = 10;
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(809, 651);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(73, 25);
            this.label_Status.TabIndex = 11;
            this.label_Status.Text = "Status";
            // 
            // button_zeroMotor
            // 
            this.button_zeroMotor.Location = new System.Drawing.Point(807, 133);
            this.button_zeroMotor.Name = "button_zeroMotor";
            this.button_zeroMotor.Size = new System.Drawing.Size(313, 51);
            this.button_zeroMotor.TabIndex = 12;
            this.button_zeroMotor.Text = "Zero Motor Position";
            this.button_zeroMotor.UseVisualStyleBackColor = true;
            this.button_zeroMotor.Click += new System.EventHandler(this.button_zeroMotor_Click);
            // 
            // richTextBox_debug
            // 
            this.richTextBox_debug.Location = new System.Drawing.Point(42, 787);
            this.richTextBox_debug.Name = "richTextBox_debug";
            this.richTextBox_debug.Size = new System.Drawing.Size(2670, 937);
            this.richTextBox_debug.TabIndex = 13;
            this.richTextBox_debug.Text = "";
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // comboBox_serialPort
            // 
            this.comboBox_serialPort.FormattingEnabled = true;
            this.comboBox_serialPort.Location = new System.Drawing.Point(303, 35);
            this.comboBox_serialPort.Name = "comboBox_serialPort";
            this.comboBox_serialPort.Size = new System.Drawing.Size(181, 33);
            this.comboBox_serialPort.TabIndex = 14;
            this.comboBox_serialPort.Click += new System.EventHandler(this.comboBox_serialPort_Click);
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(510, 29);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(159, 50);
            this.button_Connect.TabIndex = 15;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // textBox_Baud
            // 
            this.textBox_Baud.Location = new System.Drawing.Point(63, 35);
            this.textBox_Baud.Name = "textBox_Baud";
            this.textBox_Baud.Size = new System.Drawing.Size(211, 31);
            this.textBox_Baud.TabIndex = 16;
            this.textBox_Baud.Text = "9600";
            // 
            // timerDraw
            // 
            this.timerDraw.Enabled = true;
            this.timerDraw.Interval = 10;
            this.timerDraw.Tick += new System.EventHandler(this.timerDraw_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2740, 1783);
            this.Controls.Add(this.textBox_Baud);
            this.Controls.Add(this.button_Connect);
            this.Controls.Add(this.comboBox_serialPort);
            this.Controls.Add(this.richTextBox_debug);
            this.Controls.Add(this.button_zeroMotor);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.textBox_Status);
            this.Controls.Add(this.comboBox_Resolution);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.ComboBox comboBox_Resolution;
        private System.Windows.Forms.TextBox textBox_Status;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.Button button_zeroMotor;
        private System.Windows.Forms.RichTextBox richTextBox_debug;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBox_serialPort;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.TextBox textBox_Baud;
        private System.Windows.Forms.Timer timerDraw;
    }
}

