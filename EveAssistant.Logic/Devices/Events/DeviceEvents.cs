using System;
using EveAssistant.Common.Device.Events;

namespace EveAssistant.Logic.Devices.Events
{
    public class DeviceEvents: IDeviceEvents
    {
        public event Action OnEnemyAttack;
        public event Action OnTargetNoLongerPresent;
        public event Action OnWeaponReload;
        public event Action OnTargetLimitExceeded;
        public event Action OnWeaponActivity;
        public event Action OnJumpThroughStarGate;
        public event Action OnDronesActivity;
        public event Action OnCommandRejectedByCloakReasone;
        public event Action OnTractorIsTooFarAwat;
        public event Action OnCargoHoldIsFull;
        public event Action OnMinedOre;
        public event Action OnRunOutOfCharges;
        public event Action OnResourceWasHarvesting;
        public event Action OnStationDenied;
        public event Action OnReloadMiningForemanBurstCharges;


        public virtual void WeaponActivity()
        {
            OnWeaponActivity?.Invoke();
        }

        public virtual void DronesActivity()
        {
            OnDronesActivity?.Invoke();
        }

        public virtual void TargetLimitExceeded()
        {
            OnTargetLimitExceeded?.Invoke();
        }

        public virtual void WeaponReload()
        {
            OnWeaponReload?.Invoke();
        }

        public virtual void TargetNoLongerPresent()
        {
            OnTargetNoLongerPresent?.Invoke();
        }

        public virtual void EnemyAttack()
        {
            OnEnemyAttack?.Invoke();
        }

        public virtual void JumpThroughStarGate()
        {
            OnJumpThroughStarGate?.Invoke();
        }

        public virtual void CommandRejectedByCloakReasone()
        {
            OnCommandRejectedByCloakReasone?.Invoke();
        }

        public virtual void TractorIsTooFarAwat()
        {
            OnTractorIsTooFarAwat?.Invoke();
        }

        public void CargoHoldIsFull()
        {
            OnCargoHoldIsFull?.Invoke();
        }


        public void MinedOre()
        {
            OnMinedOre?.Invoke();
        }

        public void ResourceWasHarvesting()
        {
            OnResourceWasHarvesting?.Invoke();
        }

        public void StationDenied()
        {
            OnStationDenied?.Invoke();
        }

        public void ReloadMiningForemanBurstCharges()
        {
            OnReloadMiningForemanBurstCharges?.Invoke();
        }

    }
}