using System;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Tools;
using EveAssistant.Tests.Devices;
using NUnit.Framework;

namespace EveAssistant.Tests.EveAssistantLogic.Tools
{
    [TestFixture]
    public class MetricsManager_Tests
    {
        [Test]
        public void PatternsLoadedFromWrongDirectoryShouldGetException()
        {
            // Arrange
            //const string exceptedShortcutsApproach = "Q";

            // Act
            //var settings = EveAssistant.Global.ApplicationSettings;
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868454.png");
            device.Pilot = "Dana Su-Metrics";

            // Assert
            //Assert.That(exceptedShortcutsApproach, Is.EqualTo(settings.Shortcuts.Approach));

            MetricsManager.Show(device);

            //bool silenceAlarm = Logic.Tools.Dates.IsDownTime(DateTime.UtcNow);
        }

        [Test]
        public void GetMetricsFileName_ShouldBeCorrect()
        {
            // Arrange

            // Act
            IDevice device = new MockDeviceFromScreenFile(@"Screens/Screen_637773521468868454.png");
            device.Pilot = "Dana Su-Metrics";

            // Assert
            Assert.That("2022-0107-0108-Dana Su-Metrics-0.txt", 
                Is.EqualTo(MetricsManager.GetMetricsFileName(device, new DateTime(2022, 01, 08, 12, 00, 00))));
        }
    }
}