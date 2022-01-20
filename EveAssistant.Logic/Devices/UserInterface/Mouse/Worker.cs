using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace EveAssistant.Common.Devices.UserInterface.Mouse
{
    public class Worker
    {
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        public static Point RebuildCenterCoordinatesForRandomPixels(Rectangle position)
        {
            var randomInt = new Random((int)DateTime.Now.Ticks);

            if (position.X < 1 && position.Y < 1)
            {
                return new Point(0, 0);
            }

            var centerPositionX = position.X + position.Width / 2;
            var centerPositionY = position.Y + position.Height / 2;

            var clickPositionX = randomInt.Next(centerPositionX - 3, centerPositionX + 3);
            var clickPositionY = randomInt.Next(centerPositionY - 3, centerPositionY + 3);

            return new Point(clickPositionX, clickPositionY);
        }

        public static Point RebuildCenterCoordinatesForRandomPixels(Point position)
        {
            var randomInt = new Random((int)DateTime.Now.Ticks);

            if (position.X < 1 && position.Y < 1)
            {
                return new Point(0, 0);
            }

            var centerPositionX = position.X;
            var centerPositionY = position.Y;

            var clickPositionX = randomInt.Next(centerPositionX - 3, centerPositionX + 3);
            var clickPositionY = randomInt.Next(centerPositionY - 3, centerPositionY + 3);

            return new Point(clickPositionX, clickPositionY);
        }
    }
}