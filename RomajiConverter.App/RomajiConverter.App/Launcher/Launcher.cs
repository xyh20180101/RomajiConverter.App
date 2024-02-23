using System;
using System.Threading.Tasks;

namespace RomajiConverter.App.Launcher;

public abstract class Launcher
{
    public abstract Task LaunchUriAsync(Uri uri);
}