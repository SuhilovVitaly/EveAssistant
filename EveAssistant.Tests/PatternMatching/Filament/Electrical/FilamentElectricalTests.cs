using EveAssistant.Common.Device;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.PatternMatching.Filament.Electrical
{
    [TestFixture]
    public class FilamentElectricalTests
    {
        [Test]
        public void ShouldFindImageInScreen_637773521468868454()
        {
            // Arrange
            const string pattern = @"Filament/Electrical";
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
            const string pattern = @"Filament/Electrical";
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InDock/Screen_637776672158891886.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_637776677146380016()
        {
            // Arrange
            const string pattern = @"Filament/Electrical";
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InDock/Screen_637776677146380016.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_637776677874330066()
        {
            // Arrange
            const string pattern = @"Filament/Electrical";
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Filament/Electrical/Screen_637776677874330066.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_Screen_637776677949601863()
        {
            // Arrange
            const string pattern = @"Filament/Electrical";
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Filament/Electrical/Screen_637776677949601863.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldNotFindImageInScreen_637776676184444824()
        {
            // Arrange
            const string pattern = @"Filament/Electrical";
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InDock/Screen_637776676184444824.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }
    }
}