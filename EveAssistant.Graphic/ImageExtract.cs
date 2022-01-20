using System.Drawing;

namespace EveAssistant.Graphic
{
    public class ImageExtract
    {
        public static Bitmap Extract(Bitmap screen, Rectangle section)
        {
            try
            {
                var bmp = new Bitmap(section.Width, section.Height);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(screen, 0, 0, section, GraphicsUnit.Pixel);
                }
                return bmp;
            }
            catch
            {
                return new Bitmap(10, 10);
            }
        }
    }
}