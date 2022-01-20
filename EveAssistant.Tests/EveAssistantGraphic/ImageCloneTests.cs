using EveAssistant.Graphic;
using NUnit.Framework;

namespace EveAssistant.Tests.EveAssistantGraphic
{
    [TestFixture]
    public class ImageCloneTests
    {
        [Test]
        public void ShouldFindImageInImage()
        {
            // Arrange
            var sourceImage = new ImageFactory().Load(@"Screens/Screen_637773521468868454.png");

            // Act
            var clonedImage = ImageClone.Execute(sourceImage);

            // Assert
            Assert.IsNotNull(clonedImage.Image);
            Assert.AreNotEqual(sourceImage, clonedImage.Image);
        }
    }
}
