using EveAssistant.Common.Device;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.PatternMatching.State.ShipStopped
{
    [TestFixture]
    public class StateShipStoppedTests
    {
        private const string Pattern = @"State/ShipStopped";

        [Test]
        public void ShouldFindImageInScreen_637777837002130814()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InSpace/Screen_637777837002130814.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_637773521468868455()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868455.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_637776751552686782()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InSpace/Screen_637776751552686782.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }


        [Test]
        public void ShouldFindImageInScreen_637776732456059408()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InSpace/Screen_637776732456059408.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_637776682574687863()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InSpace/Screen_637776682574687863.png");

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