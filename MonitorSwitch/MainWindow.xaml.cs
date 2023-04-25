using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using MonitorSwitch.Services;
using MonitorSwitch.Utils;
using MonitorSwitch.ViewModels;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace MonitorSwitch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainTray _mainTray;

        public MainWindow()
        {
            InitializeComponent();
            _mainTray = new MainTray(ShowAction, CloseAction);
        }

        private void ShowAction()
        {
            this.Show();
            this.Activate();
        }

        private void CloseAction()
        {
            Environment.Exit(0);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
