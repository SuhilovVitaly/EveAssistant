using System;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public static class OperationsManager
    {
        public static bool Execute(OperationTypes operation, IDevice device, IShip ship, string pattern = "")
        {
            switch (operation)
            {
                case OperationTypes.ApproachToObject:
                    return new OperationApproachToObject().Execute(device, ship, pattern);
                case OperationTypes.ClearBackground:
                    return new OperationClearBackground().Execute(device, ship);
                case OperationTypes.DockToBookmark:
                    return new OperationDockToBookmark().Execute(device, ship, pattern);
                case OperationTypes.EnableActiveModules:
                    return new OperationEnableActiveModules().Execute(device, ship);
                case OperationTypes.EnterToTrace:
                    return new OperationEnterToTrace().Execute(device, ship);
                case OperationTypes.ExitStation:
                    return new OperationExitStation().Execute(device, ship);
                case OperationTypes.FormFleet:
                    return new OperationFormFleet().Execute(device, ship);
                case OperationTypes.ItemHangarAll:
                    return new OperationItemHangarAll().Execute(device, ship);
                case OperationTypes.ItemHangarFilterFilaments:
                    return new OperationItemHangarFilterFilaments().Execute(device, ship);
                case OperationTypes.JumpToAbissGate:
                    return new OperationJumpToAbissGate().Execute(device, ship);
                case OperationTypes.LootAll:
                    return new OperationLootAll().Execute(device, ship);
                case OperationTypes.MoveFilamentToCargo:
                    return new OperationMoveFilamentToCargo().Execute(device, ship);
                case OperationTypes.MoveLootToHangar:
                    return new OperationMoveLootToHangar().Execute(device, ship);
                case OperationTypes.OpenCargo:
                    return new OperationOpenCargo().Execute(device, ship);
                case OperationTypes.OpenFire:
                    return new OperationOpenFire().Execute(device, ship);
                case OperationTypes.OpenItemHangarFilters:
                    return new OperationOpenItemHangarFilters().Execute(device, ship);
                case OperationTypes.OpenOverviewTab:
                    return new OperationOpenOverviewTab().Execute(device, ship, pattern);
                case OperationTypes.OpenShipCargo:
                    return new OperationOpenShipCargo().Execute(device, ship);
                case OperationTypes.OrbitObject:
                    return new OperationOrbitObject().Execute(device, ship, pattern);
                case OperationTypes.SelectNpc:
                    return new OperationSelectNpc().Execute(device, ship);
                case OperationTypes.SelectWreck:
                    return new OperationSelectWreck().Execute(device, ship);
                case OperationTypes.UnlockTarget:
                    return new OperationUnlockTarget().Execute(device, ship);
                case OperationTypes.UseTranquilFilament:
                    return new OperationUseTranquilFilament().Execute(device, ship);
                case OperationTypes.WarpToBookmark:
                    return new OperationWarpToBookmark().Execute(device, ship, pattern);
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }

            return false;
        }
    }
}
