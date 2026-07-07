using Microsoft.Win32;
using System.Windows.Forms;

namespace PostureGuard.Systems
{
    public class PersistenceSystem
    {
        private const string RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "PostureGuard";

        public void Init()
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
            {
                if (key != null && key.GetValue(AppName) == null)
                {
                    key.SetValue(AppName, $"\"{Application.ExecutablePath}\"");
                }
            }
        }
    }
}