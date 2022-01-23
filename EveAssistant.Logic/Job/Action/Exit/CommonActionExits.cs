using System;
using EveAssistant.Common.Patterns;

namespace EveAssistant.Logic.Job.Action.Exit
{
    public class CommonActionExits
    {
        public static CheckExitResult IsServerDownTime(IBasicAction action)
        {
            if (action.Device.IsDebug)
                action.Device.Logger("[Exits] Check is server down time.");

            return new CheckExitResult
            {
                IsExitFromAction = Tools.Dates.IsDownTime(DateTime.UtcNow)
            };
        }

        public static CheckExitResult IsLootCargoOpened(IBasicAction action)
        {
            if (action.Device.IsDebug)
                action.Device.Logger("[Exits] Check is loot cargo opened.");

            return new CheckExitResult
            {
                IsExitFromAction = action.Device.FindObjectInScreen(Types.LootAll).IsFound
            };
        }

        public static CheckExitResult IsTargetLost(IBasicAction action)
        {
            if (action.Device.IsDebug)
                action.Device.Logger("[Exits] Check is target lost.");

            return new CheckExitResult
            {
                IsExitFromAction = !action.Device.FindObjectInScreen(Types.PanelSelectedItemUnLockTarget).IsFound
            };
        }

        public static CheckExitResult IsShipNotMovingToGate(IBasicAction action)
        {
            if (action.Device.IsDebug)
                action.Device.Logger("[Exits][IsShipNotMovingToGate] Check is moving to gate.");

            //action.Device.Logger($"[Exits][IsShipNotMovingToGate] Pattern is '{Types.PanelSelectedItemAbissGate}'.");

            var abissGateOnScreen = action.Device.FindObjectInScreen(Types.PanelSelectedItemAbissGate);

            //action.Device.Logger($"[Exits][IsShipNotMovingToGate] Pattern found: '{abissGateOnScreen.IsFound}'.");

            return new CheckExitResult
            {
                IsExitFromAction = !abissGateOnScreen.IsFound
            };
        }

        public static CheckExitResult IsShipInAbissPocket(IBasicAction action)
        {
            if (action.Device.IsDebug)
                action.Device.Logger("[Exits] Check is target lost.");

            return new CheckExitResult
            {
                IsExitFromAction = action.Device.FindObjectInScreen(Types.ShipInAbissPocket).IsFound
            };
        }

        public static CheckExitResult IsShipNotInAbissPocket(IBasicAction action)
        {
            if (action.Device.IsDebug)
                action.Device.Logger("[Exits] Check is target lost.");

            return new CheckExitResult
            {
                IsExitFromAction = action.Device.FindObjectInScreen(Types.ShipNotInAbissPocket).IsFound
            };
        }

        public static CheckExitResult IsShipInAbiss(IBasicAction action)
        {
            if (action.Device.IsDebug)
                action.Device.Logger("[Exits] Check is ship in abiss.");

            return new CheckExitResult
            {
                IsExitFromAction = action.Device.FindObjectInScreen(Types.ShipInAbiss).IsFound
            };
        }

        public static CheckExitResult IsShipInSpace(IBasicAction action)
        {
            if (action.Device.IsDebug) 
                action.Device.Logger("[Exits] Check is ship in space.");

            return new CheckExitResult
            {
                IsExitFromAction = action.Device.FindObjectInScreen(Types.ShipInSpace).IsFound
            };
        }

        public static CheckExitResult IsShipInDock(IBasicAction action)
        {
            if (action.Device.IsDebug)
                action.Device.Logger("[Exits] Check is ship in space.");

            return new CheckExitResult
            {
                IsExitFromAction = action.Device.FindObjectInScreen(Types.ShipInDock).IsFound
            };
        }

        public static CheckExitResult IsShipStopped(IBasicAction action)
        {
            if (action.Device.IsDebug)
                action.Device.Logger("[Exits] Check is ship stopped.");

            return new CheckExitResult
            {
                IsExitFromAction = action.Device.FindObjectInScreen(Types.ShipIsStop).IsFound
            };
        }
    }
}