using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using WallpaperPatterns.Core.Error;

namespace WallpaperPatterns.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Client")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            InitalizeErrorSystem();

            RegisterAppStart<ViewModels.PatternGroupViewModel>();
        }

        private void InitalizeErrorSystem()
        {
            var errorHub = new ErrorApplicationObject();
            Mvx.RegisterSingleton<IErrorReporter>(errorHub);
            Mvx.RegisterSingleton<IErrorSource>(errorHub);
        }
    }
}