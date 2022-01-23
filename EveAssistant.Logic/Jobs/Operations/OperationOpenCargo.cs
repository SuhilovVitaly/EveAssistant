﻿using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationOpenCargo
    {
        public static string Name { get; set; } = "[OperationOpenCargo]";

        public static bool Execute(IDevice device, IShip ship)
        {
            var openCargoButtonOnScreen = device.FindObjectInScreen(Types.PanelSelectedOpenCargo, device.Zones.SelectedItem);

            if (openCargoButtonOnScreen.IsFound == false)
            {
                device.Report("Pattern_PanelSelectedOpenCargo_NotFound");
                return false;
            }

            device.Logger($"Open Cargo Button found in selected item panel. Click on {openCargoButtonOnScreen.PositionCenter}");

            TrafficDispatcher.ClickOnPoint(device.IntPtr, openCargoButtonOnScreen.PositionCenterRandom());

            Thread.Sleep(3000);

            device.UnFocusClick();

            return true;
        }
    }
}