using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using EveAssistant.Common;
using EveAssistant.Common.Device;

namespace EveAssistant.Graphic
{
    public class ImageDetect
    {
        public static ImageSearchResult SearchImage(Bitmap image, IDevice device, Rectangle searchZone = new Rectangle())
        {
            var stopwatch = Stopwatch.StartNew();

            var patterns = new PatternsCollection
            {
                PatternEntity = new List<IPattern>
                {
                    new Pattern
                    {
                        Image = new ImageFactory().Load(@"Screens/Objects/ElectricalFilament.png"),
                        Name = "Test"
                    }
                }
            };

            var result = GetObjectPositionParallel(patterns, device, searchZone);

            result.ExecuteTimeInMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

            return result;
        }

        public static ImageSearchResult SearchImage(IPatternsCollection patterns, IDevice device, Rectangle searchZone = new Rectangle(), double tolerance = 0.05)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = GetObjectPositionParallel(patterns, device, searchZone, tolerance);

            result.ExecuteTimeInMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

            return result;
        }

        public static ImagesSearchResult SearchAllImages(IPatternsCollection patterns, IDevice device, Rectangle searchZone = new Rectangle())
        {
            var stopwatch = Stopwatch.StartNew();

            var result = new ImagesSearchResult();

            Parallel.ForEach(patterns.PatternEntity, element =>
            {
                try
                {
                    result.SearchResults.AddRange(GetObjectPositionsForOnePattern(element, device, searchZone));
                }
                catch(Exception ex)
                {
                    // ignored
                    var a = ex.Message;
                }
            });

            result.ExecuteTimeInMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

            if (result.SearchResults.Count > 0) result.IsFound = true;

            return result;
        }

        private static IEnumerable<ImageSearchResult> GetObjectPositionsForOnePattern(IPattern pattern, IDevice device, Rectangle searchZone = new Rectangle())
        {
            var searchResults = new List<ImageSearchResult>();

            var buffer = device.GetScreen();

            if (searchZone.Size != Size.Empty)
            {
                buffer = ImageExtract.Extract(device.GetScreen(), searchZone);
                //sourceImage.Save(@"C:\Reports\Device\sourceImage_" + DateTime.Now.Ticks + ".png", ImageFormat.Png);
            }

            Rectangle result;
            do
            {
                var stopwatch = Stopwatch.StartNew();

                result = SearchBitmapIntoBitmapParallel(new PatternsCollection{PatternEntity = new List<IPattern>{pattern}}, buffer);

                if (result != Rectangle.Empty)
                {
                    var searchResult = new ImageSearchResult
                    {
                        IsFound = true,
                        Zone = new Rectangle(searchZone.X + result.X, searchZone.Y + result.Y - device.ScreenModeDelta, result.Width, result.Height),
                        PositionLeftTop = new Point(searchZone.X + result.X + 2, searchZone.Y + result.Y + 2 - device.ScreenModeDelta),
                        PositionCenter = new Point(searchZone.X + result.X + result.Width / 2, searchZone.Y + result.Y + result.Height / 2 - device.ScreenModeDelta),
                        ExecuteTimeInMilliseconds = stopwatch.Elapsed.TotalMilliseconds
                };

                searchResults.Add(searchResult);
                
                var graphics = Graphics.FromImage(buffer);
                graphics.FillRectangle(new SolidBrush(Color.Red), result);
                //buffer.Save(@"C:\Reports\Device\buffer_" + DateTime.Now.Ticks + ".png", ImageFormat.Png);
                }
            } while (result != Rectangle.Empty);

            return searchResults;
        }

