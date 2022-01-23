using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Ships;
using Timer = System.Timers.Timer;

namespace EveAssistant.Logic.Job.Action
{
    public class BaseAction
    {
        public int TimeoutInSeconds { get; set; }
        public bool IsDebug { get; set; }
        public IDevice Device { get; set; }
        public IShip Ship { get; set; }
        public ExitFromActionReason Reason { get; set; }
        public Stopwatch WorkMetric { get; set; }
        public string Text { get; set; }
        public string Bookmark { get; set; }

        public event Action<BasicActionResult> OnComplete;

        private readonly Timer _crlRefreshMap;

        public List<(Func<BaseAction, ExitFromActionReason, bool, CheckExitResult>, ExitFromActionReason)> ActionExits { get; set; } = new List<(Func<BaseAction, ExitFromActionReason, bool, CheckExitResult>, ExitFromActionReason)>();
        public List<Func<IDevice, IShip, bool, bool>> ActionWait { get; set; } = new List<Func<IDevice, IShip, bool, bool>>();

        private readonly Action<string> _logger;

        public BaseAction(IDevice device, IShip ship, Action<string> logger)
        {
            Device = device;
            Ship = ship;
            _logger = logger;

            _crlRefreshMap = new Timer();
            _crlRefreshMap.Elapsed += Event_Refresh;
            _crlRefreshMap.Interval = 1000;
            _crlRefreshMap.Enabled = true;
        }

        private void Event_Refresh(object sender, ElapsedEventArgs e)
        {
            if (WorkMetric == null) return;

            if (Device is null) return;

            Device.Metrics.ActionTime = $"{WorkMetric.Elapsed.TotalSeconds:0}" + " | " + TimeoutInSeconds;
        }

        public void Logger(string message)
        {
            _logger(Text + " " + message);
        }

        public BasicActionResult Waiting()
        {
            Device.Metrics.Action = Text;

            while (IsResumeAction())
            {
                Logger($"in process. ({Device.Metrics.ActionTime})");

                Thread.Sleep(1000);

                foreach (var operationWait in ActionWait)
                {
                    operationWait(Device, Ship, IsDebug);
                }
            }

            Thread.Sleep(200);

            return new BasicActionResult
            {
                Type = Reason,
                Seconds = WorkMetric.Elapsed.TotalSeconds
            };
        }

        protected bool IsResumeAction()
        {
            foreach (var actionExit in ActionExits)
            {
                var checkExitResult = actionExit.Item1(this, actionExit.Item2, IsDebug);

                if (checkExitResult.IsExitFromAction)
                {
                    Reason = checkExitResult.Reason;
                    return false;
                }
            }

            return true;
        }
    }
}