using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RomajiConverter.App.Desktop.Launcher;

public class WindowsLauncher : RomajiConverter.App.Launcher.Launcher
{
    public override async Task LaunchUriAsync(Uri uri)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            using var proc = new Process { StartInfo = { UseShellExecute = true, FileName = uri.AbsoluteUri } };
            proc.Start();
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("x-www-browser", uri.AbsoluteUri);
        }
    }
}