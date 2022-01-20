using System;
using System.IO;
using EveAssistant.Graphic;
using NUnit.Framework;

namespace EveAssistant.Tests.EveAssistantGraphic
{
    [TestFixture]
    public class PatternsTests
    {
        [Test]
        public void PatternsLoadedFromWrongDirectoryShouldGetException()
        {
            // Arrange
            var absolutePath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\..\\EveAssistant\\Patterns1");

            // Act

            // Assert
            Assert.Throws<DirectoryNotFoundException>(() => { new Patterns(absolutePath); });
        }

        [Test]
        public void PatternsBaseLoadClientPatternsShouldBeCorrect()
        {
            // Arrange

            // Act
            var patterns = new Patterns(Global.PatternsClientPath);

            // Assert
            Assert.IsNotNull(patterns);
        }

        [Test]
        public void PatternsLoadedFromDirectoryShouldByCorrectCount()
        {
            // Arrange
            const int expectedPatternsCountInDirectoryCommon = 2;
            const int expectedPatternsCountInDirectoryFilamentElectrical = 1;

            // Act
            var patterns = new Patterns(Global.PatternsTestsPath);

            // Assert
            Assert.IsNotNull(patterns);
            Assert.That(expectedPatternsCountInDirectoryCommon, Is.EqualTo(patterns.Get("Common").Count));
            Assert.That(expectedPatternsCountInDirectoryFilamentElectrical, Is.EqualTo(patterns.Get("Filament/Electrical").Count));
        }
    }
}
