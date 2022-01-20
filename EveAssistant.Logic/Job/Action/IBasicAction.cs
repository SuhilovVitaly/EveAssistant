using System;
using System.Diagnostics;
using EveAssistant.Common.Device;

namespace EveAssistant.Logic.Job.Action
{
    public interface IBasicAction
    {
        event Action<BasicActionResult, IBasicAction> OnComplete;

        IDevice Device { get; set; }

        int TimeoutInSeconds { get; set; }

        Stopwatch WorkMetric { get; set; }

        string Text { get; set; }

        void Execute();

        void CommandsExecute();
    }
}