namespace RomajiConverter.Core.Extensions;

public static class StringExtension
{
    public static string RemoveEmptyLine(this string str)
    {
        if (str == null) return string.Empty;
        var strArray = str.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        strArray = strArray.Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
        return string.Join(Environment.NewLine, strArray);
    }

    public static string[] LineToUnits(this string str)
    {
        return str.Split(new[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries);
    }
}