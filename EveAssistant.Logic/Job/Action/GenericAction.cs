using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Job.Action
{
    public class GenericAction
    {
        public event Action<BasicActionResult, IBasicAction> OnComplete;

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

        private void Event_Refresh(object sender, ElapsedEventArgs e)
        {
            if (inAction) return;

            inAction = true;

            Device.Metrics.ActionTime = $"{WorkMetric.Elapsed.TotalSeconds:0}" + " | " + TimeoutInSeconds;

            Logger($"in process. ({Device.Metrics.ActionTime}). Reason is {Reason}");

            if (WorkMetric.Elapsed.TotalSeconds > TimeoutInSeconds)
            {
                ScreenCapture.ScreenShot(Device.IntPtr, "Timeout", Device.Logger);
                FinishAction(ExitFromActionReason.Timeout);
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

            OnComplete?.Invoke(new BasicActionResult { Type = Reason, Seconds = WorkMetric.Elapsed.TotalSeconds }, this as IBasicAction);
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