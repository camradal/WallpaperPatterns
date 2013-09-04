using System;
using Cirrious.CrossCore.Core;

namespace WallpaperPatterns.Core.Error
{
    public class ErrorApplicationObject
        : MvxMainThreadDispatchingObject
            , IErrorReporter
            , IErrorSource
    {
        public void ReportError(string error, Exception exception)
        {
            if (ErrorReported == null)
                return;

            InvokeOnMainThread(() =>
            {
                EventHandler<ErrorEventArgs> handler = ErrorReported;
                if (handler != null)
                    handler(this, new ErrorEventArgs(error, exception));
            });
        }

        public event EventHandler<ErrorEventArgs> ErrorReported;
    }
}