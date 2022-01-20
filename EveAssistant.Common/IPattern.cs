using System.Drawing;

namespace EveAssistant.Common
{
    public interface IPattern
    {
        Bitmap Image { get; set; }

        string Name { get; set; }

        Bitmap Screen { get; set; }
    }
}