        private static ImageSearchResult GetObjectPositionParallel(IPatternsCollection patterns, IDevice device, Rectangle searchZone = new Rectangle(), double tolerance = 0.05)
        {
            try
            {
                Bitmap sourceImage;

                if (searchZone.Size == Size.Empty)
                {
                    sourceImage = device.GetScreen();
                    //sourceImage.Save(@"C:\Reports\Device\sourceImage_" + DateTime.Now.Ticks + ".png", ImageFormat.Png);
                }
                else
                {
                    sourceImage = ImageExtract.Extract(device.GetScreen(), searchZone);
                    //sourceImage.Save(@"C:\Reports\Device\sourceImage_" + DateTime.Now.Ticks + ".png", ImageFormat.Png);
                }

                var result = SearchBitmapIntoBitmapParallel(patterns, sourceImage, tolerance);

                if (result.X > 0 && result.Y > 0)
                {
                    return new ImageSearchResult
                    {
                        IsFound = true,
                        Zone = new Rectangle(searchZone.X + result.X, searchZone.Y + result.Y - device.ScreenModeDelta, result.Width, result.Height),
                        PositionLeftTop = new Point(searchZone.X + result.X + 2, searchZone.Y + result.Y + 2 - device.ScreenModeDelta),
                        PositionCenter = new Point(searchZone.X + result.X + result.Width / 2, searchZone.Y + result.Y + result.Height / 2 - device.ScreenModeDelta)
                    };
                }

            }
            catch(Exception ex)
            {
                // ignore
                var error = ex.Message;
            }

            return new ImageSearchResult { IsFound = false };
        }

        private static Rectangle SearchBitmapIntoBitmapParallel(IPatternsCollection patterns, Bitmap screen, double tolerance = 0.05)
        {
            var rectangle = new Rectangle(0, 0, 0, 0);

            try
            {
                var results = new List<Rectangle>();

                Parallel.ForEach(patterns.PatternEntity, element =>
                {
                    try
                    {
                        var result = GetRectangleFromBitmap(ImageClone.Execute(element.Image).Image, ImageClone.Execute(screen).Image, tolerance);
                        results.Add(result);
                    }
                    catch
                    {
                        // TODO: Add Logger
                    }
                });


                foreach (var result in results)
                {
                    if (result != new Rectangle(0, 0, 0, 0))
                    {
                        return result;
                    }
                }

                return rectangle;
            }
            catch
            {

            }

            return rectangle != new Rectangle(0, 0, 0, 0) ? rectangle : new Rectangle(0, 0, 0, 0);
        }

        private static Rectangle GetRectangleFromBitmap(Bitmap smallBmp, Bitmap bigBmp, double tolerance)
        {
            var smallData = smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            var bigData = bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride;
            int bigStride = bigData.Stride;

            int bigWidth = bigBmp.Width;
            int bigHeight = bigBmp.Height - smallBmp.Height + 1;
            int smallWidth = smallBmp.Width * 3;
            int smallHeight = smallBmp.Height;

            Rectangle location = Rectangle.Empty;
            int margin = Convert.ToInt32(255.0 * tolerance);

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0;
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int smallOffset = smallStride - smallBmp.Width * 3;
                int bigOffset = bigStride - bigBmp.Width * 3;

                bool matchFound = true;

                for (int y = 0; y < bigHeight; y++)
                {
                    for (int x = 0; x < bigWidth; x++)
                    {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        //Look for the small picture.
                        for (int i = 0; i < smallHeight; i++)
                        {
                            int j = 0;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++)
                            {
                                //With tolerance: pSmall value should be between margins.
                                int inf = pBig[0] - margin;
                                int sup = pBig[0] + margin;
                                if (sup < pSmall[0] || inf > pSmall[0])
                                {
                                    matchFound = false;
                                    break;
                                }

                                pBig++;
                                pSmall++;
                            }

                            if (!matchFound) break;

                            //We restore the pointers.
                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            //Next rows of the small and big pictures.
                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        //If match found, we return.
                        if (matchFound)
                        {
                            location.X = x;
                            location.Y = y;
                            location.Width = smallBmp.Width;
                            location.Height = smallBmp.Height;
                            break;
                        }
                        //If no match found, we restore the pointers and continue.
                        else
                        {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }

                    if (matchFound) break;

                    pBig += bigOffset;
                }
            }

            bigBmp.UnlockBits(bigData);
            smallBmp.UnlockBits(smallData);

            return location;
        }

        
    }
}
