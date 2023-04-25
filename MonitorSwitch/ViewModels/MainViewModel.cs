using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using MonitorSwitch.Repositories;
using MonitorSwitch.Services;
using MonitorSwitch.Utils;

namespace MonitorSwitch.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly KeyHookService _keyHookService = new();
    private readonly MonitorService _monitorService = new();
    private readonly DisplayRepository _displayRepository = new();

    public MainViewModel()
    {
        LoadFromSavedDisplay();
        RefreshScreens();
        _keyHookService.KeyPressed += KeyHookServiceOnKeyPressed;
    }

    private void LoadFromSavedDisplay()
    {
        var lastSelectedDisplay = _displayRepository.GetLastSelectedDisplayName();
        if (lastSelectedDisplay == null) return;

        SelectedScreen = new ScreenViewModel(
            new DisplayDevice(lastSelectedDisplay, null, null, null, MonitorService.DisplayDeviceStateFlags.None));
    }

    private void KeyHookServiceOnKeyPressed(KeyHookService.VKeys obj)
    {
        if (obj != KeyHookService.VKeys.VK_PAUSE) return;

        InvokeOnUIThread(() =>
        {
            if (_selectedScreen == null) return;

            if (_selectedScreen.IsConnected)
            {
                _monitorService.Disconnect(deviceName: _selectedScreen.DeviceName);
            }
            else
            {
                _monitorService.Connect(deviceName: _selectedScreen.DeviceName);
            }

            RefreshScreens();
        });
    }

    public DelegateCommand RefreshCommand => new(_ => RefreshScreens());

    public DelegateCommand SelectDisplayCommand => new(o =>
    {
        if (o is ScreenViewModel viewModel)
        {
            SelectedScreen = viewModel;
            _displayRepository.Save(viewModel.DeviceName);
        }
    });

    private void RefreshScreens()
    {
        Screens.Clear();

        foreach (var screen in _monitorService.GetDisplayDevices())
        {
            Screens.Add(new ScreenViewModel(screen));
        }

        var selectedScreen = Screens.FirstOrDefault(x => x.DeviceName == SelectedScreen?.DeviceName);
        if (selectedScreen != null)
        {
            SelectedScreen = selectedScreen;
        }
    }

    public ObservableCollection<ScreenViewModel> Screens { get; } = new();

    private ScreenViewModel? _selectedScreen;

    public ScreenViewModel? SelectedScreen
    {
        get => _selectedScreen;
        set => SetField(ref _selectedScreen, value);
    }
}

public class ScreenViewModel : BaseViewModel
{
    private readonly DisplayDevice _screen;

    public string DisplayDeviceName => _screen.DeviceName
        .Replace("\\", "")
        .Replace(".", "");

    public bool IsConnected => _screen.StateFlags.HasFlag(MonitorService.DisplayDeviceStateFlags.AttachedToDesktop);
    public string DeviceName => _screen.DeviceName;

    public ScreenViewModel(DisplayDevice screen)
    {
        _screen = screen;
    }
}