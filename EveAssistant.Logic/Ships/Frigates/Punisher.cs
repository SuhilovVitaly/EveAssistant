using System.Collections.Generic;

namespace EveAssistant.Logic.Ships.Frigates
{
    public class Punisher : IShip
    {
        public string Name { get; set; } = "Punisher";
        public List<string> ActiveModules { get; set; } = new List<string> { "F3", "F5", "F7" };
        public List<string> ActiveDefenseModules { get; set; }
        public List<string> FriendModules { get; set; }
        public ShipModulesGroup TargetAuxiliaryModules { get; set; }
        public List<string> WeaponModules { get; set; } = new List<string> { "F1"};
        public List<string> MiningLasersModules { get; set; }
        public List<string> MiningForemanBursts { get; set; }
        public int MiningLasers { get; set; }
        public string MiningLaserPattern { get; set; }
        public int MaximumLockedTargets { get; set; }
        public bool IsForceStopLasers { get; set; }
        public bool IsUseMinerDrones { get; set; }
        public bool IsUseCombatDrones { get; set; }
        public bool IsNeedMoveToAsteroid { get; set; }
        public bool IsNeedMoveToTractor { get; set; }
        public string AmmoType { get; set; }
        public bool IsFightWithEnemyShips { get; set; }
        public bool IsReloadingAmmo(int secondsAfterLastReload)
        {
            throw new System.NotImplementedException();
        }

        public int PitStopCicle { get; set; }
        public int Range { get; set; }
        public int AdditionalHarvestTime { get; set; }
        public int AdditionalWarpTime { get; set; }
    }
}