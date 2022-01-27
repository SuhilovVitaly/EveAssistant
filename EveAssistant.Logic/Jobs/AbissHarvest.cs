using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Job;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Jobs.Actions;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs
{
    public class AbissHarvest
    {
        private const int TimeoutAfterJumpInGateInMs = 1000;
        private const int TimeoutAfterKillNpcShipInMs = 2000;
        private const int TimeoutAfterStationExitMs = 5000;
        public CancellationTokenSource CancellationToken { get; set; } = new CancellationTokenSource();

        private Stopwatch MetricWorkTime { get; set; }
        public IDevice Device { get; set; }
        public IShip Ship { get; set; }

        private IBasicAction ActionJobInitialization { get; set; }
        private IBasicAction ActionStationExit { get; set; }
        private IBasicAction ActionWarpToBookmark { get; set; }

        private IBasicAction ActionEnterToAbiss { get; set; }

        private IBasicAction ActionWaveInitialization { get; set; }

        private IBasicAction ActionWaveNpcKill { get; set; }

        private IBasicAction ActionLootObjectKill { get; set; }

        private IBasicAction ActionLootAllToCargo { get; set; }

        private IBasicAction ActionJumpInGate { get; set; }

        private IBasicAction ActionDockToBookmark { get; set; }

        public AbissHarvest(IDevice device, IShip ship)
        {
            Device = device;
            Ship = ship;

            Device.Job = "[AbissHarvest]";

            ActionJobInitialization = new NpcFarmJobInitialization(Device, Ship);
            ActionJobInitialization.OnComplete += Event_OnComplete;

            ActionStationExit = new StationExit(Device, Ship);
            ActionStationExit.OnComplete += Event_StationExitOnComplete;

            ActionWarpToBookmark = new WarpToBookmark(Device, Ship, Types.BookmarksAbissHarvest);
            ActionWarpToBookmark.OnComplete += Event_WarpToBookmarkOnComplete;

            ActionEnterToAbiss = new EnterToAbiss(Device, Ship);
            ActionEnterToAbiss.OnComplete += Event_EnterToAbissOnComplete;

            ActionWaveInitialization = new WaveInitialization(Device, Ship);
            ActionWaveInitialization.OnComplete += Event_WaveInitializationOnComplete;

            ActionLootAllToCargo = new LootAllToCargo(Device, Ship);
            ActionLootAllToCargo.OnComplete += Event_LootAllToCargoOnComplete;

            ActionJumpInGate = new JumpInGate(Device, Ship);
            ActionJumpInGate.OnComplete += Event_JumpInGateOnComplete;

            ActionDockToBookmark = new DockToBookmark(Device, Ship);
            ActionDockToBookmark.OnComplete += Event_DockToBookmarkOnComplete;

            Device.Metrics.StartJobTime = DateTime.Now;
        }

        private void AfterActionExecution(BasicActionResult actionResult, IBasicAction action)
        {
            LogWrite($"Event {action.Text} OnComplete - {actionResult.Type}");

            Device.Report($"[{action.Text.Replace("[","").Replace("]", "")}][{actionResult.Type}]");

            Device.Action = "";

            Thread.Sleep(1000);
        }

        private void Event_DockToBookmarkOnComplete(BasicActionResult actionResult, IBasicAction action)
        {
            AfterActionExecution(actionResult, action);

            switch (actionResult.Type)
            {
                case ExitFromActionReason.ActionCompleted:
                    Thread.Sleep(5000);

                    Device.Metrics.NumberOfCycles = Device.Metrics.NumberOfCycles + 1;
                    Device.Metrics.LastCycleTime = DateTime.Now.Subtract(Device.Metrics.StartCycleTime);
                    Device.Metrics.AverageCycleTime = new TimeSpan(DateTime.Now.Subtract(Device.Metrics.StartJobTime).Ticks / Device.Metrics.NumberOfCycles); //DateTime.Now.Subtract(Device.Metrics.StartJobTime) / Device.Metrics.NumberOfCycles;

                    ActionStationExit.Execute();
                    break;

                case ExitFromActionReason.Downtime:
                    Environment.Exit(1);
                    break;

                default:
                    ActionStationExit.Execute();
                    break;
            }
        }

        private void Event_JumpInGateOnComplete(BasicActionResult actionResult, IBasicAction action)
        {
            AfterActionExecution(actionResult, action);

            switch (actionResult.Type)
            {
                case ExitFromActionReason.ActionCompleted:

                    Thread.Sleep(TimeoutAfterJumpInGateInMs);

                    if (CommonActionExits.IsShipNotInAbissPocket(action).IsExitFromAction)
                    {
                        Device.Metrics.PocketNumber = 0;
                        ActionDockToBookmark.Execute();
                        break;
                    }

                    ActionWaveInitialization.Execute();
                    break;

                case ExitFromActionReason.CantActivateGate:
                    Thread.Sleep(3000);
                    ActionWaveInitialization.Execute();
                    break;

                default:
                    Thread.Sleep(3000);
                    ActionWaveInitialization.Execute();
                    break;
            }
        }

        private void Event_LootAllToCargoOnComplete(BasicActionResult actionResult, IBasicAction action)
        {
            AfterActionExecution(actionResult, action);

            switch (actionResult.Type)
            {
                case ExitFromActionReason.ActionCompleted:
                    ActionJumpInGate.Execute();
                    break;

                case ExitFromActionReason.LootNotFound:
                    ActionJumpInGate.Execute();
                    break;

                case ExitFromActionReason.PatternNotFound:
                    ActionJumpInGate.Execute();
                    break;

                case ExitFromActionReason.RestartKillNpc:
                    ActionWaveInitialization.Execute();
                    break;

                default:
                    ActionJumpInGate.Execute();
                    break;
            }
        }

        private void RunNpcKillAction()
        {
            ActionWaveNpcKill = new NpcKill(Device, Ship);
            ActionWaveNpcKill.OnComplete += Event_WaveNpcKillOnComplete;

            LogWrite($"Start kill nps process.");

            ActionWaveNpcKill.Execute();
        }

        private void RunLootObjectKillAction()
        {
            ActionWaveNpcKill = null;
            LogWrite($"Start kill budka process.");
            ActionLootObjectKill = new BudkaKill(Device, Ship);
            ActionLootObjectKill.OnComplete += Event_LootObjectKillOnComplete;

            ActionLootObjectKill.Execute();
        }

        private void Event_LootObjectKillOnComplete(BasicActionResult actionResult, IBasicAction action)
        {
            AfterActionExecution(actionResult, action);

            switch (actionResult.Type)
            {
                case ExitFromActionReason.ActionCompleted:
                    ActionLootAllToCargo.Execute();
                    break;

                case ExitFromActionReason.PatternNotFound:
                    ActionLootAllToCargo.Execute();
                    break;

                case ExitFromActionReason.BudkaNotFound:
                    ActionJumpInGate.Execute();
                    break;

                case ExitFromActionReason.Timeout:
                    ActionJumpInGate.Execute();
                    break;

                default:
                    ActionJumpInGate.Execute();
                    break;
            }
        }

        private void Event_WaveNpcKillOnComplete(BasicActionResult actionResult, IBasicAction action)
        {
            AfterActionExecution(actionResult, action);

            switch (actionResult.Type)
            {
                case ExitFromActionReason.ActionCompletedWithAggressiveMode:
                    LogWrite($"Killed one nps with ActionCompletedWithAggressiveMode.");
                    ActionJumpInGate.Execute();
                    break;

                case ExitFromActionReason.ActionCompleted:
                    LogWrite($"Killed one nps with ActionCompleted.");
                    Thread.Sleep(TimeoutAfterKillNpcShipInMs);
                    RunNpcKillAction();
                    break;
                case ExitFromActionReason.AllNpcAreKilled:
                    LogWrite($"Killed all nps with AllNpcAreKilled.");
                    Thread.Sleep(TimeoutAfterKillNpcShipInMs);
                    RunLootObjectKillAction();
                    break;
                case ExitFromActionReason.Timeout:
                    RunNpcKillAction();
                    break;
                case ExitFromActionReason.PatternNotFound:
                    RunNpcKillAction();
                    break;
                case ExitFromActionReason.ObjectInOverviewNotFound:
                    RunNpcKillAction();
                    break;

                case ExitFromActionReason.CantActivateGate:
                    LogWrite($"CantActivateGate.");
                    Thread.Sleep(TimeoutAfterKillNpcShipInMs);
                    RunNpcKillAction();
                    break;

                default:
                    RunNpcKillAction();
                    break;
            }
        }

        private void Event_WaveInitializationOnComplete(BasicActionResult actionResult, IBasicAction action)
        {
            AfterActionExecution(actionResult, action);

            switch (actionResult.Type)
            {
                case ExitFromActionReason.ActionCompleted:
                    Device.Metrics.PocketNumber++;
                    RunNpcKillAction();
                    break;
                default:
                    RunNpcKillAction();
                    break;
            }
        }

        private void Event_EnterToAbissOnComplete(BasicActionResult actionResult, IBasicAction action)
        {
            AfterActionExecution(actionResult, action);

            switch (actionResult.Type)
            {
                case ExitFromActionReason.ActionCompleted:
                    OperationEnableActiveModules.Execute(Device, Ship);
                    ActionWaveInitialization.Execute();
                    break;
                case ExitFromActionReason.Timeout:
                    ActionDockToBookmark.Execute();
                    break;
                default:
                    ActionDockToBookmark.Execute();
                    break;
            }
        }

        private void Event_WarpToBookmarkOnComplete(BasicActionResult actionResult, IBasicAction action)
        {
            AfterActionExecution(actionResult, action);

            switch (actionResult.Type)
            {
                case ExitFromActionReason.ActionCompleted:
                    ActionEnterToAbiss.Execute();
                    break;
                case ExitFromActionReason.Timeout:
                    Device.UnFocusClick();
                    ActionDockToBookmark.Execute();
                    break;
                default:
                    ActionDockToBookmark.Execute();
                    break;
            }
        }

        private void Event_OnComplete(BasicActionResult actionResult, IBasicAction action)
        {
            AfterActionExecution(actionResult, action);

            switch (actionResult.Type)
            {
                case ExitFromActionReason.ActionCompleted:
                    ActionStationExit.Execute();
                    break;
            }
        }

        private void Event_StationExitOnComplete(BasicActionResult actionResult, IBasicAction action)
        {
            AfterActionExecution(actionResult, action);

            switch (actionResult.Type)
            {
                case ExitFromActionReason.ActionCompleted:
                    Thread.Sleep(TimeoutAfterStationExitMs);
                    ActionWarpToBookmark.Execute();
                    break;
                case ExitFromActionReason.Timeout:
                    ActionDockToBookmark.Execute();
                    break;
                default:
                    ActionDockToBookmark.Execute();
                    break;
            }
        }

        public async Task<AfterJobReport> Execute(CancellationToken token, int miningBookmarkPoint = 0)
        {
            MetricWorkTime = Stopwatch.StartNew();

            Device.Metrics.StartCycleTime = DateTime.Now;

            LogWrite("Start Job: [Category - Harvest Combat Sites] [Type - EmergingConduit]");

            await Task.Run(() => ActionJobInitialization.Execute(), token);

            return new AfterJobReport { DurationInSeconds = MetricWorkTime.Elapsed.Seconds };
        }

        private protected void LogWrite(string message)
        {
            Device.Logger($"{message}");
        }
    }
}