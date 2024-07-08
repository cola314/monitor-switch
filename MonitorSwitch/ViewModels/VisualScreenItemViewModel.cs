using System;
using System.Windows;
using MonitorSwitch.Utils;

namespace MonitorSwitch.ViewModels;

public class VisualScreenItemViewModel : BaseViewModel
{
	public int Left { get; }
	public int Top { get; }
	public int Width { get; }
	public int Height { get; }
	public string DeviceName { get; }
	public string DisplayName { get; }

	private readonly Action<VisualScreenItemViewModel> _onClicked;

	public VisualScreenItemViewModel(
		int left,
		int top,
		int width,
		int height,
		string deviceName,
		string displayName,
		Action<VisualScreenItemViewModel> onClicked)
	{
		Left = left;
		Top = top;
		Width = width;
		Height = height;
		DeviceName = deviceName;
		DisplayName = displayName;
		_onClicked = onClicked;
	}

	private bool _isSelected;
	public bool IsSelected
	{
		get => _isSelected;
		set => SetField(ref _isSelected, value);
	}

	public DelegateCommand OnClickedCommand => new(_ =>
	{
		_onClicked.Invoke(this);
	});
}