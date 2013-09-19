namespace WallpaperPatterns.Core.Messages
{
    public class LoadingChangedMessage : Cirrious.MvvmCross.Plugins.Messenger.MvxMessage
    {
        public bool Loading { get; private set; }

        public LoadingChangedMessage(object sender, bool loading)
            : base(sender)
        {
            Loading = loading;
        }
    }
}