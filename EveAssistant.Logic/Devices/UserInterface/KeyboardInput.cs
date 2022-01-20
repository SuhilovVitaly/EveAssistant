using System;
using EveAssistant.Common.UserInterface;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Common.Devices.UserInterface
{
    public class KeyboardInput : IKeyboardInput
    {
        private readonly IntPtr _handle;

        public KeyboardInput(IntPtr handle)
        {
            _handle = handle;
        }

        public void PressCrlShiftKey(string key)
        {
            TrafficDispatcher.EmulationPressCrlShiftKey(_handle, key);
        }

        public void PressCrlKey(string key)
        {
            TrafficDispatcher.EmulationPressCrlKey(_handle, key);
        }

        public void PressAltKey(string key)
        {
            TrafficDispatcher.EmulationPressAltKey(_handle, key);
        }

        public void PressKey(string key)
        {
            // TODO: Check Crt+ Alt+ Shift+
            if (key.Contains("Crt+Shift+"))
            {
                TrafficDispatcher.EmulationPressCrlShiftKey(_handle, key.Replace("Crt+Shift+", ""));
                return;
            }

            if (key.Contains("Crt+"))
            {
                TrafficDispatcher.EmulationPressCrlKey(_handle, key.Replace("Crt+", ""));
                return;
            }

            if (key.Contains("Crl+"))
            {
                TrafficDispatcher.EmulationPressCrlKey(_handle, key.Replace("Crl+", ""));
                return;
            }

            if (key.Contains("Alt+"))
            {
                TrafficDispatcher.EmulationPressAltKey(_handle, key.Replace("Alt+", ""));
                return;
            }

            if (key.Contains("Shift+"))
            {
                TrafficDispatcher.EmulationPressShiftKey(_handle, key.Replace("Shift+", ""));
                return;
            }

            TrafficDispatcher.EmulationPressKey(_handle, key);
        }

    }
}