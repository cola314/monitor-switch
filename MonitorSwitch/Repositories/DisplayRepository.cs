using Microsoft.Win32;

namespace MonitorSwitch.Repositories;

public class DisplayRepository
{
    private const string SoftWareKey = "Software";
    private const string MonitorSwitchKey = "MonitorSwitch";
    private const string LastSelectedDisplayNameKey = "LastSelectedDisplayName";

    public void Save(string displayName)
    {
        Registry.CurrentUser
            .CreateSubKey(SoftWareKey)
            .CreateSubKey(MonitorSwitchKey)
            .SetValue(LastSelectedDisplayNameKey, displayName);
    }

    public string? GetLastSelectedDisplayName()
    {
        return Registry.CurrentUser
            .CreateSubKey(SoftWareKey)
            .CreateSubKey(MonitorSwitchKey)
            .GetValue(LastSelectedDisplayNameKey) as string;
    }
}