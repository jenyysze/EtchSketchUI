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
        int screenWidthMM = 150; // mm
        int screenHeightMM = 100; // mm
        int tileSizeMM = 10; // mm
        int heightInTiles;
        int widthInTiles;

        int fileSizeOffset = 2,
            imageDataOffset = 10,
            widthOffset = 18,
            heightOffset = 22,
            colorPlanesOffset = 26,
            bitsPerPixelOffset = 28,
            compressionOffset = 30,
            imageSizeOffset = 34,
            pxPerMeterHorizontalOffset = 38,
            pxPerMeterVerticalOffset = 42;
            

        class image24BitBMP {
            public ulong fileSize;
            public ulong imageDataOffset;
            public ulong imageWidthPixels;
            public ulong imageHeightPixels;
            public ulong numColorPlanes;
            public ulong bitsPerPixel;
            public ulong compressionType;
            public ulong pixelsPerMMHorizontal;
            public ulong pixelsPerMMVertical;
            public byte[] imageData;
            public byte[] commandBytes;
            public byte[] grayScaleImageData;
            public byte[] grayScaleImageDataTruncated;
        }

        private void processImageData(ref image24BitBMP image)
        {
            // Calculate tile height and width
            heightInTiles = screenHeightMM / tileSizeMM;
            widthInTiles = screenWidthMM / tileSizeMM;
            int numTiles = heightInTiles * widthInTiles;
            int numPixels = (int)image.imageHeightPixels * (int)image.imageWidthPixels;

            int currentTile = 0;
            int currentGrayscalePixel = 0;
            int currentImageByte = 0;
            int row = 0, column = 0;


            // Convert image to grayscale
            int padding = 0;
            padding = (int)image.imageWidthPixels % 4;

            currentGrayscalePixel = 0;

            for (row = 0; row < (int)image.imageHeightPixels; row++)
            {
                for (column = 0; column < (int)image.imageWidthPixels; column++)
                {
                    int i = row * ((int)image.imageWidthPixels) * 3  + padding + column * 3;
                    image.grayScaleImageData[currentGrayscalePixel] = (byte) (((int)image.imageData[i] + (int)image.imageData[i + 1] + (int)image.imageData[i + 2]) / 3 );
                    currentGrayscalePixel++;
                }
            }

            // Scale image, assuming image's vertical and horizontal resolution are equal

            // Image height exceeds screen limits
            if ((double)image.imageHeightPixels / image.pixelsPerMMVertical > screenHeightMM)
            {
                image.pixelsPerMMVertical = (ulong)Math.Ceiling((double)image.imageHeightPixels / screenHeightMM);
                image.pixelsPerMMHorizontal = image.pixelsPerMMVertical;
            }

            // Image width exceeds screen limits
            if ((double)image.imageWidthPixels / image.pixelsPerMMHorizontal > screenWidthMM)
            {
                image.pixelsPerMMHorizontal = (ulong)Math.Ceiling((double)image.imageWidthPixels / screenWidthMM);
                image.pixelsPerMMVertical = image.pixelsPerMMHorizontal;
            }

            // Calculate pixels per tile
            int tileWidthPx = (int)image.pixelsPerMMHorizontal * tileSizeMM;
            int tileHeightPx = (int)image.pixelsPerMMVertical * tileSizeMM;

            // Crop image to eliminate stray pixels
            int numStrayHorzPixels = (int)image.imageWidthPixels % (int)image.pixelsPerMMHorizontal;
            int numStrayVertPixels = (int)image.imageHeightPixels % (int)image.pixelsPerMMVertical;
            if (numStrayHorzPixels > 0 || numStrayVertPixels > 0)
            {
                currentGrayscalePixel = 0;

                image.imageHeightPixels -= (ulong)numStrayVertPixels;
                image.imageWidthPixels -= (ulong)numStrayHorzPixels;
                numPixels = (int)image.imageHeightPixels * (int)image.imageWidthPixels;

                image.grayScaleImageDataTruncated = new byte[image.imageWidthPixels * image.imageHeightPixels];

                for(row = 0; row < (int)image.imageHeightPixels; row++)
                {
                    for (column = 0; column < (int)image.imageWidthPixels; column++)
                    {
                        int i = row * ((int)image.imageWidthPixels + numStrayHorzPixels) + column;
                        image.grayScaleImageDataTruncated[currentGrayscalePixel] = image.grayScaleImageData[i];
                        currentGrayscalePixel++;
                    }
                }
            }
            
            // Create command byte array
            currentTile = 0;
            for (row = 0; row < heightInTiles; row++)
            {
                for(column = 0; column < widthInTiles; column++)
                {
                    // Sum pixels in each tile
                    currentGrayscalePixel = (currentTile + 1) * tileWidthPx * tileHeightPx;
                    int pixelRow = 0, pixelColumn = 0;
                    for(pixelRow = 0; pixelRow < tileHeightPx; pixelRow++)
                    {
                        for(pixelColumn = 0; pixelColumn < tileWidthPx; pixelColumn++)
                        {

                        }
                    }
                    currentTile++;
                }
            }

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
            image.imageWidthPixels = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = heightOffset;
            image.imageHeightPixels = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = colorPlanesOffset;
            image.numColorPlanes = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = bitsPerPixelOffset;
            image.bitsPerPixel = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = compressionOffset;
            image.compressionType = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]);

            i = pxPerMeterHorizontalOffset;
            image.pixelsPerMMHorizontal = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]) / 1000;

            i = pxPerMeterVerticalOffset;
            image.pixelsPerMMVertical = getFourByteNumber(imageArray[i], imageArray[i + 1], imageArray[i + 2], imageArray[i + 3]) / 1000;

            image.imageData = new byte[imageArray.Length - (int)image.imageDataOffset];
            Array.Copy(imageArray, (int)image.imageDataOffset, image.imageData, 0, image.imageData.Length);

            image.grayScaleImageData = new byte[(int)image.imageWidthPixels * (int)image.imageHeightPixels];
            image.commandBytes = new byte[(screenWidthMM * screenHeightMM)/ (tileSizeMM * tileSizeMM)];

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
            // Convert image to byte array
            Image imageIn = Image.FromFile(@"C:\Users\jen-s\Documents\MECH 4\MECH 423\4.FINAL PROJECT\Sample_Images\blackWhiteRectangle.bmp");
            byte[] byteArray;
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byteArray =  ms.ToArray();
            image24BitBMP currentImage = new image24BitBMP();

            // Parse image
            parseImageArray(byteArray, ref currentImage);

            // Process image array
            processImageData(ref currentImage);

            foreach (byte pixel in byteArray){
                richTextBox_debug.AppendText(pixel.ToString() + ", ");
            }

        }
    }
}
