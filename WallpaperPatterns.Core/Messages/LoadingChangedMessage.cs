namespace WallpaperPatterns.Core.Messages
{
    public class LoadingChangedMessage : Cirrious.MvvmCross.Plugins.Messenger.MvxMessage
    {
        public LoadingChangedMessage(object sender)
            : base(sender)
        {
        }
    }
}