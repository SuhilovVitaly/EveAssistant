using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.PatternMatching.Fleet
{
    [TestFixture]
    public class FleetPatternMatchingTests
    {
        [Test]
        public void FleetCommanderNotFindImageInScreen_637782698953802193()
        {
            // Arrange
            const string pattern = Types.FleetCommander;
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Fleet/Screen_637782698953802193.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }

        [Test]
        public void FleetCommanderFindImageInScreen_637782699707849715()
        {
            // Arrange
            const string pattern = Types.FleetCommander;
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Fleet/Screen_637782699707849715.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ContextMenuFormFleet_FoundOnScreen_637782718244548579()
        {
            // Arrange
            const string pattern = Types.ContextMenuFormFleet;
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Fleet/Screen_637782718244548579.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ContextMenuFormFleet_NotFoundOnScreen_637782699707849715()
        {
            // Arrange
            const string pattern = Types.ContextMenuFormFleet;
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Fleet/Screen_637782699707849715.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }
    }
}