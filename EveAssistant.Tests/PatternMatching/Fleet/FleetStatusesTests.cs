using EveAssistant.Common.Device;
using EveAssistant.Logic.Jobs.Status;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.PatternMatching.Fleet
{
    [TestFixture]
    public class FleetStatusesTests
    {
        [Test]
        public void NotFleetCommander_637782698953802193()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Fleet/Screen_637782698953802193.png");

            // Act
            var itemOnScreen = AllStates.IsFleetCommander(device);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }

        [Test]
        public void FleetCommander_637782698953802193()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Fleet/Screen_637782699707849715.png");

            // Act
            var itemOnScreen = AllStates.IsFleetCommander(device);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void FleetWindowIsClosed_637782698953802193()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Fleet/Screen_637782698953802193.png");

            // Act
            var itemOnScreen = AllStates.IsFleetWindowOpened(device);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }

        [Test]
        public void FleetWindowIsOpened_637782699636002407()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Fleet/Screen_637782699636002407.png");

            // Act
            var itemOnScreen = AllStates.IsFleetWindowOpened(device);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void FleetWindowIsOpened_637782699707849715()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Fleet/Screen_637782699707849715.png");

            // Act
            var itemOnScreen = AllStates.IsFleetWindowOpened(device);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void FleetWindowIsOpened_637782714548837472()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Fleet/Screen_637782714548837472.png");

            // Act
            var itemOnScreen = AllStates.IsFleetWindowOpened(device);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }
    }
}