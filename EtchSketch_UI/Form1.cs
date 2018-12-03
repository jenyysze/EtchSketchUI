using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.IO;

namespace EtchSketch_UI
{

    public partial class Form1 : Form
    {
        ConcurrentQueue<byte> cqueue = new ConcurrentQueue<byte>();
        ConcurrentQueue<byte> drawQueue = new ConcurrentQueue<byte>();

        image24BitBMP currentImage;
        string imageLocation = @"C:\Users\jen-s\Documents\MECH 4\MECH 423\4.FINAL PROJECT\Sample_Images\grayscalePallet.bmp";
        enum printerState { printInProgress, printPaused, idle, sendDrawingBytes, readyForNextDrawingByte };

        printerState currentPrinterState = printerState.idle;

        byte startByte = 255;
        byte dataByte = 0;
        byte startPrintCommand = 1;
        byte stopPrintCommand = 0;
        byte pausePrintCommand = 2;
        byte resumePrintCommand = 3;
        byte zeroMotorCommand = 4;
        byte drawCommand = 5;

        int screenWidthMM = 144; // mm
        int screenHeightMM = 96; // mm
        int lowResTileSizeMM = 4;
        int mediumResTileSizeMM = 3;
        int highResTileSizeMM = 2;
        int tileSizeMM = 4; // mm

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

        private void button_Connect_Click(object sender, EventArgs e)
        {
            int baud = 0;
            if (serialPort1.IsOpen) // Close port operation
            {
                serialPort1.Close();
                button_Connect.Text = "Connect";
            }
            else if (!serialPort1.IsOpen) // Open port operation
            {
                if (int.TryParse(textBox_Baud.Text, out baud) && comboBox_serialPort.Text != "")
                {
                    serialPort1.PortName = comboBox_serialPort.Text;
                    serialPort1.BaudRate = baud;
                    try
                    {
                        serialPort1.Open();
                        button_Connect.Text = "Disconnect";
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error opening serial port: " + ex.Message);
                    }

                }
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                int numBytesRxBuffer = serialPort1.BytesToRead;
                byte[] buffer = new byte[numBytesRxBuffer];
                serialPort1.Read(buffer, 0, numBytesRxBuffer);
                foreach (byte data in buffer)
                {
                    cqueue.Enqueue(data);
                }
                numBytesRxBuffer = serialPort1.BytesToRead;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Dequeue and print incoming messages from Arduino 
            byte queueData = 0;
            while(cqueue.Count > 0)
            {
                cqueue.TryDequeue(out queueData);
                richTextBox_debug.AppendText( queueData + " ");

                // Incoming command from Arduino
                if(queueData == 255)
                {
                    cqueue.TryDequeue(out queueData);
                    // Print has been complete, change to idle state
                    if (queueData == 5 && currentPrinterState == printerState.printInProgress)
                    {
                        currentPrinterState = printerState.idle;
                        button_startStop.Text = "Start Print";
                        button_pauseResume.Text = "Pause Print";

                        textBox_Status.Text = "Print Complete";
                    }
                    // Arduino is ready for the next byte
                    else if(queueData == 6 && currentPrinterState == printerState.sendDrawingBytes)
                    {
                        currentPrinterState = printerState.readyForNextDrawingByte;
                    }
                }

            }

        }

        private static double getScalingFactor(Size image, Size boundingBox)
        {
            double scale = 0;
            if (image.Width != 0)
                scale = (double)boundingBox.Width / (double)image.Width;

            return scale;
        }

        private void button_Upload_Click(object sender, EventArgs e)
        {
            // Provide user feedback
            textBox_Status.Text = "Processing image";

            // Display image
            pictureBox1.ImageLocation = imageLocation;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            // Determine resolution of print
            if (comboBox_Resolution.Text == "Low")
            {
                tileSizeMM = lowResTileSizeMM;
            }
            else if (comboBox_Resolution.Text == "Medium")
            {
                tileSizeMM = mediumResTileSizeMM;

            }
            else if (comboBox_Resolution.Text == "High")
            {
                tileSizeMM = highResTileSizeMM;                
            }

            // Convert image to byte array
            Image imageIn = Image.FromFile(imageLocation);
            byte[] byteArray;
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byteArray = ms.ToArray();

            // Create image class
            currentImage = new image24BitBMP();

            // Parse image
            parseImageArray(byteArray, ref currentImage);

            // Process image array
            processImageData(ref currentImage);

            // Add command bytes to draw queue
            int columnCount = 0;
            foreach(byte tileByte in currentImage.commandBytes)
            {

                // Enqueue tile byte
                switch (tileByte)
                {
                    case 255:
                        drawQueue.Enqueue(254);
                        break;
                    case 0:
                        drawQueue.Enqueue(1);
                        break;
                    default:
                        drawQueue.Enqueue(tileByte);
                        break;
                }
                columnCount++;

                // Enqueue newline character '0' at end of row
                if (columnCount == (int)currentImage.imageWidthTiles)
                {
                    columnCount = 0;
                    drawQueue.Enqueue(0);
                }

            }
                        
            textBox_Status.Text = "Image processing complete";
            
            ////Print out the image
            //int currentTile = 0;
            //foreach (byte currentByte in currentImage.commandBytes)
            //{
            //    richTextBox_debug.AppendText(currentByte.ToString() + " ");
            //    if ((currentTile + 1) % (int)currentImage.imageWidthTiles == 0)
            //    {
            //        richTextBox_debug.AppendText(Environment.NewLine);
            //    }
            //    currentTile++;
            //}
        }

        private void button_pauseResume_Click(object sender, EventArgs e)
        {
            // Pause the print. Can only pause print once all drawing bytes have been sent to the Arduino.
            if (currentPrinterState == printerState.printInProgress)
            {
                currentPrinterState = printerState.printPaused;
                textBox_Status.Text = "Print Paused";
                button_pauseResume.Text = "Resume Print";
                // Send start print command to Arduino
                if (serialPort1.IsOpen)
                {
                    dataByte = 0;
                    serialPort1.Write(new byte[2] { startByte, pausePrintCommand }, 0, 2);
                }
            }
            // Resume the print
            else if (currentPrinterState == printerState.printPaused)
            {
                textBox_Status.Text = "Printing";
                currentPrinterState = printerState.printInProgress;
                button_pauseResume.Text = "Pause Print";
                // Send stop print command to Arduino
                if (serialPort1.IsOpen)
                {
                    dataByte = 0;
                    serialPort1.Write(new byte[2] { startByte, resumePrintCommand }, 0, 2);
                }
            }
        }

        private void button_zeroMotor_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(new byte[2] { startByte, zeroMotorCommand }, 0, 2);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
        }

