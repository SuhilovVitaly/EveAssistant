using System;
using System.Drawing;
using System.Threading;
using EveAssistant.Common.Devices.UserInterface.Mouse;
using EveAssistant.Common.UserInterface;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Devices.UserInterface
{
    public class MouseInput : IMouseInput
    {
        private readonly IntPtr _handle;

        public MouseInput(IntPtr handle)
        {
            _handle = handle;
        }

        public void MouseMoveTo(Point point)
        {
            Thread.Sleep(100);

            TrafficDispatcher.MoveCursorPoint(_handle, point);

            Thread.Sleep(100);
        }

        public void MouseClick(Point point)
        {
            Thread.Sleep(100);

            TrafficDispatcher.ClickOnPoint(_handle, point);

            Thread.Sleep(100);
        }

        public void FastClick(Point point)
        {
            Thread.Sleep(50);

            TrafficDispatcher.ClickOnPoint(_handle, point);

            Thread.Sleep(50);
        }

        public void Click(Point point)
        {
            MouseMoveTo(point);
            MouseClick(point);
        }

        public void ClickCentreScreen()
        {
            var screenSize = ScreenCapture.GetScreenRectangle(_handle);

            var point = Worker.RebuildCenterCoordinatesForRandomPixels(new Rectangle(3, 3, screenSize.Width, screenSize.Height));

            MouseClick(point);
        }

        public void ClickFocusScreen()
        {
            var screenSize = ScreenCapture.GetScreenRectangle(_handle);

            var point = Worker.RebuildCenterCoordinatesForRandomPixels(new Rectangle(20, 20, screenSize.Width, screenSize.Height));

            MouseMoveTo(point);
            MouseClick(point);
        }
    }
}