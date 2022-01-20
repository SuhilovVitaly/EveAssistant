using System;
using System.Drawing;
using EveAssistant.Common;

namespace EveAssistant.Graphic
{
    public class ImageSearchResult : IImageSearchResult
    {
        public bool IsFound { get; set; } = false;

        public Point PositionLeftTop { get; set; }

        public Point PositionCenter { get; set; }

        public Point PositionCenterRandom()
        {
            var randomInt = new Random((int)DateTime.Now.Ticks);

            if (PositionCenter.X < 1 && PositionCenter.Y < 1)
            {
                return new Point(0, 0);
            }

            var clickPositionX = randomInt.Next(PositionCenter.X - 3, PositionCenter.X + 3);
            var clickPositionY = randomInt.Next(PositionCenter.Y - 3, PositionCenter.Y + 3);

            return new Point(clickPositionX, clickPositionY);
        }

        public Bitmap Image { get; set; }

        public Rectangle Zone { get; set; }

        public double ExecuteTimeInMilliseconds { get; set; }

        public void Dispose()
        {
            Image?.Dispose();
        }
    }
}