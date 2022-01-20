using System;
using NUnit.Framework;

namespace EveAssistant.Tests.EveAssistantCommon.Configuration
{
    [TestFixture]
    public class ApplicationSettingsManagerTests
    {
        [Test]
        public void PatternsLoadedFromWrongDirectoryShouldGetException()
        {
            // Arrange
            const string exceptedShortcutsApproach = "Q";

            // Act
            var settings = EveAssistant.Global.ApplicationSettings;

            // Assert
            Assert.That(exceptedShortcutsApproach, Is.EqualTo(settings.Shortcuts.Approach));


            bool silenceAlarm = Logic.Tools.Dates.IsDownTime(DateTime.Now);
        }
    }
}