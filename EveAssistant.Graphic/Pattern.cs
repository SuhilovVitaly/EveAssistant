using System.Drawing;
using EveAssistant.Common;

namespace EveAssistant.Graphic
{
    public class Pattern: IPattern
    {
        public Bitmap Image { get; set; }

        public string Name { get; set; }

        public Bitmap Screen { get; set; }
    }
}
