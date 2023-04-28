using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            KillDuplicateProcess();
            SetWorkingDirectory();
        }

        private void KillDuplicateProcess()
        {
            var currentProcess = Process.GetCurrentProcess();
            var duplicateProcesses = Process
                .GetProcessesByName(currentProcess.ProcessName)
                .Where(p => p.Id != currentProcess.Id);

            foreach (var duplicateProcess in duplicateProcesses)
            {
                duplicateProcess.Kill();
            }
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
