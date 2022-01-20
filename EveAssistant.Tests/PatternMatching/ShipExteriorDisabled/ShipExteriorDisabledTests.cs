using EveAssistant.Common.Device;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.PatternMatching.ShipExteriorDisabled
{
    [TestFixture]
    public class ShipExteriorDisabledTests
    {
        private const string pattern = @"ShipExteriorDisabled";

        [Test]
        public void ShouldNotFindImageInScreen_637773521468868454()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868454.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_Screen_637773521468868455()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868455.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_Screen_637776682522525968()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InSpace/Screen_637776682522525968.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_Screen_637776682574687863()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InSpace/Screen_637776682574687863.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert 
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_Screen_637776685608552674()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InDock/Screen_637776685608552674.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert 
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_Screen_637776686161779575()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InDock/Screen_637776686161779575.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert 
            Assert.IsTrue(itemOnScreen.IsFound);
        }
    }
}