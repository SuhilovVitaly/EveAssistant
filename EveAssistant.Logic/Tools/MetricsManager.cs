using System;
using System.IO;
using EveAssistant.Common.Device;

namespace EveAssistant.Logic.Tools
{
    public class MetricsManager
    {
        public static void Show(IDevice device)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports\\Metrics");

            UpdateDirectory(path);

            var pathForShowMetrics = Path.Combine(path, GetMetricsFileName(device));

            ShowMetrics(pathForShowMetrics, device);
        }

        public static string GetMetricsFileName(IDevice device, DateTime? currentDate = null)
        {
            if (!currentDate.HasValue)
                currentDate = DateTime.UtcNow;

            var file = $"{currentDate.Value.Year:D4}";

            var today = currentDate.Value;
            var yesterday = currentDate.Value.AddDays(-1);
            var tomorrow = currentDate.Value.AddDays(+1);

            if (Dates.IsBeforeDownTime(currentDate.Value))
            {
                file += $"-{yesterday.Month:D2}{yesterday.Day:D2}-{today.Month:D2}{today.Day:D2}";
            }
            else
            {
                file += $"-{today.Month:D2}{today.Day:D2}-{tomorrow.Month:D2}{tomorrow.Day:D2}";
            }

            file += $"-{device.Pilot}-{device.Id}.txt";

            return file;
        }

        private static void ShowMetrics(string path, IDevice device)
        {
            if (device?.Metrics is null) return;

            File.WriteAllLines(path, new[] { GetMessage(device) });
        }

        private static string GetMessage(IDevice device)
        {
            var result = "";

            try
            {
                result = result + device.Pilot + Environment.NewLine + Environment.NewLine;
            }
            catch
            {
                // ignored
            }

            try
            {
                result += $"Job work (min): {DateTime.Now.Subtract(device.Metrics.StartJobTime).ToReadableString()} {Environment.NewLine}";
            }
            catch
            {
                // ignored
            }

            try
            {
                result += $"Cycles: {device.Metrics.NumberOfCycles} {Environment.NewLine}";
            }
            catch
            {
                // ignored Device.Metrics.NumberOfCycles
            }

            try
            {
                result += $"Cycle work (min): {DateTime.Now.Subtract(device.Metrics.StartCycleTime).ToReadableString()} {Environment.NewLine}";
            }
            catch
            {
                // ignored
            }

            try
            {
                result += $"Average cycle time (min): {device.Metrics.AverageCycleTime.ToReadableString()} {Environment.NewLine}";
            }
            catch
            {
                // ignored 
            }

            return result;
        }

        private static void UpdateDirectory(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}