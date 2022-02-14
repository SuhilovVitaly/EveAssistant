using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class ResetActivity : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[ResetActivity]";

        public ResetActivity(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 60;
        }

        public void CommandsExecute()
        {
            Thread.Sleep(200);

            Device.Keyboard.PressKey(Device.Shortcuts.NextTarget);

            Thread.Sleep(1000);

            var isTargetLocked = true;

            while (isTargetLocked)
            {
                isTargetLocked = OperationsManager.Execute(OperationTypes.UnlockTarget, Device, Ship);
                
                Thread.Sleep(1000);

                Device.Keyboard.PressKey(Device.Shortcuts.NextTarget);

                Thread.Sleep(1000);
            }
        }
    }
}