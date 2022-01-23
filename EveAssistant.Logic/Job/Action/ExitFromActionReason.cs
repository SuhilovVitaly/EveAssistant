﻿namespace EveAssistant.Logic.Job.Action
{
    public enum ExitFromActionReason
    {
        ActionCompleted,
        None,
        UnderAttack,
        AsteroidsIsEnded,
        TargetIsNotLocked,
        TargetLost,
        CargoIsFull,
        ShipNotInStation,
        OverviewNotFound,
        PatternNotFound,
        AsteroidIsEnded,
        Timeout,
        ShipLost,
        ObjectInOverviewNotFound,
        BookmarkNotFound,
        TargetInRange,
        EmergencyEvacuation,
        UnknownError,
        InProgress,
        FleetMemberNotFound,
        ShipDamaged,
        AmmoRunsOut,
        AllNpcAreKilled,
        HarvestCompleted,
        LootNotFound,
        Downtime,
        ActionCompletedWithAggressiveMode,
        BudkaNotFound
    }
}