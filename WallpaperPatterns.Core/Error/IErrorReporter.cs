using System;

namespace WallpaperPatterns.Core.Error
{
    public interface IErrorReporter
    {
        void ReportError(string error, Exception exception);
    }
}