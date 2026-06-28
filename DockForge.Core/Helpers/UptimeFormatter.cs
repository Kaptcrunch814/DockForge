using DockForge.Core.Models;

namespace DockForge.Core.Helpers;

public static class UptimeFormatter
{
    public static string GetUptimeText(ContainerInfo container)
    {
        if (!container.Status.Equals("running", StringComparison.OrdinalIgnoreCase))
        {
            return "Not running";
        }

        if (container.StartedAt is null)
        {
            return "Unknown";
        }

        var uptime = DateTime.UtcNow - container.StartedAt.Value.ToUniversalTime();

        if (uptime.TotalSeconds < 0)
        {
            return "Unknown";
        }

        return FormatDuration(uptime);
    }

    private static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalDays >= 1)
        {
            return $"{(int)duration.TotalDays}d {duration.Hours}h {duration.Minutes}m";
        }

        if (duration.TotalHours >= 1)
        {
            return $"{duration.Hours}h {duration.Minutes}m";
        }

        if (duration.TotalMinutes >= 1)
        {
            return $"{duration.Minutes}m {duration.Seconds}s";
        }

        return $"{duration.Seconds}s";
    }
}