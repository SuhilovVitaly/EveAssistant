using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;
using Timer = System.Timers.Timer;

namespace EveAssistant.Logic.Jobs.Events
{
    public class BaseEvent
    {
        public event Action<string, int> OnBaseEvent;

        private readonly IDevice _device;
        private IShip _ship;

        private readonly Timer _crlRefreshMap;

        public BaseEvent(IDevice device, IShip ship)
        {
            _device = device;
            _ship = ship;

            _crlRefreshMap = new Timer();
            _crlRefreshMap.Elapsed += Event_Refresh;
            _crlRefreshMap.Interval = 2000;
            _crlRefreshMap.Enabled = false;
        }

        public async Task Execute()
        {
            _device.Logger("[Event] Start execute base event.");

            _crlRefreshMap.Enabled = true;
        }

        private void Event_Refresh(object sender, ElapsedEventArgs e)
        {
            OnBaseEvent?.Invoke("[Event] Refresh execute base event.", 100);
        }

        public void Stop()
        {
            _crlRefreshMap.Enabled = false;
        }
    }
}