        private void button_browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            imageLocation = openFileDialog1.FileName;
            textBox_filePath.Text = imageLocation;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox_Resolution.Items.Add("Low");
            comboBox_Resolution.Items.Add("Medium");
            comboBox_Resolution.Items.Add("High");
            comboBox_Resolution.Text = "Low";
        }

        private void timerDraw_Tick(object sender, EventArgs e)
        {
            // Send tile bytes to the Arduino
            if(currentPrinterState == printerState.readyForNextDrawingByte)
            {
                drawQueue.TryDequeue(out dataByte);
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write(new byte[] { startByte, drawCommand, dataByte }, 0, 3);
                }

                // Wait until Arduino is ready for the next byte
                currentPrinterState = printerState.sendDrawingBytes;

                // Drawing byte transfer is complete
                if (drawQueue.IsEmpty)
                {
                    // Send print complete command
                    serialPort1.Write(new byte[] { startByte, stopPrintCommand }, 0, 2);
                    currentPrinterState = printerState.printInProgress;
                    textBox_Status.Text = "Printing";
                }

            }
        }

        private void comboBox_serialPort_Click(object sender, EventArgs e)
        {
            comboBox_serialPort.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox_serialPort.Items.Add(port);
            }
        }

        // Image class
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
            // Declare useful variables
            int numPixels = (int)image.imageHeightPixels * (int)image.imageWidthPixels;        
            int currentTile = 0;
            int currentGrayscalePixel = 0;
            int row = 0, column = 0;


            // Convert image to grayscale. Average the three bytes (R, G, B) for each pixel

            int padding = ((int)image.imageWidthPixels * 3) % 4; // Rows are padded with '0's to make the row count divisible by 4
            if( padding != 0)
            {
                padding = 4 - padding;
            }

            // Grayscale conversion
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


            // Crop image to make image dimensions in pixels divisible by the tile size in pixels
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

            // Create draw byte array
            image.commandBytes = new byte[image.imageHeightTiles * image.imageWidthTiles];
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

        }

        public Form1()
        {
            InitializeComponent();
        }


        private void button_startStop_Click(object sender, EventArgs e)
        {
            // Start print
            if(currentPrinterState == printerState.idle && !drawQueue.IsEmpty)
            {                
                currentPrinterState = printerState.sendDrawingBytes;
                button_startStop.Text = "Stop Print";
                richTextBox_debug.Clear();
                textBox_Status.Text = "Uploading print job";

                // Send start print command to Arduino
                if (serialPort1.IsOpen)
                {
                    // Determine resolution of print
                    if(comboBox_Resolution.Text == "Low")
                    {
                        dataByte = 0;
                    }
                    else if(comboBox_Resolution.Text == "Medium")
                    {
                        dataByte = 1;

                    }
                    else if (comboBox_Resolution.Text == "High")
                    {
                        dataByte = 2;

                    }
                    serialPort1.Write(new byte[] { startByte, startPrintCommand, dataByte }, 0, 3);
                }
                
            }
            // Stop print. Can only stop print once all drawing bytes have been sent to the Arduino.
            else if(currentPrinterState == printerState.printInProgress || currentPrinterState == printerState.printPaused)
            {
                currentPrinterState = printerState.idle;
                button_startStop.Text = "Start Print";
                button_pauseResume.Text = "Pause Print";
                textBox_Status.Text = "Print Stopped";
                // Send stop print command to Arduino
                if (serialPort1.IsOpen)
                {
                    dataByte = 0;
                    serialPort1.Write(new byte[2] { startByte, stopPrintCommand }, 0, 2);
                }
            }

            // Error : user has not uploaded their image
            else if (currentPrinterState == printerState.idle && drawQueue.IsEmpty)
            {
                textBox_Status.Text = "Please upload your image";
            }

            
        }
    }
}
