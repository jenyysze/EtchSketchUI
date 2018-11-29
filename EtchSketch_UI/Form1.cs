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
        int fileSizeOffset = 2,
            imageDataOffset = 10,
            widthOffset = 18,
            heightOffset = 22,
            colorPlanesOffset = 26,
            bitsPerPixelOffset = 28,
            compressionOffset = 30,
            imageSizeOffset = 34;

        class image24BitBMP {
            public ulong fileSize;
            public ulong imageDataOffset;
            public ulong imageWidth;
            public ulong imageHeight;
            public ulong numColorPlanes;
            public ulong bitsPerPixel;
            public ulong compressionType;
            public byte[] imageData;
        }

        private ulong getTwoByteNumber(byte LSB, byte MSB)
        {
            ulong twoByteNumber;
            twoByteNumber = (ulong)((MSB << 8) | LSB);
            return twoByteNumber;
        }

        private ulong getFourByteNumber(byte byte1, byte byte2, byte byte3, byte byte4)
        {
            ulong fourByteNumber;
            fourByteNumber = (ulong) (byte1 | (byte2 << 8) | (byte3 << 16) | (byte4 << 24));
            return fourByteNumber;
        }
        private void parseImageArray(byte[] imageArray, ref image24BitBMP image)
        {
            int i = fileSizeOffset;
            image.fileSize = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = imageDataOffset;
            image.imageDataOffset = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = widthOffset;
            image.imageWidth = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = heightOffset;
            image.imageHeight = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = colorPlanesOffset;
            image.numColorPlanes = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = bitsPerPixelOffset;
            image.bitsPerPixel = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = compressionOffset;
            image.compressionType = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            imageArray.CopyTo(image.imageData, (uint)image.imageDataOffset);
        }

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
