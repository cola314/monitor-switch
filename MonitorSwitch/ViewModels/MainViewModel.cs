﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using MonitorSwitch.Repositories;
using MonitorSwitch.Services;
using MonitorSwitch.Utils;

namespace MonitorSwitch.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly KeyHookService _keyHookService = new();
    private readonly MonitorService _monitorService = new();
    private readonly DisplayRepository _displayRepository = new();
    private readonly AutoStartService _autoStartService = new();

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
            new DisplayDevice(
	            DeviceName: lastSelectedDisplay, 
	            DeviceString: string.Empty, 
	            DeviceId: string.Empty, 
	            DeviceKey: string.Empty,
	            StateFlags: MonitorService.DisplayDeviceStateFlags.None,
	            Detail: new DisplayDetailInfo(0, 0, 0, 0, 0, 0)));
    }

	private void RefreshScreens()
	{
		var displayDevices = _monitorService.GetDisplayDevices().ToList();
		
		Screens.Clear();
		foreach (var screen in displayDevices)
		{
			Screens.Add(new ScreenViewModel(screen));
		}

		var selectedScreen = Screens.FirstOrDefault(x => x.DeviceName == SelectedScreen?.DeviceName);
		if (selectedScreen != null)
		{
			SelectedScreen = selectedScreen;
		}

		VisualScreens.Clear();

		int visualWidth = 300;
        int visualHeight = 100;

        int minLeft = displayDevices.Min(x => x.Detail.PosX);
        int maxRight = displayDevices.Max(x => x.Detail.PosX + x.Detail.Width);
        int width = maxRight - minLeft;

        double widthRatio = (double) visualWidth / width;

        int minBottom = displayDevices.Min(x => x.Detail.PosY);
        int maxBottom = displayDevices.Max(x => x.Detail.PosY + x.Detail.Height);
        int height = maxBottom - minBottom;

        double heightRatio = (double) visualHeight / height;

        double ratio = Math.Min(widthRatio, heightRatio);

		int leftBias = displayDevices.Min(x => x.Detail.PosX);
		int topBias = displayDevices.Min(x => x.Detail.PosY);
		int margin = 3;

		int Scaling(int x, int bias) => (int)((x - bias) * ratio);

		foreach (var screen in displayDevices)
		{
            VisualScreens.Add(new VisualScreenItemViewModel(
	            left: Scaling(screen.Detail.PosX, leftBias),
	            top: Scaling(screen.Detail.PosY, topBias),
	            width: Scaling(screen.Detail.Width, 0) - margin,
	            height: Scaling(screen.Detail.Height, 0) - margin,
	            displayName: new ScreenViewModel(screen).DisplayDeviceName,
	            isConnected: new ScreenViewModel(screen).IsConnected));
		}
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

    public DelegateCommand ToggleDisplayCommand => new(o =>
    {
        if (o is ScreenViewModel viewModel)
        {
            if (viewModel.IsConnected)
            {
                _monitorService.Disconnect(viewModel.DeviceName);
            }
            else
            {
                _monitorService.Connect(viewModel.DeviceName);
            }

            RefreshScreens();
        }
    });

    public ObservableCollection<ScreenViewModel> Screens { get; } = new();

    private ScreenViewModel? _selectedScreen;

    public ScreenViewModel? SelectedScreen
    {
        get => _selectedScreen;
        set
        {
            SetField(ref _selectedScreen, value);

            foreach (var screen in Screens)
                screen.UpdateSelectedStatus(SelectedScreen);
        }
    }

    public ObservableCollection<VisualScreenItemViewModel> VisualScreens { get; } = new();

    public bool IsStartUpProgram
    {
        get => _autoStartService.IsRegistered();
        set
        {
            if (value)
                _autoStartService.Register();
            else
                _autoStartService.UnRegister();
        }
    }
}