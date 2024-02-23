using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RomajiConverter.App.Android.Launcher;

public class AndroidLauncher : RomajiConverter.App.Launcher.Launcher
{
    public override async Task LaunchUriAsync(Uri uri)
    {
        await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }
}