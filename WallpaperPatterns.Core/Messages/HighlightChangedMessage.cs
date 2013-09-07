namespace WallpaperPatterns.Core.Messages
{
    public class HighlightChangedMessage : Cirrious.MvvmCross.Plugins.Messenger.MvxMessage
    {
        public HighlightChangedMessage(object sender)
            : base(sender)
        {
        }
    }
}