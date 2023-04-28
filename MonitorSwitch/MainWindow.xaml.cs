using System;
using System.ComponentModel;
using System.Windows;
using MonitorSwitch.Services;

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
            HideWindowIfRunAsStartupProgram();
            _mainTray = new MainTray(ShowAction, CloseAction);
        }

        private void HideWindowIfRunAsStartupProgram()
        {
            if (Environment.GetCommandLineArgs() is [_, AutoStartService.StartupArgument])
            {
                this.Hide();
            }
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
