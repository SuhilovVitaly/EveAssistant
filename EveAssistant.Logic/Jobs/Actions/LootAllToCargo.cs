using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class LootAllToCargo : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[LootAllToCargo]";

        public LootAllToCargo(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 60;

            ActionExits.Add((CommonActionExits.IsLootCargoOpened, ExitFromAction));
        }

        public void CommandsExecute()
        {
            Thread.Sleep(1000);

            OperationsManager.Execute(OperationTypes.OpenOverviewTab, Device, Ship, Types.OverviewTabLoot);

            Thread.Sleep(1000);

            if (OperationsManager.Execute(OperationTypes.SelectWreck, Device, Ship) == false)
            {
                var wreckOnScreen = Device.FindObjectInScreen(Types.PanelSelectedItemUnLockTarget, Device.Zones.Overview);

                if (wreckOnScreen.IsFound)
                {
                    Device.Logger($"[{Text}] UnLockTarget found. Click on {wreckOnScreen.PositionCenter}");

                    Device.Click(wreckOnScreen.PositionCenterRandom());

                    Thread.Sleep(2000);

                    FinishAction(ExitFromActionReason.RestartKillNpc);
                    return;
                }

                FinishAction(ExitFromActionReason.PatternNotFound);
                return;
            }

            if (OperationsManager.Execute(OperationTypes.OpenCargo, Device, Ship) == false)
            {
                FinishAction(ExitFromActionReason.PatternNotFound);
                return;
            }

            Thread.Sleep(200);
        }

        private void ExitFromAction()
        {
            if (OperationsManager.Execute(OperationTypes.LootAll, Device, Ship) == false)
            {
                FinishAction(ExitFromActionReason.PatternNotFound);
                return;
            }

            FinishAction(ExitFromActionReason.ActionCompleted);
        }

    }
}