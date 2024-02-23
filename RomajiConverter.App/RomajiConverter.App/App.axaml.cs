using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using FluentAvalonia.Styling;
using Newtonsoft.Json;
using RomajiConverter.App.Models;
using RomajiConverter.App.Views;
using RomajiConverter.Core.Helpers;
using RomajiConverter.Core.Models;

namespace RomajiConverter.App;

public partial class App : Application
{
    public const string ConfigFileName = "config.json";

    public static MyConfig Config = null;

    public static List<ConvertedLine> ConvertedLineList = new();

    public static IServiceProvider ServiceProvider { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        InitConfig();
        InitHelper();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.MainWindow.Closed += MainWindowOnClosed;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
            singleViewPlatform.MainView.Unloaded += MainViewOnUnloaded;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void MainWindowOnClosed(object? sender, EventArgs e)
    {
        SaveConfig();
    }

    private void MainViewOnUnloaded(object? sender, RoutedEventArgs e)
    {
        SaveConfig();
    }

    /// <summary>
    /// 初始化应用设置
    /// </summary>
    private void InitConfig()
    {
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
        if (File.Exists(configPath))
        {
            Config = JsonConvert.DeserializeObject<MyConfig>(File.ReadAllText(configPath));
        }
        else
        {
            Config = new MyConfig();
            var file = File.Create(configPath);
            using var sw = new StreamWriter(file);
            sw.Write(JsonConvert.SerializeObject(Config, Formatting.Indented));
        }

        var unidicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "unidic");
        if (Directory.Exists(unidicPath) == false)
        {
            using var resourceZipStream = AssetLoader.Open(new Uri("avares://RomajiConverter.App/Assets/resource.zip"));

            resourceZipStream.Seek(0, SeekOrigin.Begin);

            using var ms = new MemoryStream();

            resourceZipStream.CopyTo(ms);

            ZipFile.ExtractToDirectory(ms, AppDomain.CurrentDomain.BaseDirectory, true);
        }
    }

    public void SaveConfig()
    {
        File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName),
            JsonConvert.SerializeObject(Config, Formatting.Indented));
    }

    private void InitHelper()
    {
        RomajiHelper.Init();
        VariantHelper.Init();
    }
}