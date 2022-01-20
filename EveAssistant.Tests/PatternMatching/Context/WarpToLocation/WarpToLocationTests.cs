using EveAssistant.Common.Device;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.PatternMatching.Context.WarpToLocation
{
    [TestFixture]
    public class WarpToLocationTests
    {
        private const string Pattern = @"ContextMenu/WarpToLocation";

        [Test]
        public void ShouldFindImageInScreen_637776736871579584()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/ContextMenu/Screen_637776736871579584.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_637776682522525968()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InSpace/Screen_637776682522525968.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }
    }
}