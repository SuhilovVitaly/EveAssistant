using System;
using System.Drawing;
using System.Drawing.Imaging;
using EveAssistant.Common.WinAPI;

namespace EveAssistant.Logic.Tools
{
    public class ScreenCapture
    {
        public static Bitmap GetScreen(IntPtr handle)
        {
            return (Bitmap)CaptureWindow(handle);
        }

        public static void ScreenShot(IntPtr handle, string prefix, Action<string> logger)
        {
            var screen = GetScreen(handle);

            var filename = "Reports/" + prefix + "_" + DateTime.Now.Ticks + @".png";

            logger("[ScreenCaptureTools - SaveScreenShot] True. Save screen to " + Environment.CurrentDirectory + @"\" + filename.Replace(@"/", @"\"));

            screen.Save(filename, ImageFormat.Png);
        }

        public static Size GetScreenRectangle(IntPtr handle)
        {
            return CaptureWindow(handle).Size;
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public static Image CaptureWindow(IntPtr handle)
        {
            try
            {
                // get te hDC of the target window
                var hdcSrc = User32.GetWindowDC(handle);
                // get the size
                var windowRect = new User32.Rect();
                User32.GetWindowRect(handle, ref windowRect);
                var width = windowRect.right - windowRect.left;
                var height = windowRect.bottom - windowRect.top;
                // create a device context we can copy to
                var hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
                // create a bitmap we can copy it to,
                // using GetDeviceCaps to get the width/height
                var hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
                // select the bitmap object
                var hOld = GDI32.SelectObject(hdcDest, hBitmap);
                // bitblt over
                GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
                // restore selection
                GDI32.SelectObject(hdcDest, hOld);
                // clean up 
                GDI32.DeleteDC(hdcDest);
                User32.ReleaseDC(handle, hdcSrc);
                // get a .NET image object for it
                Image img = Image.FromHbitmap(hBitmap);
                // free up the Bitmap object
                GDI32.DeleteObject(hBitmap);
                return img;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}