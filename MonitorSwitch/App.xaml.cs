using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace MonitorSwitch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SetWorkingDirectory();
        }

        private void SetWorkingDirectory()
        {
            var process = Process.GetCurrentProcess();
            var directory = Path.GetDirectoryName(process.MainModule?.FileName)
                            ?? throw new Exception("MainModule directory can not be null");

            Directory.SetCurrentDirectory(directory);
        }
    }
}
