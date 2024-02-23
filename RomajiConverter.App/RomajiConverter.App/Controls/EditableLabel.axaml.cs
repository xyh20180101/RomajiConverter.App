using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using RomajiConverter.App.Enums;
using RomajiConverter.Core.Models;

namespace RomajiConverter.App.Controls;

public partial class EditableLabel : UserControl, INotifyPropertyChanged
{
    public static readonly StyledProperty<ReplaceString> SelectedTextProperty =
        AvaloniaProperty.Register<EditableLabel, ReplaceString>(
            nameof(SelectedText), new ReplaceString(0, "", true));

    public static readonly StyledProperty<ObservableCollection<ReplaceString>> ReplaceTextProperty =
        AvaloniaProperty.Register<EditableLabel, ObservableCollection<ReplaceString>>(nameof(ReplaceText),
            new ObservableCollection<ReplaceString>());

    public static readonly StyledProperty<double> MyFontSizeProperty =
        AvaloniaProperty.Register<EditableLabel, double>(nameof(MyFontSize), 14d);

    public static readonly StyledProperty<BorderVisibilitySetting> BorderVisibilitySettingProperty =
        AvaloniaProperty.Register<EditableLabel, BorderVisibilitySetting>(nameof(BorderVisibilitySetting),
            BorderVisibilitySetting.Hidden);

    private bool _isEdit;

    public EditableLabel()
    {
        InitializeComponent();
        DataContext = this;
        IsEdit = false;
    }

    public ReplaceString SelectedText
    {
        get => GetValue(SelectedTextProperty);
        set => SetValue(SelectedTextProperty, value);
    }

    public ObservableCollection<ReplaceString> ReplaceText
    {
        get => GetValue(ReplaceTextProperty);
        set => SetValue(ReplaceTextProperty, value);
    }

    public double MyFontSize
    {
        get => GetValue(MyFontSizeProperty);
        set => SetValue(MyFontSizeProperty, value);
    }

    public bool IsEdit
    {
        get => _isEdit;
        set
        {
            if (value == _isEdit) return;
            _isEdit = value;
            OnPropertyChanged();
        }
    }

    public BorderVisibilitySetting BorderVisibilitySetting
    {
        get => GetValue(BorderVisibilitySettingProperty);
        set => SetValue(BorderVisibilitySettingProperty, value);
    }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private ComboBox Cb;

    public async void ToEdit()
    {
        Cb = new ComboBox
        {
            IsVisible = false,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            ItemsSource = ReplaceText,
            DisplayMemberBinding = new Binding("Value", BindingMode.OneWay),
            FontSize = MyFontSize,
            IsDropDownOpen = true
        };
        Cb.Bind(ComboBox.SelectedItemProperty, new Binding
        {
            Mode = BindingMode.TwoWay,
            Source = this,
            Path = nameof(SelectedText)
        });
        Cb.DropDownClosed += EditBox_OnDropDownClosed;

        Grid.Children.Add(Cb);

        IsEdit = true;
        Cb.IsVisible = true;
    }

    public void ToSave()
    {
        IsEdit = false;
    }

    private void EditLabel_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        ToEdit();
        e.Handled = true;
    }

    private void EditBox_OnDropDownClosed(object? sender, EventArgs e)
    {
        ToSave();
        Grid.Children.Remove(Cb);
    }

    public void Destroy()
    {
        EditLabel.DoubleTapped -= EditLabel_OnDoubleTapped;
        if (Cb != null)
            Cb.DropDownClosed -= EditBox_OnDropDownClosed;
        ClearValue(SelectedTextProperty);
        ClearValue(ReplaceTextProperty);
        ClearValue(MyFontSizeProperty);
        ClearValue(BorderVisibilitySettingProperty);
    }
}