using System.Collections.Generic;

namespace EveAssistant.Logic.Ships
{
    public interface IShip
    {
        string Name { get; set; }

        List<string> ActiveModules { get; set; }

        List<string> ActiveDefenseModules { get; set; }

        List<string> FriendModules { get; set; }

        ShipModulesGroup TargetAuxiliaryModules { get; set; }

        List<string> WeaponModules { get; set; }

        List<string> MiningLasersModules { get; set; }

        List<string> MiningForemanBursts { get; set; }


        int MiningLasers { get; set; }

        string MiningLaserPattern { get; set; }

        int MaximumLockedTargets { get; set; }

        bool IsForceStopLasers { get; set; }

        bool IsUseMinerDrones { get; set; }

        bool IsUseCombatDrones { get; set; }

        bool IsNeedMoveToAsteroid { get; set; }

        bool IsNeedMoveToTractor { get; set; }

        string AmmoType { get; set; }

        bool IsFightWithEnemyShips { get; set; }

        bool IsReloadingAmmo(int secondsAfterLastReload);

        int PitStopCicle { get; set; }
        int Range { get; set; }

        int AdditionalHarvestTime { get; set; }
        int AdditionalWarpTime { get; set; }
    }
}