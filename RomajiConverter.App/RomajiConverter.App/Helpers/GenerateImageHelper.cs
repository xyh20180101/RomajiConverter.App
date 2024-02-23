using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Platform;
using RomajiConverter.App.Models;
using SkiaSharp;

namespace RomajiConverter.App.Helpers;

public static class GenerateImageHelper
{
    public static SKImage ToImage(this List<string[][]> list, ImageSetting setting)
    {
        if (list.Any() == false || list[0].Any() == false || list[0][0].Any() == false)
            return SKImage.Create(new SKImageInfo(1, 1));

        var fontSize = setting.FontPixelSize;
        var pagePadding = setting.PagePadding;
        var textMargin = setting.TextMargin;
        var lineMargin = setting.LineMargin;
        var linePadding = setting.LinePadding;

        var typeface = SKTypeface.FromStream(AssetLoader.Open(new Uri($"avares://RomajiConverter.App/Assets/Fonts/{setting.FontFamilyName}")));
        var paint = new SKPaint
        {
            Typeface = typeface,
            TextSize = fontSize,
            Color = setting.FontColor,
            IsAntialias = true
        };
        var background = setting.BackgroundColor;

        //(最长句的渲染长度,该句的单元数)
        var longestLine = list
            .Select(p => new { MaxLength = p.Sum(q => GetUnitLength(q, paint, setting.WordMargin)), UnitCount = p.Length })
            .MaxBy(p => p.MaxLength);

        //最长句子的渲染长度
        var maxLength = longestLine?.MaxLength ?? 0;
        //最大单元数
        var maxUnitCount = longestLine?.UnitCount ?? 0;
        //图片宽度
        var width = maxLength + maxUnitCount * textMargin + pagePadding * 2;
        //图片高度
        var height = list.Count * (list[0][0].Length * fontSize + linePadding) + list.Count * lineMargin +
                     pagePadding * 2;

        var imageInfo = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);

        using var imageSurface = SKSurface.Create(imageInfo);

        var canvas = imageSurface.Canvas;
        canvas.Clear(background);

        for (var i = 0; i < list.Count; i++)
        {
            var line = list[i];
            var startX = pagePadding + textMargin;
            foreach (var unit in line)
            {
                var unitLength = GetUnitLength(unit, paint, setting.WordMargin);
                var renderXArray = unit.Select(str => startX + GetStringXOffset(str, paint, unitLength)).ToArray();
                var renderYArray = unit.Select((str, index) =>
                    pagePadding + (fontSize * unit.Length + linePadding + lineMargin) * i +
                    index * (fontSize + linePadding) + paint.TextSize).ToArray(); //SkiaSharp的绘制和System.Drawing不一样,这里Y方向要加一个paint.TextSize的偏移值
                for (var j = 0; j < unit.Length; j++)
                    canvas.DrawText(unit[j], renderXArray[j], renderYArray[j], paint);
                startX += unitLength + textMargin;
            }
        }

        return imageSurface.Snapshot();
    }

    /// <summary>
    /// 获取单元长度(最长字符串渲染长度)
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="paint"></param>
    /// <param name="wordMargin"></param>
    /// <returns></returns>
    private static int GetUnitLength(string[] unit, SKPaint paint, float wordMargin)
    {
        return unit.Any() ? (int)unit.Max(paint.MeasureText) + (int)(paint.TextSize * wordMargin) : 0;
    }

    /// <summary>
    /// 获取字符串的渲染坐标x轴偏移值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="paint"></param>
    /// <param name="unitLength"></param>
    /// <returns></returns>
    private static float GetStringXOffset(string str, SKPaint paint, float unitLength)
    {
        var textWidth = paint.MeasureText(str);
        return (unitLength - textWidth) / 2;
    }

    public class ImageSetting
    {
        public ImageSetting()
        {
        }

        public ImageSetting(MyConfig config)
        {
            FontFamilyName = config.FontFamilyName;
            FontPixelSize = config.FontPixelSize;
            PagePadding = config.PagePadding;
            TextMargin = config.TextMargin;
            WordMargin = config.WordMargin;
            LineMargin = config.LineMargin;
            LinePadding = config.LinePadding;
            BackgroundColor = SKColor.Parse(config.BackgroundColor);
            FontColor = SKColor.Parse(config.FontColor);
        }

        public string FontFamilyName { get; set; }

        public int FontPixelSize { get; set; }

        public int PagePadding { get; set; }

        public int TextMargin { get; set; }

        public float WordMargin { get; set; }

        public int LineMargin { get; set; }

        public int LinePadding { get; set; }

        public SKColor BackgroundColor { get; set; }

        public SKColor FontColor { get; set; }
    }
}