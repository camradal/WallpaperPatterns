using System;

namespace WallpaperPatterns.Core.Error
{
    public interface IErrorSource
    {
        event EventHandler<ErrorEventArgs> ErrorReported;
    }
}