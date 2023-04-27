using MonitorSwitch.Services;
using MonitorSwitch.Utils;

namespace MonitorSwitch.ViewModels;

public class ScreenViewModel : BaseViewModel
{
    private readonly DisplayDevice _screen;

    public string DisplayDeviceName => _screen.DeviceName
        .Replace("\\", "")
        .Replace(".", "");

    public bool IsConnected => _screen.StateFlags.HasFlag(MonitorService.DisplayDeviceStateFlags.AttachedToDesktop);
    public string DeviceName => _screen.DeviceName;
    
    private bool _isSelected = false;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetField(ref _isSelected, value);
    }

    public void UpdateSelectedStatus(ScreenViewModel? selectedScreenViewModel)
    {
        IsSelected = selectedScreenViewModel?.DeviceName == DeviceName;
    }

    public ScreenViewModel(DisplayDevice screen)
    {
        _screen = screen;
    }
}