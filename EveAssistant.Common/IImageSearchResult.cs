using System;
using System.Drawing;

namespace EveAssistant.Common
{
    public interface IImageSearchResult : IDisposable
    {
        bool IsFound { get; set; }
        Point PositionLeftTop { get; set; }
        Point PositionCenter { get; set; }
        Bitmap Image { get; set; }
        Rectangle Zone { get; set; }
        Point PositionCenterRandom();
        void Dispose();
    }
}