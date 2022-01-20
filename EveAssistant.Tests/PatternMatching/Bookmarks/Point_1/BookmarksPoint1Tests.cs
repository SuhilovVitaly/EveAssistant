using EveAssistant.Common.Device;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.PatternMatching.Bookmarks.Point_1
{
    [TestFixture]
    public class BookmarksPoint1Tests
    {
        private const string Pattern = @"Bookmarks/Point_1";

        [Test]
        public void ShouldFindImageInScreen_637773521468868454()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InDock/Screen_637776677146380016.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_Screen_637776682522525968()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InSpace/Screen_637776682522525968.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_Screen_637776732456059408()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InSpace/Screen_637776732456059408.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsTrue(itemOnScreen.IsFound);
        }

        [Test]
        public void ShouldFindImageInScreen_6377766721588918861()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868454.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }
    }
}