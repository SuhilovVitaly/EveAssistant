using System;
using EveAssistant.Common;
using EveAssistant.Common.Device;

namespace EveAssistant.Logic.Job
{
    public class JobMetrics: IJobMetrics
    {
        public string Action { get; set; }

        public string ActionTime { get; set; }

        public DateTime TractorIsTooFarAwat { get; set; }

        public DateTime CommandRejectedByCloakReasone { get; set; }

        public DateTime StartJobTime { get; set; }

        public DateTime StartCycleTime { get; set; }

        public TimeSpan LastCycleTime { get; set; }

        public TimeSpan AverageCycleTime { get; set; }

        public int NumberOfCycles { get; set; }
        public int PocketNumber { get; set; }

        public int MoveToTractorAttempts { get; set; }

        public int CyclesWithoutReloadAmmo { get; set; }

        public int RepairAction { get; set; }

        public int LootItems { get; set; }

        public DateTime LastWeaponActivity { get; set; }

        public DateTime LastDroneActivity { get; set; }

        public DateTime LastEnemyAttack { get; set; }

        public DateTime LastTargetLimitExceeded { get; set; }

        public DateTime LastWeaponReload { get; set; }

        public DateTime TargetNoLongerPresent { get; set; }

        public DateTime JumpThroughStarGate { get; set; }
        public int ForceReloadAmmo { get; set; }

        public int NpcKilledInSession { get; set; }

        public int NpcKilledInAction { get; set; }
        public DateTime CargoHoldIsFull { get; set; }

        public DateTime MinedOre { get; set; }
        public DateTime ResourceWasHarvesting { get; set; }
        public DateTime StationDenied { get; set; }
        public DateTime ReloadMiningForemanBurstCharges { get; set; }

        public string Location { get; set; }
        public string ShipType { get; set; }

        public int LastResourceWasHarvestingInSeconds()
        {
            var result = (int)(DateTime.Now - ResourceWasHarvesting).TotalSeconds;

            return result < 0 ? 0 : result;
        }

        public int LastTractorIsTooFarAwatInSeconds()
        {
            var result = (int)(DateTime.Now - TractorIsTooFarAwat).TotalSeconds;

            if (result < 0) return 1000;

            return result;
        }

        public int LastJumpThroughStarGateInSeconds()
        {
            var result = (int)(DateTime.Now - JumpThroughStarGate).TotalSeconds;

            if (result < 0) return 1000;

            return result;
        }

        public int LastCommandRejectedByCloakReasoneInSeconds()
        {
            var result = (int)(DateTime.Now - CommandRejectedByCloakReasone).TotalSeconds;

            if (result < 0) return 1000;

            return result;
        }

        public int LastTargetLimitExceededInSeconds()
        {
            var result = (int)(DateTime.Now - LastTargetLimitExceeded).TotalSeconds;

            if (result < 0) return 0;

            return result;
        }

        public int LastWeaponReloadInSeconds()
        {
            var result = (int)(DateTime.Now - LastWeaponReload).TotalSeconds;

            return result < 0 ? 0 : result;
        }

        public int TargetNoLongerPresentInSeconds()
        {
            var result = (int)(DateTime.Now - TargetNoLongerPresent).TotalSeconds;

            return result < 0 ? 0 : result;
        }

        public int LastEnemyAttackInSeconds()
        {
            var result = (int)(DateTime.Now - LastEnemyAttack).TotalSeconds;

            return result < 0 ? 0 : result;
        }

        public int LastWeaponActivityInSeconds()
        {
            var result = (int)(DateTime.Now - LastWeaponActivity).TotalSeconds;

            return result < 0 ? 10000 : result;
        }

        public int LastDronesActivityInSeconds()
        {
            var result = (int)(DateTime.Now - LastDroneActivity).TotalSeconds;

            return result < 0 ? 10000 : result;
        }



        public void Print(IDevice device)
        {
            device.Logger("---------------------------------");
            device.Logger("TargetNoLongerPresent " + TargetNoLongerPresentInSeconds());
            device.Logger("LastWeaponReload " + LastWeaponReloadInSeconds());
            device.Logger("LastEnemyAttack " + LastEnemyAttackInSeconds());
            device.Logger("LastTargetLimitExceeded " + LastTargetLimitExceededInSeconds());
            device.Logger("LastWeaponActivity " + LastWeaponActivityInSeconds());
            device.Logger("---------------------------------");
        }

        public void CollectHarvestAction(double reasonExitSeconds)
        {
            harvestAverageTime = (int)(harvestAverageTime + reasonExitSeconds) / 2;
        }

        private int harvestAverageTime;

        public int HarvestAverageTime()
        {
            return harvestAverageTime;
        }
    }
}