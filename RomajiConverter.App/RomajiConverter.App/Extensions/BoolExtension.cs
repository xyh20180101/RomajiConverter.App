namespace RomajiConverter.App.Extensions;

public static class BoolExtension
{
    public static bool ToBool(this bool? value)
    {
        return value ?? false;
    }
}