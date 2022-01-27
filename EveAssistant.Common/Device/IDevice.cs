using System;
using System.Drawing;
using EveAssistant.Common.Configuration;
using EveAssistant.Common.Device.Events;
using EveAssistant.Common.UserInterface;

namespace EveAssistant.Common.Device
{
    public interface IDevice
    {
        int ScreenModeDelta { get; set; }
        Bitmap GetScreen();
        IntPtr IntPtr { get; set; }
        string Pilot { get; set; }
        string LogFileName { get; set; }
        void Logger(string value);
        bool IsDebug { get; set; }

        string Action { get; set; }

        string Job { get; set; }

        IJobMetrics Metrics { get; set; }
        IDeviceEvents Events { get; set; }

        IMouseInput Mouse { get; set; }

        IKeyboardInput Keyboard { get; set; }

        Shortcuts Shortcuts { get; set; }

        ScreenZones Zones { get; set; }

        IPatterns PatternFactory { get; set; }

        IImageSearchResult FindObjectInScreen(string entity);

        IImageSearchResult FindObjectInScreen(string entity, Rectangle zone);

        IImageSearchResult FindObjectInScreen(string entity, double tolerance);

        void ContextMenuClick(string typeOnScreen, string typeOnMenu);

        void Click(Point point);

        void ClickAndReturn(Point point, string message);

        void ClickAndReturn(Point point);

        void Click(string typeOnScreen);

        void UnFocusClick();

        void Report(string file);

        void Report(string file, string message);
    }
}