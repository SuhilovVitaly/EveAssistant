using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Job;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Jobs.Actions;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs
{
    public class AbissHarvest
    {
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
        private IBasicAction ActionEnableAllActiveModules { get; set; }

        public AbissHarvest(IDevice device, IShip ship)
        {
            Device = device;
            Ship = ship;

            Device.Job = "[AbissHarvest]";

            ActionJobInitialization = new NpcFarmJobInitialization(Device, Ship);
            ActionStationExit = new StationExit(Device, Ship);
            ActionWarpToBookmark = new WarpToBookmark(Device, Ship, Types.BookmarksAbissHarvest);
            ActionEnterToAbiss = new EnterToAbiss(Device, Ship);
            ActionEnableAllActiveModules = new EnableAllActiveModules(Device, Ship);
            ActionWaveNpcKill = new NpcKill(Device, Ship);
            ActionLootObjectKill = new BudkaKill(Device, Ship);
            ActionWaveInitialization = new WaveInitialization(Device, Ship);
            ActionLootAllToCargo = new LootAllToCargo(Device, Ship);
            ActionJumpInGate = new JumpInGate(Device, Ship);
            ActionDockToBookmark = new DockToBookmark(Device, Ship);

            //-----------------------------------------------------------------------------------------------
            ActionJobInitialization.OnResumeAfterComplete += ActionStationExit.Execute;
            //-----------------------------------------------------------------------------------------------
            ActionStationExit.OnResumeAfterComplete += ActionWarpToBookmark.Execute;
            ActionStationExit.OnResumeAfterTimeout += ActionDockToBookmark.Execute;
            //-----------------------------------------------------------------------------------------------
            ActionWarpToBookmark.OnResumeAfterComplete += ActionEnterToAbiss.Execute;
            ActionWarpToBookmark.OnResumeAfterTimeout += ActionDockToBookmark.Execute;
            //-----------------------------------------------------------------------------------------------
            ActionEnterToAbiss.OnResumeAfterComplete += ActionEnableAllActiveModules.Execute;
            ActionEnterToAbiss.OnResumeAfterTimeout += ActionDockToBookmark.Execute;
            //-----------------------------------------------------------------------------------------------
            ActionEnableAllActiveModules.OnResumeAfterComplete += ActionWaveInitialization.Execute;
            ActionEnableAllActiveModules.OnResumeAfterTimeout += ActionDockToBookmark.Execute;
            //-----------------------------------------------------------------------------------------------
            ActionWaveInitialization.OnResumeAfterComplete += ActionWaveNpcKill.Execute;
            ActionWaveInitialization.OnResumeAfterTimeout += ActionWaveNpcKill.Execute;
            //-----------------------------------------------------------------------------------------------
            ActionWaveNpcKill.OnResumeAfterComplete += ActionWaveNpcKill.Execute;
            ActionWaveNpcKill.OnResumeAfterAllNpcAreKilled += ActionLootObjectKill.Execute;
            ActionWaveNpcKill.OnResumeAfterActionCompletedWithAggressiveMode += ActionJumpInGate.Execute;
            ActionWaveNpcKill.OnResumeAfterPatternNotFound += ActionWaveNpcKill.Execute;
            ActionWaveNpcKill.OnResumeAfterObjectInOverviewNotFound += ActionWaveNpcKill.Execute;
            ActionWaveNpcKill.OnResumeAfterCantActivateGate += ActionWaveNpcKill.Execute;
            ActionWaveNpcKill.OnResumeAfterTimeout += ActionWaveNpcKill.Execute;
            //-----------------------------------------------------------------------------------------------
            ActionLootObjectKill.OnResumeAfterComplete += ActionLootAllToCargo.Execute;
            ActionLootObjectKill.OnResumeAfterPatternNotFound += ActionLootAllToCargo.Execute;
            ActionLootObjectKill.OnResumeAfterBudkaNotFound += ActionJumpInGate.Execute;
            ActionLootObjectKill.OnResumeAfterTimeout += ActionJumpInGate.Execute;
            //-----------------------------------------------------------------------------------------------
            ActionLootAllToCargo.OnResumeAfterComplete += ActionJumpInGate.Execute;
            ActionLootAllToCargo.OnResumeAfterLootNotFound += ActionJumpInGate.Execute;
            ActionLootAllToCargo.OnResumeAfterPatternNotFound += ActionJumpInGate.Execute;
            ActionLootAllToCargo.OnResumeAfterRestartKillNpc += ActionWaveInitialization.Execute;
            ActionLootAllToCargo.OnResumeAfterTimeout += ActionJumpInGate.Execute;
            //-----------------------------------------------------------------------------------------------
            ActionJumpInGate.OnResumeAfterComplete += ActionWaveInitialization.Execute;
            ActionJumpInGate.OnResumeAfterShipNotInStation += ActionDockToBookmark.Execute;
            ActionJumpInGate.OnResumeAfterCantActivateGate += ActionWaveInitialization.Execute;
            ActionJumpInGate.OnResumeAfterTimeout += ActionWaveInitialization.Execute;
            //-----------------------------------------------------------------------------------------------
            ActionDockToBookmark.OnResumeAfterComplete += Event_CycleEnd;
            ActionDockToBookmark.OnResumeAfterTimeout += ActionStationExit.Execute;
            //-----------------------------------------------------------------------------------------------

            Device.Metrics.StartJobTime = DateTime.Now;
        }

        private void Event_CycleEnd()
        {
            Device.Metrics.NumberOfCycles = Device.Metrics.NumberOfCycles + 1;
            Device.Metrics.LastCycleTime = DateTime.Now.Subtract(Device.Metrics.StartCycleTime);
            Device.Metrics.AverageCycleTime = new TimeSpan(DateTime.Now.Subtract(Device.Metrics.StartJobTime).Ticks / Device.Metrics.NumberOfCycles); //DateTime.Now.Subtract(Device.Metrics.StartJobTime) / Device.Metrics.NumberOfCycles;

            ActionStationExit.Execute();

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