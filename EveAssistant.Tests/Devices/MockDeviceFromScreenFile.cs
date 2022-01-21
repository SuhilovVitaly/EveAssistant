using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using EveAssistant.Common.Device;
using EveAssistant.Graphic;

namespace EveAssistant.Tests.Devices
{
    public class MockDeviceFromScreenFile: Logic.Devices.BaseDevice, IDevice
    {
        private readonly string _screen;
        private readonly Bitmap _bitmap;

        public MockDeviceFromScreenFile(string screenFile)
        {
            _screen = screenFile;

            _bitmap = CreateMockScreen();

            PatternFactory = new Patterns(Global.PatternsClientPath);
        }

        public Bitmap GetScreen()
        {
            return ImageClone.Execute(_bitmap).Image;
        }

        private Bitmap CreateMockScreen()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _screen);
            var pathForCopy = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Screens", DateTime.Now.Ticks.ToString());

            File.Copy(path, pathForCopy, true);

            var orig = (Bitmap)Image.FromFile(pathForCopy);

            var clone = new Bitmap(orig.Width, orig.Height, PixelFormat.Format32bppPArgb);
            using var gr = Graphics.FromImage(clone);
            gr.DrawImage(orig, new Rectangle(0, 0, clone.Width, clone.Height));
            gr.Dispose();

            return clone;
        }

        public void Logger(string value)
        {

        }

        public void ContextMenuClick(string typeOnScreen, string typeOnMenu)
        {
            
        }

        public void Click(Point point)
        {
            throw new NotImplementedException();
        }

        public void Click(string typeOnScreen)
        {
            throw new NotImplementedException();
        }

        public void UnFocusClick()
        {
            throw new NotImplementedException();
        }
    }
}