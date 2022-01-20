using EveAssistant.Common.Device;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.PatternMatching.Bookmarks.Home
{
    [TestFixture]
    public class BookmarksHomeTests
    {
        private const string Pattern = @"Bookmarks/Home";

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
        public void ShouldFindImageInScreen_637776751552686782()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/InSpace/Screen_637776751552686782.png");

            // Act
            var itemOnScreen = device.FindObjectInScreen(Pattern);

            // Assert
            Assert.IsFalse(itemOnScreen.IsFound);
        }
    }
}