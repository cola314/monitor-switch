using System.Windows;
using MonitorSwitch.Utils;

namespace MonitorSwitch.ViewModels;

public class VisualScreenItemViewModel : BaseViewModel
{
	public Visibility Visibility { get; }
	public int Left { get; }
	public int Top { get; }
	public int Width { get; }
	public int Height { get; }
	public string DisplayName { get; }

	public VisualScreenItemViewModel(int left, int top, int width, int height, string displayName, bool isConnected)
	{
		Left = left;
		Top = top;
		Width = width;
		Height = height;
		DisplayName = displayName;
		Visibility = isConnected ? Visibility.Visible : Visibility.Collapsed;
	}
}