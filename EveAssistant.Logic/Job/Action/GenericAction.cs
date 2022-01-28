using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Job.Action
{
    public class GenericAction
    {
        public event Action<BasicActionResult, IBasicAction> OnComplete;

        public event System.Action OnResumeAfterComplete;
        public event System.Action OnResumeAfterNone;
        public event System.Action OnResumeAfterUnderAttack;
        public event System.Action OnResumeAfterTargetIsNotLocked;
        public event System.Action OnResumeAfterTargetLost;
        public event System.Action OnResumeAfterCargoIsFull;
        public event System.Action OnResumeAfterShipNotInStation;
        public event System.Action OnResumeAfterOverviewNotFound;
        public event System.Action OnResumeAfterPatternNotFound;
        public event System.Action OnResumeAfterAsteroidIsEnded;
        public event System.Action OnResumeAfterTimeout;
        public event System.Action OnResumeAfterShipLost;
        public event System.Action OnResumeAfterObjectInOverviewNotFound;
        public event System.Action OnResumeAfterBookmarkNotFound;
        public event System.Action OnResumeAfterTargetInRange;
        public event System.Action OnResumeAfterEmergencyEvacuation;
        public event System.Action OnResumeAfterUnknownError;
        public event System.Action OnResumeAfterInProgress;
        public event System.Action OnResumeAfterFleetMemberNotFound;
        public event System.Action OnResumeAfterShipDamaged;
        public event System.Action OnResumeAfterAmmoRunsOut;
        public event System.Action OnResumeAfterAllNpcAreKilled;
        public event System.Action OnResumeAfterHarvestCompleted;
        public event System.Action OnResumeAfterLootNotFound;
        public event System.Action OnResumeAfterDowntime;
        public event System.Action OnResumeAfterCantActivateGate;
        public event System.Action OnResumeAfterActionCompletedWithAggressiveMode;
        public event System.Action OnResumeAfterBudkaNotFound;
        public event System.Action OnResumeAfterRestartKillNpc;
        public event System.Action OnResumeAfterShipDestroyed;

        public IDevice Device { get; set; }
        public IShip Ship { get; set; }
        
        public int TimeoutInSeconds { get; set; }
        
        public Stopwatch WorkMetric { get; set; }
        public ExitFromActionReason Reason { get; set; } = ExitFromActionReason.None;

        public List<(Func<IBasicAction, CheckExitResult> FunctionCheckIsNeedExit, System.Action FunctionExitFromAction)> ActionExits { get; set; } = new List<(Func<IBasicAction, CheckExitResult>, System.Action)>();

        private readonly Timer _crlRefreshMap;

        public GenericAction(IDevice device, IShip ship)
        {
            Device = device;
            Ship = ship;

            ActionExits = new List<(Func<IBasicAction, CheckExitResult>, System.Action)>();

            Logger(" initialization finished.");

            _crlRefreshMap = new Timer();
            _crlRefreshMap.Elapsed += Event_Refresh;
            _crlRefreshMap.Interval = 1000;
            _crlRefreshMap.Enabled = false;
        }

        private bool inAction = false;
        private int counter = 0;

        private void Event_Refresh(object sender, ElapsedEventArgs e)
        {
            if (inAction) return;

            inAction = true;

            Device.Metrics.ActionTime = $"{WorkMetric.Elapsed.TotalSeconds:0}" + " | " + TimeoutInSeconds;

            Logger($"in process. ({Device.Metrics.ActionTime}). Reason is {Reason}");

            if (WorkMetric.Elapsed.TotalSeconds > TimeoutInSeconds)
            {
                FinishAction(ExitFromActionReason.Timeout);
                return;
            }

            // TODO: Remove to separate function or class
            if (Device.Action == "[NpcKill]")
            {
                if (counter > 30)
                {
                    counter = 0;

                    Device.Report("InProcess");
                }

                counter++;
            }

            if (Reason != ExitFromActionReason.None) return;

            IsResumeAction();

            inAction = false;
        }

        public void Execute()
        {
            inAction = false;

            Device.Action = (this as IBasicAction)?.Text;

            Logger((this as IBasicAction)?.Text + "is executed.");

            Reason = ExitFromActionReason.None;

            WorkMetric = Stopwatch.StartNew();

            (this as IBasicAction)?.CommandsExecute();

            if (Reason == ExitFromActionReason.None)
                WaitingActionResults();
        }

        public void FinishAction(ExitFromActionReason exitFromActionReason)
        {
            _crlRefreshMap.Enabled = false;

            Reason = exitFromActionReason;

            Logger($"is finish. Work time is {WorkMetric.Elapsed.TotalSeconds:N2} seconds.");

            switch (Reason)
            {
                case ExitFromActionReason.ActionCompleted:
                    OnResumeAfterComplete?.Invoke();
                    break;
                case ExitFromActionReason.None:
                    OnResumeAfterNone?.Invoke();
                    break;
                case ExitFromActionReason.UnderAttack:
                    OnResumeAfterUnderAttack?.Invoke();
                    break;
                case ExitFromActionReason.AsteroidsIsEnded:
                    OnResumeAfterAsteroidIsEnded?.Invoke();
                    break;
                case ExitFromActionReason.TargetIsNotLocked:
                    OnResumeAfterTargetIsNotLocked?.Invoke();
                    break;
                case ExitFromActionReason.TargetLost:
                    OnResumeAfterTargetLost?.Invoke();
                    break;
                case ExitFromActionReason.CargoIsFull:
                    OnResumeAfterCargoIsFull?.Invoke();
                    break;
                case ExitFromActionReason.ShipNotInStation:
                    OnResumeAfterShipNotInStation?.Invoke();
                    break;
                case ExitFromActionReason.OverviewNotFound:
                    OnResumeAfterOverviewNotFound?.Invoke();
                    break;
                case ExitFromActionReason.PatternNotFound:
                    OnResumeAfterPatternNotFound?.Invoke();
                    break;
                case ExitFromActionReason.AsteroidIsEnded:
                    OnResumeAfterAsteroidIsEnded?.Invoke();
                    break;
                case ExitFromActionReason.Timeout:
                    OnResumeAfterTimeout?.Invoke();
                    break;
                case ExitFromActionReason.ShipLost:
                    OnResumeAfterShipLost?.Invoke();
                    break;
                case ExitFromActionReason.ObjectInOverviewNotFound:
                    OnResumeAfterObjectInOverviewNotFound?.Invoke();
                    break;
                case ExitFromActionReason.BookmarkNotFound:
                    OnResumeAfterBookmarkNotFound?.Invoke();
                    break;
                case ExitFromActionReason.TargetInRange:
                    OnResumeAfterTargetInRange?.Invoke();
                    break;
                case ExitFromActionReason.EmergencyEvacuation:
                    OnResumeAfterEmergencyEvacuation?.Invoke();
                    break;
                case ExitFromActionReason.UnknownError:
                    OnResumeAfterUnknownError?.Invoke();
                    break;
                case ExitFromActionReason.InProgress:
                    OnResumeAfterInProgress?.Invoke();
                    break;
                case ExitFromActionReason.FleetMemberNotFound:
                    OnResumeAfterFleetMemberNotFound?.Invoke();
                    break;
                case ExitFromActionReason.ShipDamaged:
                    OnResumeAfterShipDamaged?.Invoke();
                    break;
                case ExitFromActionReason.AmmoRunsOut:
                    OnResumeAfterAmmoRunsOut?.Invoke();
                    break;
                case ExitFromActionReason.AllNpcAreKilled:
                    OnResumeAfterAllNpcAreKilled?.Invoke();
                    break;
                case ExitFromActionReason.HarvestCompleted:
                    OnResumeAfterHarvestCompleted?.Invoke();
                    break;
                case ExitFromActionReason.LootNotFound:
                    OnResumeAfterLootNotFound?.Invoke();
                    break;
                case ExitFromActionReason.Downtime:
                    OnResumeAfterDowntime?.Invoke();
                    break;
                case ExitFromActionReason.CantActivateGate:
                    OnResumeAfterCantActivateGate?.Invoke();
                    break;
                case ExitFromActionReason.ActionCompletedWithAggressiveMode:
                    OnResumeAfterActionCompletedWithAggressiveMode?.Invoke();
                    break;
                case ExitFromActionReason.BudkaNotFound:
                    OnResumeAfterBudkaNotFound?.Invoke();
                    break;
                case ExitFromActionReason.RestartKillNpc:
                    OnResumeAfterRestartKillNpc?.Invoke();
                    break;
                case ExitFromActionReason.ShipDestroyed:
                    OnResumeAfterShipDestroyed?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //OnComplete?.Invoke(new BasicActionResult { Type = Reason, Seconds = WorkMetric.Elapsed.TotalSeconds }, this as IBasicAction);
        }

        public void WaitingActionResults()
        {
            Device.Metrics.Action = (this as IBasicAction)?.Text;

            _crlRefreshMap.Enabled = true;

            Logger("is started.");
        }

        protected void IsResumeAction()
        {
            foreach (var actionExit in ActionExits)
            {
                var checkExitResult = actionExit.FunctionCheckIsNeedExit(this as IBasicAction);

                if (!checkExitResult.IsExitFromAction) continue;

                actionExit.FunctionExitFromAction();
            }
        }

        public void Logger(string message)
        {
            Device.Logger($"{message}");
        }
    }
}