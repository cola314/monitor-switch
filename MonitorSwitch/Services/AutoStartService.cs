using System.Windows.Forms;
using Microsoft.Win32;

namespace MonitorSwitch.Services;

public class AutoStartService
{
    private const string ProgramName = "MonitorSwitch";
    private const string StartupRegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    
    public const string StartupArgument = "/startup";

    private readonly string _executeCommand;

    public AutoStartService()
    {
        _executeCommand = $"{Application.ExecutablePath} {StartupArgument}";
    }

    private RegistryKey? StartupRegistry
        => Registry.CurrentUser.OpenSubKey(StartupRegPath, true);

    public bool IsRegistered() 
        => StartupRegistry?.GetValue(ProgramName) != null;

    public void Register() 
        => StartupRegistry?.SetValue(ProgramName, _executeCommand);

    public void UnRegister() 
        => StartupRegistry?.DeleteValue(ProgramName);
}