using EveAssistant.Graphic;
using NUnit.Framework;

namespace EveAssistant.Tests.EveAssistantGraphic
{
    [TestFixture]
    public class ImageFactoryTests
    {
        [Test]
        public void ShouldLoadImageFromFile()
        {
            // Arrange
            const string fileName = @"Screens/Screen_637773521468868454.png";

            // Act
            var image = new ImageFactory().Load(fileName);

            // Assert
            Assert.AreNotEqual(null, image);
        }

        [Test]
        public void ShouldFailOnLoadNonExistImageFromFile()
        {
            // Arrange
            const string fileName = "13123";

            // Act

            // Assert
            Assert.Throws<System.IO.FileNotFoundException>(() => new ImageFactory().Load(fileName));
        }

        [Test]
        public void ShouldLoadImageFromCache()
        {
            // Arrange
            const string fileName = @"Screens/Screen_637773521468868454.png";

            // Act
            var imageFactory = new ImageFactory();

            var imageFromFile = imageFactory.Load(fileName);
            var imageFromCache = imageFactory.Load(fileName);

            // Assert
            Assert.AreNotEqual(null, imageFromFile);
            Assert.AreNotEqual(null, imageFromCache);
        }
    }
}
