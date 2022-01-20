using System.Diagnostics;
using System.Drawing;
using EveAssistant.Common;


namespace EveAssistant.Graphic
{
    public static class ImageClone
    {
        private static readonly object LockCloneImage = new object();

        public static ImageSearchResult Execute(Bitmap source)
        {
            Bitmap result;

            var stopwatch = Stopwatch.StartNew();

            lock (LockCloneImage)
            {
                result = source.Clone(new Rectangle(0, 0, source.Width, source.Height), source.PixelFormat);
            }

            return new ImageSearchResult
            {
                Image = result,
                ExecuteTimeInMilliseconds = stopwatch.Elapsed.TotalMilliseconds
            };
        }
    }
}