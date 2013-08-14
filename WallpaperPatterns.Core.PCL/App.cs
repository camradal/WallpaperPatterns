using Cirrious.CrossCore.IoC;

namespace WallpaperPatterns.Core.PCL
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Client")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<ViewModels.PatternsViewModel>();
        }
    }
}