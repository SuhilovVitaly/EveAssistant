using EveAssistant.Common.Device;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.PatternMatching.Panel.ActiveShipCargo
{
    [TestFixture]
    public class ActiveShipCargoTests
    {
        [Test]
        public void ShouldFindImageInScreen_637773521468868454()
        {
            // Arrange
            const string pattern = @"Panel/ActiveShipCargo";
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868454.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_637776672158891886()
        {
            // Arrange
            const string pattern = @"Panel/ActiveShipCargo";
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InDock/Screen_637776672158891886.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldNotFindImageInScreen_637776673967770315()
        {
            // Arrange
            const string pattern = @"Panel/ActiveShipCargo";
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InDock/Screen_637776673967770315.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }
    }
}