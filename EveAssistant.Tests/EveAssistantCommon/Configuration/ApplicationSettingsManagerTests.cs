using System;
using NUnit.Framework;

namespace EveAssistant.Tests.EveAssistantCommon.Configuration
{
    [TestFixture]
    public class ApplicationSettingsManagerTests
    {
        [Test]
        public void IsBeforeDownTimeShouldGetBeCorrect()
        {
            // Arrange

            // Act

            // Assert

            Assert.IsTrue(Logic.Tools.Dates.IsBeforeDownTime(
                new DateTime(
                    DateTime.UtcNow.Year,
                    DateTime.UtcNow.Month,
                    DateTime.UtcNow.Day,
                    08,00,00)));

            Assert.IsFalse(Logic.Tools.Dates.IsBeforeDownTime(
                new DateTime(
                    DateTime.UtcNow.Year,
                    DateTime.UtcNow.Month,
                    DateTime.UtcNow.Day,
                    12, 00, 00)));

            Assert.IsTrue(Logic.Tools.Dates.IsBeforeDownTime(
                new DateTime(
                    DateTime.UtcNow.Year,
                    DateTime.UtcNow.Month,
                    DateTime.UtcNow.Day - 1,
                    12, 00, 00)));
        }
    }
}