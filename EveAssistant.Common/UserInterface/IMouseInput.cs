using System.Drawing;

namespace EveAssistant.Common.UserInterface
{
    public interface IMouseInput
    {
        void MouseMoveTo(Point point);
        void MouseClick(Point point);
        void FastClick(Point point);
        void Click(Point point);
        void ClickCentreScreen();
        void ClickFocusScreen();
    }
}