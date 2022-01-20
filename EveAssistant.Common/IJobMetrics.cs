using System;
using EveAssistant.Common.Device;

namespace EveAssistant.Common
{
    public interface IJobMetrics
    {
        string Action { get; set; }

        string ActionTime { get; set; }

        DateTime TractorIsTooFarAwat { get; set; }

        DateTime CommandRejectedByCloakReasone { get; set; }

        DateTime StartJobTime { get; set; }

        DateTime StartCycleTime { get; set; }

        TimeSpan LastCycleTime { get; set; }

        TimeSpan AverageCycleTime { get; set; }

        int NumberOfCycles { get; set; }

        int PocketNumber { get; set; }

        int MoveToTractorAttempts { get; set; }

        int CyclesWithoutReloadAmmo { get; set; }

        int RepairAction { get; set; }

        int LootItems { get; set; }

        DateTime LastWeaponActivity { get; set; }

        DateTime LastDroneActivity { get; set; }

        DateTime LastEnemyAttack { get; set; }

        DateTime LastTargetLimitExceeded { get; set; }

        DateTime LastWeaponReload { get; set; }

        DateTime TargetNoLongerPresent { get; set; }

        DateTime JumpThroughStarGate { get; set; }
        int ForceReloadAmmo { get; set; }

        int NpcKilledInSession { get; set; }

        int NpcKilledInAction { get; set; }
        DateTime CargoHoldIsFull { get; set; }

        DateTime MinedOre { get; set; }
        DateTime ResourceWasHarvesting { get; set; }
        DateTime StationDenied { get; set; }
        DateTime ReloadMiningForemanBurstCharges { get; set; }

        string Location { get; set; }
        string ShipType { get; set; }

        int LastResourceWasHarvestingInSeconds();
        int LastTractorIsTooFarAwatInSeconds();
        int LastJumpThroughStarGateInSeconds();
        int LastCommandRejectedByCloakReasoneInSeconds();
        int LastTargetLimitExceededInSeconds();
        int LastWeaponReloadInSeconds();
        int TargetNoLongerPresentInSeconds();
        int LastEnemyAttackInSeconds();
        int LastWeaponActivityInSeconds();
        int LastDronesActivityInSeconds();
        void Print(IDevice device);
        void CollectHarvestAction(double reasonExitSeconds);
        int HarvestAverageTime();
    }
}