using System;

namespace EveAssistant.Common.Device.Events
{
    public interface IDeviceEvents
    {
        event Action OnEnemyAttack;
        event Action OnTargetNoLongerPresent;
        event Action OnWeaponReload;
        event Action OnTargetLimitExceeded;
        event Action OnWeaponActivity;
        event Action OnJumpThroughStarGate;
        event Action OnDronesActivity;
        event Action OnCommandRejectedByCloakReasone;
        event Action OnTractorIsTooFarAwat;
        event Action OnCargoHoldIsFull;
        event Action OnMinedOre;
        event Action OnRunOutOfCharges;
        event Action OnResourceWasHarvesting;
        event Action OnStationDenied;
        event Action OnReloadMiningForemanBurstCharges;

        void WeaponActivity();
        void DronesActivity();
        void TargetLimitExceeded();
        void WeaponReload();
        void TargetNoLongerPresent();
        void EnemyAttack();
        void JumpThroughStarGate();
        void CommandRejectedByCloakReasone();
        void TractorIsTooFarAwat();
        void CargoHoldIsFull();
        void MinedOre();
        void ResourceWasHarvesting();
        void StationDenied();
        void ReloadMiningForemanBurstCharges();

    }
}