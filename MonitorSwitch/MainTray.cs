using System;
using MonitorSwitch.Utils;
using System.Drawing;
using System.Windows.Forms;

namespace MonitorSwitch;

public class MainTray
{
    private readonly NotifyIcon _notifyIcon;

    private readonly Action _showAction;
    private readonly Action _closeAction;

    public MainTray(Action showAction, Action closeAction)
    {
        _showAction = showAction;
        _closeAction = closeAction;
        _notifyIcon = CreateNotifyIcon();
    }

    private NotifyIcon CreateNotifyIcon()
    {
        var context = new ContextMenuStrip();
        var showItem = context.Items.Add("show", null);
        var exitItem = context.Items.Add("exit", null);

        showItem.Command = new DelegateCommand(_ =>
        {
            _showAction.Invoke();
        });
        exitItem.Command = new DelegateCommand(_ =>
        {
            _closeAction.Invoke();
        });

        var ni = new NotifyIcon()
        {
            Icon = new Icon("tray.ico"),
            Visible = true,
            ContextMenuStrip = context,
        };
        ni.Click += NiOnClick;

        return ni;
    }

    private void NiOnClick(object? sender, EventArgs e)
    {
        if (e is MouseEventArgs { Button: MouseButtons.Left })
        {
            _showAction.Invoke();
        }
    }
}