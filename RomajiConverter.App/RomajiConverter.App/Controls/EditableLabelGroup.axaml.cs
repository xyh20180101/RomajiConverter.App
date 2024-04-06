using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using RomajiConverter.App.Enums;
using RomajiConverter.Core.Models;

namespace RomajiConverter.App.Controls;

public partial class EditableLabelGroup : UserControl, INotifyPropertyChanged
{
    public static readonly StyledProperty<ConvertedUnit> UnitProperty =
        AvaloniaProperty.Register<EditableLabelGroup, ConvertedUnit>(nameof(Unit));

    public static readonly StyledProperty<bool> RomajiVisibilityProperty =
        AvaloniaProperty.Register<EditableLabelGroup, bool>(nameof(RomajiVisibility));

    public static readonly StyledProperty<HiraganaVisibility> HiraganaVisibilityProperty =
        AvaloniaProperty.Register<EditableLabelGroup, HiraganaVisibility>(nameof(HiraganaVisibility),
            HiraganaVisibility.Collapsed);

    public static readonly StyledProperty<double> MyFontSizeProperty =
        AvaloniaProperty.Register<EditableLabelGroup, double>(nameof(MyFontSize), 14d);

    public static readonly StyledProperty<BorderVisibilitySetting> BorderVisibilitySettingProperty =
        AvaloniaProperty.Register<EditableLabelGroup, BorderVisibilitySetting>(nameof(BorderVisibilitySetting),
            BorderVisibilitySetting.Hidden);

    private ReplaceString _selectedHiragana;

    private ReplaceString _selectedRomaji;

    public EditableLabelGroup(ConvertedUnit unit)
    {
        InitializeComponent();
        Unit = unit;
        MyFontSize = 14;
        SelectedRomaji = Unit.ReplaceRomaji.First(p => p.Id == unit.SelectId);
        SelectedHiragana = Unit.ReplaceHiragana.First(p => p.Id == unit.SelectId);
        BorderVisibilitySetting = BorderVisibilitySetting.Hidden;
    }

    public ConvertedUnit Unit
    {
        get => GetValue(UnitProperty);
        set => SetValue(UnitProperty, value);
    }

    public bool RomajiVisibility
    {
        get => GetValue(RomajiVisibilityProperty);
        set
        {
            switch (value)
            {
                case true:
                    RomajiLabel.IsEnabled = true;
                    RomajiLabel.Opacity = 1;
                    RomajiLabel.IsVisible = true;
                    break;
                case false:
                    RomajiLabel.IsEnabled = false;
                    RomajiLabel.Opacity = 0;
                    RomajiLabel.IsVisible = false;
                    break;
            }

            SetValue(RomajiVisibilityProperty, value);
        }
    }

    public HiraganaVisibility HiraganaVisibility
    {
        get => GetValue(HiraganaVisibilityProperty);
        set
        {
            switch (value)
            {
                case HiraganaVisibility.Visible:
                    HiraganaLabel.IsEnabled = true;
                    HiraganaLabel.Opacity = 1;
                    HiraganaLabel.IsVisible = true;
                    break;
                case HiraganaVisibility.Collapsed:
                    HiraganaLabel.IsEnabled = false;
                    HiraganaLabel.Opacity = 0;
                    HiraganaLabel.IsVisible = false;
                    break;
                case HiraganaVisibility.Hidden:
                    HiraganaLabel.IsEnabled = false;
                    HiraganaLabel.Opacity = 0;
                    HiraganaLabel.IsVisible = true;
                    break;
            }

            SetValue(HiraganaVisibilityProperty, value);
        }
    }

    public double MyFontSize
    {
        get => GetValue(MyFontSizeProperty);
        set
        {
            SetValue(MyFontSizeProperty, value);
            OnPropertyChanged();
        }
    }

    public BorderVisibilitySetting BorderVisibilitySetting
    {
        get => GetValue(BorderVisibilitySettingProperty);
        set
        {
            SetValue(BorderVisibilitySettingProperty, value);
            OnPropertyChanged();
        }
    }

    public ReplaceString SelectedRomaji
    {
        get => _selectedRomaji;
        set
        {
            if (Equals(value, _selectedRomaji)) return;
            _selectedRomaji = value;
            if (_selectedRomaji.IsSystem)
                SelectedHiragana = Unit.ReplaceHiragana.First(p => p.Id == _selectedRomaji.Id);
            Unit.Romaji = _selectedRomaji.Value;
            Unit.SelectId = _selectedRomaji.Id;
            OnPropertyChanged();
        }
    }

    public ReplaceString SelectedHiragana
    {
        get => _selectedHiragana;
        set
        {
            if (Equals(value, _selectedHiragana)) return;
            _selectedHiragana = value;
            if (_selectedHiragana.IsSystem)
                SelectedRomaji = Unit.ReplaceRomaji.First(p => p.Id == _selectedHiragana.Id);
            Unit.Hiragana = _selectedHiragana.Value;
            Unit.SelectId = _selectedHiragana.Id;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Destroy()
    {
        RomajiLabel.Destroy();
        HiraganaLabel.Destroy();
        ClearValue(UnitProperty);
        ClearValue(RomajiVisibilityProperty);
        ClearValue(HiraganaVisibilityProperty);
        ClearValue(MyFontSizeProperty);
    }
}