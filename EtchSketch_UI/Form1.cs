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
        int screenWidthMM = 120; // mm
        int screenHeightMM = 100; // mm
        int tileSizeMM = 4; // mm
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
            public ulong imageWidthTiles;
            public ulong imageHeightTiles;
            public ulong numColorPlanes;
            public ulong bitsPerPixel;
            public ulong compressionType;
            public ulong pixelsPerMMHorizontal;
            public ulong pixelsPerMMVertical;
            public byte[] imageData;
            public byte[] commandBytes;
            public byte[] grayScaleImageData;
            public byte[] grayScaleImageDataCorrectSize;
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
            int padding = ((int)image.imageWidthPixels * 3) % 4;
            if( padding != 0)
            {
                padding = 4 - padding;
            }

            currentGrayscalePixel = 0;

            for (row = 0; row < (int)image.imageHeightPixels; row++)
            {
                for (column = 0; column < (int)image.imageWidthPixels; column++)
                {
                    int i = row * (((int)image.imageWidthPixels) * 3  + padding) + column * 3;
                    image.grayScaleImageData[currentGrayscalePixel] = (byte) (((int)image.imageData[i] + (int)image.imageData[i + 1] + (int)image.imageData[i + 2]) / 3 );
                    currentGrayscalePixel++;
                }
            }

            // Scale image, assuming image's vertical and horizontal resolution are equal

            // Image height exceeds screen limits
            if ((double)image.imageHeightPixels / image.pixelsPerMMVertical > screenHeightMM)
            {
                image.pixelsPerMMVertical = (ulong)((double)image.imageHeightPixels / screenHeightMM);
                image.pixelsPerMMHorizontal = image.pixelsPerMMVertical;
            }

            // Image width exceeds screen limits
            if ((double)image.imageWidthPixels / image.pixelsPerMMHorizontal > screenWidthMM)
            {
                image.pixelsPerMMHorizontal = (ulong)((double)image.imageWidthPixels / screenWidthMM);
                image.pixelsPerMMVertical = image.pixelsPerMMHorizontal;
            }


            // Crop image to eliminate stray pixels
            int numStrayHorzPixels = (int)image.imageWidthPixels % ((int)image.pixelsPerMMHorizontal * tileSizeMM);
            int numStrayVertPixels = (int)image.imageHeightPixels % ((int)image.pixelsPerMMVertical * tileSizeMM);
            if (numStrayHorzPixels > 0 || numStrayVertPixels > 0)
            {
                currentGrayscalePixel = 0;

                image.imageHeightPixels -= (ulong)numStrayVertPixels;
                image.imageWidthPixels -= (ulong)numStrayHorzPixels;
                numPixels = (int)image.imageHeightPixels * (int)image.imageWidthPixels;

                image.grayScaleImageDataCorrectSize = new byte[image.imageWidthPixels * image.imageHeightPixels];

                for(row = 0; row < (int)image.imageHeightPixels; row++)
                {
                    for (column = 0; column < (int)image.imageWidthPixels; column++)
                    {
                        int i = row * ((int)image.imageWidthPixels + numStrayHorzPixels) + column;
                        image.grayScaleImageDataCorrectSize[currentGrayscalePixel] = image.grayScaleImageData[i];
                        currentGrayscalePixel++;
                    }
                }             
            }
            else
            {
                image.grayScaleImageDataCorrectSize = new byte[image.grayScaleImageData.Length];
                Array.Copy(image.grayScaleImageData, image.grayScaleImageDataCorrectSize, image.grayScaleImageData.Length);
            }


            // Calculate pixels per tile
            int tileWidthPx = (int)image.pixelsPerMMHorizontal * tileSizeMM;
            int tileHeightPx = (int)image.pixelsPerMMVertical * tileSizeMM;

            // Calculate image size in tiles
            image.imageWidthTiles = image.imageWidthPixels / image.pixelsPerMMHorizontal / (ulong)tileSizeMM;
            image.imageHeightTiles = image.imageHeightPixels / image.pixelsPerMMVertical / (ulong)tileSizeMM;

            // Create command byte array
            currentTile = 0;
            int currentTileValue = 0;
            for (row = 0; row < (int)image.imageHeightTiles; row++)
            {
                for(column = 0; column < (int)image.imageWidthTiles; column++)
                {
                    // Calculate top left tile number
                    int i = (row) * (int)image.imageWidthPixels * tileHeightPx + column * tileWidthPx;

                    // Sum pixels in each tile
                    int pixelRow = 0, pixelColumn = 0;
                    for(pixelRow = 0; pixelRow < tileHeightPx; pixelRow++)
                    {
                        for(pixelColumn = 0; pixelColumn < tileWidthPx; pixelColumn++)
                        {
                            currentTileValue += image.grayScaleImageDataCorrectSize[i + pixelColumn];
                        }
                    }

                    currentTileValue /= (tileWidthPx * tileHeightPx);
                    image.commandBytes[currentTile] = (byte)currentTileValue;
                    currentTile++;
                    currentTileValue = 0;
                }
            }
            currentTileValue = 0;

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
            Image imageIn = Image.FromFile(@"C:\Users\jen-s\Documents\MECH 4\MECH 423\4.FINAL PROJECT\Sample_Images\apple.bmp");
            byte[] byteArray;
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byteArray =  ms.ToArray();
            image24BitBMP currentImage = new image24BitBMP();

            // Parse image
            parseImageArray(byteArray, ref currentImage);

            // Process image array
            processImageData(ref currentImage);

            // Print out the image
            int currentTile = 0;
            foreach (byte commandByte in currentImage.commandBytes){
                richTextBox_debug.AppendText(commandByte.ToString() + " ");
                if ((currentTile + 1) % (int)currentImage.imageWidthTiles == 0)
                {
                    richTextBox_debug.AppendText(Environment.NewLine);
                }
                currentTile++;
            }
            //int grayscaleByteCount = 0;
            //foreach (byte grayscaleByte in currentImage.grayScaleImageDataCorrectSize)
            //{
            //    richTextBox_debug.AppendText(grayscaleByte.ToString() + " ");
            //    if ((grayscaleByteCount + 1) % (int)currentImage.imageWidthPixels == 0)
            //    {
            //        richTextBox_debug.AppendText(Environment.NewLine);
            //    }
            //    grayscaleByteCount++;
            //}


            currentTile = 0;

        }
    }
}
