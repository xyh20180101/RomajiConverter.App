using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using RomajiConverter.App.Android.Launcher;

namespace RomajiConverter.App.Android;

[Activity(
    Label = "罗马音转换器",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        var services = new ServiceCollection();
        services.AddSingleton<RomajiConverter.App.Launcher.Launcher, AndroidLauncher>();
        App.ServiceProvider = services.BuildServiceProvider();

        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    protected override void OnPause()
    {
        ((App)Avalonia.Application.Current).SaveConfig();
        base.OnPause();
    }
}