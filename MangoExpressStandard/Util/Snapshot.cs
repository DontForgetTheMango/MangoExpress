using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using AForge.Imaging;
using MangoExpressStandard.Extension;
using NLog;
using OpenQA.Selenium;

namespace MangoExpressStandard.Util
{
    public class Snapshot
    {
        readonly IWebDriver driver;

        public Snapshot(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void SnapshotWithHighlights(IWebElement element, Tuple<int, int, int> rgb, string path)
        {
            var logger = LogManager.GetCurrentClassLogger();

            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();

            using(var ms = new MemoryStream(ss.AsByteArray))
            using (Bitmap bmp = new Bitmap(ms))
            {
                if (element != null)
                {
                    Rectangle bmpRectangle = new Rectangle(0, 0, bmp.Width, bmp.Height);

                    BitmapData imgData = bmp.LockBits(bmpRectangle, ImageLockMode.ReadWrite, bmp.PixelFormat);

                    int i = 1;
                    foreach (int alpha in new List<int> {255, 127, 50, 20})
                    {
                        Color color = Color.FromArgb(alpha, rgb.Item1, rgb.Item2, rgb.Item3);
                        Rectangle drawRectangle = new Rectangle(
                            element.Location.X - 1,
                            element.Location.Y - 1,
                            element.Size.Width + (i * 2),
                            element.Size.Height + (i * 2)
                        );
                        AForge.Imaging.Drawing.Rectangle(imgData, drawRectangle, color);
                        i++;
                    }
                    bmp.UnlockBits(imgData);
                }

                bmp.Save(path, ImageFormat.Png);
            }
        }

        public string ScrollingSnapshot(string path, string name)
        {
            string imageLocation = $@"{path}\{name}.png";
            var logger = LogManager.GetCurrentClassLogger();
            var js = driver as IJavaScriptExecutor;
            int innerH = int.Parse(js.ExecuteScript("return window.innerHeight").ToString());
            int currentY = 0;
            int lastY = 0;
            int scrollBy = innerH - 100;
            bool atBottom = false;
            var snapshotList = new List<Bitmap>();
            var locationList = new List<int>();

            try
            {
                driver.ScrollToTop();
                System.Threading.Thread.Sleep(250);

                bool isSingleSnapshot = true;
                do
                {
                    var ss = ((ITakesScreenshot)driver).GetScreenshot().AsByteArray;
                    MemoryStream memoryStream = new MemoryStream(ss);

                    Bitmap tempBitmap;

                    if (isSingleSnapshot)
                    {
                        tempBitmap = new Bitmap(memoryStream);
                        locationList.Add(currentY);
                    }
                    else
                    {
                        using (Bitmap bmp = new Bitmap(memoryStream))
                        {
                            tempBitmap = CropBitmap(bmp, 0, bmp.Size.Height - scrollBy - 20, bmp.Size.Width, scrollBy);
                            locationList.Add(currentY + bmp.Size.Height - scrollBy - 20);
                        }
                    }

                    snapshotList.Add(tempBitmap);
                    memoryStream.Dispose();

                    js.ExecuteScript($"window.scrollBy(0, {scrollBy});");
                    currentY = int.Parse(js.ExecuteScript("return window.scrollY").ToString());

                    if (currentY == lastY)
                        atBottom = true;
                    else
                        isSingleSnapshot = false;

                    lastY = currentY;
                } while (atBottom != true);

                if (!isSingleSnapshot)
                {
                    var ss = ((ITakesScreenshot)driver).GetScreenshot().AsByteArray;

                    using (MemoryStream memorystream = new MemoryStream(ss))
                    using (Bitmap t = new Bitmap(memorystream))
                    {
                        Bitmap temp = CropBitmap(t, 0, t.Size.Height - 20, t.Size.Width, 20);
                        snapshotList.Add(temp);
                        locationList.Add(currentY + t.Size.Height - 20);
                    }
                }

                int w = snapshotList[0].Width;
                int h = locationList.Last() + snapshotList.Last().Height;

                using (Bitmap bitmap = new Bitmap(w, h))
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    var zipped = snapshotList.Zip(locationList, (bmp, yLocation) => new { bmp, yLocation });
                    foreach (var items in zipped)
                    {
                        g.DrawImage(items.bmp, 0, items.yLocation);
                    }
                    bitmap.Save(imageLocation, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"image capture exception:\r\n{ex.Message}");
            }
            finally
            {
                snapshotList.ForEach(item => item.Dispose());
            }

            return imageLocation;
        }

        private Bitmap CropBitmap(Bitmap bitmap, int cropX, int cropY, int cropW, int cropH)
        {
            Rectangle rectangle = new Rectangle(cropX, cropY, cropW, cropH);
            Bitmap cropped = bitmap.Clone(rectangle, bitmap.PixelFormat);
            return cropped;
        }
    }
}
