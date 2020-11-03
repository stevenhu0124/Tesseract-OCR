
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Tesseract;


namespace Tesseract_OCR.Controller
{
    public class CaputreScreen
    {
        private readonly Rectangle captureScreen;
        private readonly Rectangle caputreRange;

        public CaputreScreen(int screenWidth, int screenHeight)
        {
            captureScreen = new Rectangle() { Width = screenWidth, Height = screenHeight };
            caputreRange = new Rectangle(new Point(screenWidth / 4, screenHeight / 4), new Size(screenWidth / 2, screenHeight / 2));
        }

        public string ScanCoordinate()
        {
            using Bitmap fullScreen = new Bitmap(captureScreen.Width, captureScreen.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(fullScreen))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, captureScreen.Size);
            }

            MemoryStream ms = new MemoryStream();
            using Bitmap captureImage = CaptureImage(fullScreen, caputreRange);

            captureImage.Save(AppDomain.CurrentDomain.BaseDirectory + "Test.bmp");
            captureImage.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);


            return Convert(ms.ToArray());
        }

        private Bitmap CaptureImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height, PixelFormat.Format32bppArgb);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            return bitmap;
        }

        private string Convert(byte[] imageBytes)
        {
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadTiffFromMemory(imageBytes))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();
                            }
                            return text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Format("{0}, {1},", "Unexpected Error: " + ex.Message, ex.ToString());
            }
        }
    }
}
