using System;
using System.Diagnostics;
using EveAssistant.Common.Device;

namespace EveAssistant.Logic.Job.Action
{
    public interface IBasicAction
    {
        event Action<BasicActionResult, IBasicAction> OnComplete;

        event System.Action OnResumeAfterComplete;
        event System.Action OnResumeAfterNone;
        event System.Action OnResumeAfterUnderAttack;
        event System.Action OnResumeAfterTargetIsNotLocked;
        event System.Action OnResumeAfterTargetLost;
        event System.Action OnResumeAfterCargoIsFull;
        event System.Action OnResumeAfterShipNotInStation;
        event System.Action OnResumeAfterOverviewNotFound;
        event System.Action OnResumeAfterPatternNotFound;
        event System.Action OnResumeAfterAsteroidIsEnded;
        event System.Action OnResumeAfterTimeout;
        event System.Action OnResumeAfterShipLost;
        event System.Action OnResumeAfterObjectInOverviewNotFound;
        event System.Action OnResumeAfterBookmarkNotFound;
        event System.Action OnResumeAfterTargetInRange;
        event System.Action OnResumeAfterEmergencyEvacuation;
        event System.Action OnResumeAfterUnknownError;
        event System.Action OnResumeAfterInProgress;
        event System.Action OnResumeAfterFleetMemberNotFound;
        event System.Action OnResumeAfterShipDamaged;
        event System.Action OnResumeAfterAmmoRunsOut;
        event System.Action OnResumeAfterAllNpcAreKilled;
        event System.Action OnResumeAfterHarvestCompleted;
        event System.Action OnResumeAfterLootNotFound;
        event System.Action OnResumeAfterDowntime;
        event System.Action OnResumeAfterCantActivateGate;
        event System.Action OnResumeAfterActionCompletedWithAggressiveMode;
        event System.Action OnResumeAfterBudkaNotFound;
        event System.Action OnResumeAfterRestartKillNpc;
        event System.Action OnResumeAfterShipDestroyed;

        IDevice Device { get; set; }

        int TimeoutInSeconds { get; set; }

        Stopwatch WorkMetric { get; set; }

        string Text { get; set; }

        void Execute();

        void CommandsExecute();
    }
}