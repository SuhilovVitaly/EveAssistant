using System.Collections.Generic;
using System.Drawing;
using EveAssistant.Common;
using EveAssistant.Common.Device;
using EveAssistant.Graphic;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.EveAssistantGraphic
{
    [TestFixture]
    public class ImageDetectTests
    {
        [Test]
        public void GetObjectPositionShouldReturnImage()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868454.png");

            var patterns = new PatternsCollection
            {
                PatternEntity = new List<IPattern>
                {
                    new Pattern
                    {
                        Image = new ImageFactory().Load(@"Screens/Objects/ElectricalFilament.png"),
                        Name = "Test"
                    }
                }
            };
            var expectedRectangle = new Rectangle(95, 671, 51, 103);

            // Act
            var result = ImageDetect.SearchImage(patterns, device);

            // Assert
            Assert.That(result.IsFound);
            Assert.That(result.Zone, Is.EqualTo(expectedRectangle));
        }

        [Test]
        public void SearchImageInRectangleShouldBeCorrect()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868454.png");

            var patterns = new PatternsCollection
            {
                PatternEntity = new List<IPattern>
                {
                    new Pattern
                    {
                        Image = new ImageFactory().Load(@"Screens/Objects/ElectricalFilament.png"),
                        Name = "Test"
                    }
                }
            };
            var expectedRectangle = new Rectangle(95, 671, 51, 103);

            // Act
            var result = ImageDetect.SearchImage(patterns, device, new Rectangle(65, 665, 200, 200));

            // Assert
            Assert.That(result.IsFound);
            Assert.That(result.Zone, Is.EqualTo(expectedRectangle));
        }

        [Test]
        public void GetFirstImagePositionFromManyShouldBeCorrect()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868455.png");

            var patterns = new PatternsCollection
            {
                PatternEntity = new List<IPattern>
                {
                    new Pattern
                    {
                        Image = new ImageFactory().Load(@"Screens/Objects/ElectricalFilament.png"),
                        Name = "Test"
                    }
                }
            };
            var expectedRectangle = new Rectangle(712, 356, 51, 103);

            // Act
            var result = ImageDetect.SearchImage(patterns, device);

            // Assert
            Assert.That(result.IsFound);
            Assert.That(result.Zone, Is.EqualTo(expectedRectangle));
        }

        [Test]
        public void SearchAllImagesByPatternsInRectangleShouldBeCorrect()
        {
            // Arrange
            const int expectedSearchResultCount = 3;
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868455.png");

            var patterns = new PatternsCollection
            {
                PatternEntity = new List<IPattern>
                {
                    new Pattern
                    {
                        Image = new ImageFactory().Load(@"Screens/Objects/ElectricalFilament.png"),
                        Name = "ElectricalFilament"
                    },
                    new Pattern
                    {
                        Image = new ImageFactory().Load(@"Screens/Objects/Turret.png"),
                        Name = "Turret"
                    }
                }
            };

            // Act
            var result = ImageDetect.SearchAllImages(patterns, device, new Rectangle(600, 0, 300, 600));

            // Assert
            Assert.That(result.IsFound);
            Assert.AreEqual(expectedSearchResultCount, result.SearchResults.Count);
        }

        [Test]
        public void SearchAllImagesByPatternInScreenShouldReturnCollection()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868455.png");
            
            const int expectedSearchResultCount = 2;

            var patterns = new PatternsCollection
            {
                PatternEntity = new List<IPattern>
                {
                    new Pattern
                    {
                        Image = new ImageFactory().Load(@"Screens/Objects/ElectricalFilament.png"),
                        Name = "Test"
                    }
                }
            };

            // Act
            var result = ImageDetect.SearchAllImages(patterns, device);

            // Assert
            Assert.AreEqual(expectedSearchResultCount, result.SearchResults.Count );
        }

        [Test]
        public void SearchAllImagesByPatternsInScreenShouldReturnCollection()
        {
            // Arrange
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868455.png");
            
            const int expectedSearchResultCount = 4;

            var patterns = new PatternsCollection
            {
                PatternEntity = new List<IPattern>
                {
                    new Pattern
                    {
                        Image = new ImageFactory().Load(@"Screens/Objects/ElectricalFilament.png"),
                        Name = "Test"
                    },
                    new Pattern
                    {
                        Image = new ImageFactory().Load(@"Screens/Objects/Turret.png"),
                        Name = "Test"
                    }
                }
            };

            // Act
            var result = ImageDetect.SearchAllImages(patterns, device);

            // Assert
            Assert.AreEqual(expectedSearchResultCount, result.SearchResults.Count);

        }
    }
}
