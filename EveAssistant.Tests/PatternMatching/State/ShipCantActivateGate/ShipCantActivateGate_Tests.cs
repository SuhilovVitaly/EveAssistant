using EveAssistant.Common.Device;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.PatternMatching.State.ShipCantActivateGate
{
    [TestFixture]
    public class ShipCantActivateGate_Tests
    {
        private const string Pattern = @"State/ShipInAbiss";

        [Test]
        public void ShouldFindImageInScreen_637777913850525373()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InAbiss/Screen_637777913850525373.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }
    }
}