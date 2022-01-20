using System.Diagnostics;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;

namespace EveAssistant.Logic.Jobs.Status
{
    public class AllStates
    {
        public static CheckStatusResult IsFleetCommander(IDevice device)
        {
            var workMetric = Stopwatch.StartNew();

            var result = device.FindObjectInScreen(Types.FleetCommander);

            if (device.IsDebug)
                device.Logger($"[IsFleetCommander] Pattern is '{result.IsFound}'. Work time is {workMetric.Elapsed.TotalSeconds.ToString("N2")} seconds.");

            return new CheckStatusResult
            {
                IsFound = result.IsFound,
                OnScreen = result.PositionCenterRandom()
            };
        }

        public static CheckStatusResult IsFleetWindowOpened(IDevice device)
        {
            var workMetric = Stopwatch.StartNew();

            var result = device.FindObjectInScreen(Types.FleetWindow);

            if (device.IsDebug)
                device.Logger($"[IsFleetWindowOpened] Pattern is '{result.IsFound}'. Work time is {workMetric.Elapsed.TotalSeconds.ToString("N2")} seconds.");

            return new CheckStatusResult
            {
                IsFound = result.IsFound,
                OnScreen = result.PositionCenterRandom()
            };
        }

        public static CheckStatusResult IsShipStopped(IDevice device)
        {
            var workMetric = Stopwatch.StartNew();

            var result = device.FindObjectInScreen(Types.ShipIsStop, device.Zones.SpeedState);

            if(device.IsDebug)
                device.Logger($"[IsShipStopped] Pattern is '{result.IsFound}'. Work time is {workMetric.Elapsed.TotalSeconds.ToString("N2")} seconds.");

            return new CheckStatusResult
            {
                IsFound = result.IsFound,
                OnScreen = result.PositionCenterRandom()
            };
        }
    }
}