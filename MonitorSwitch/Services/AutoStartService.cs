using System.Windows.Forms;
using Microsoft.Win32;

namespace MonitorSwitch.Services;

public class AutoStartService
{
    private const string ProgramName = "MonitorSwitch";
    private const string StartupRegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    private readonly string _executablePath;

    public AutoStartService()
    {
        _executablePath = Application.ExecutablePath;
    }

    private RegistryKey? StartupRegistry
        => Registry.CurrentUser.OpenSubKey(StartupRegPath, true);

    public bool IsRegistered() 
        => StartupRegistry?.GetValue(ProgramName) != null;

    public void Register() 
        => StartupRegistry?.SetValue(ProgramName, _executablePath);

    public void UnRegister() 
        => StartupRegistry?.DeleteValue(ProgramName);
}