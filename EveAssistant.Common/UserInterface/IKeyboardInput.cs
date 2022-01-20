namespace EveAssistant.Common.UserInterface
{
    public interface IKeyboardInput
    {
        void PressCrlShiftKey(string key);

        void PressCrlKey(string key);

        void PressAltKey(string key);

        void PressKey(string key);
    }
}