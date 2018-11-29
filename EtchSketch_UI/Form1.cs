using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace EtchSketch_UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button_startStop_Click(object sender, EventArgs e)
        {
            Image imageIn = Image.FromFile(@"C:\Users\jen-s\Documents\MECH 4\MECH 423\4.FINAL PROJECT\Sample_Images\blackRectangle30x13px.bmp");
            byte[] byteArray;
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byteArray =  ms.ToArray();
            foreach (byte pixel in byteArray){
                richTextBox_debug.AppendText(pixel.ToString() + ", ");
            }

        }
    }
}
