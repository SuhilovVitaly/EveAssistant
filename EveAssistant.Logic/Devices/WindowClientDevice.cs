using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using EveAssistant.Common;
using EveAssistant.Common.Configuration;
using EveAssistant.Common.Device;
using EveAssistant.Common.Devices.UserInterface;
using EveAssistant.Logic.Devices.UserInterface;
using EveAssistant.Logic.GameClient;
using EveAssistant.Logic.Job;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Devices
{
    public class WindowClientDevice : BaseDevice, IDevice
    {
        private readonly Action<string> _logger;
        public LogHarvester LogEvents { get; set; }

        public WindowClientDevice(IntPtr handle, Action<string> logger, Shortcuts shortcuts, IPatterns patternFactory ,string pilot = "None")
        {
            IntPtr = handle;
            Shortcuts = shortcuts;
            PatternFactory = patternFactory;
            _logger = logger;

            Pilot = pilot.Trim();

            Mouse = new MouseInput(handle);
            Keyboard = new KeyboardInput(handle);

            LogEvents = new LogHarvester(this);
            Metrics = new JobMetrics();

            Zones = new ScreenZones();

            Task.Run(LogEvents.Execute);
        }

        public void ContextMenuClick(string typeOnScreen, string typeOnMenu)
        {
            var result = FindObjectInScreen(typeOnScreen);
            
            TrafficDispatcher.ContextMenuClick(this, result.PositionLeftTop, typeOnMenu);
        }

        public void Click(Point point)
        {
            Mouse.Click(point);

            Thread.Sleep(200);
        }

        public void ClickAndReturn(Point point, string message = "")
        {
            ClickAndReturn(point);

            if (string.IsNullOrEmpty(message) == false)
            {
                Logger(message);
            }
        }

        public void ClickAndReturn(Point point)
        {
            Mouse.Click(point);

            UnFocusClick();
        }


        public void Click(string typeOnScreen)
        {
            Mouse.Click(FindObjectInScreen(typeOnScreen).PositionCenterRandom());
        }

        public void UnFocusClick()
        {
            Mouse.Click(Zones.SafeUnFocusPoint);
        }

        

        public void Logger(string message)
        {
            _logger($"[{Pilot}] {Job} {Action} " + message);
        }

        public Bitmap GetScreen()
        {
            return ScreenCapture.GetScreen(IntPtr);
        }
    }
}