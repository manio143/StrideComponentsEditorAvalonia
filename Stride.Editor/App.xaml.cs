using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

namespace Stride.Editor
{
    public class App : Application
    {
        //public const string ResourceRoot = "Stride.Editor.Avalonia";
        //public static Styles FluentDark = new Styles
        //{
        //    new StyleInclude(new Uri($"avares://{ResourceRoot}/Styles"))
        //    {
        //        Source = new Uri("avares://Avalonia.Themes.Fluent/Accents/FluentDark.xaml")
        //    },
        //};

        //public static Styles FluentLight = new Styles
        //{
        //    new StyleInclude(new Uri($"avares://{ResourceRoot}/Styles"))
        //    {
        //        Source = new Uri("avares://Avalonia.Themes.Fluent/Accents/FluentLight.xaml")
        //    },
        //};

        public override void Initialize()
        {
            //Styles.Insert(0, FluentDark); no Fluent style in 0.9.9

            AvaloniaXamlLoader.Load(this);
        }

        public static MainWindow dbgWindow;

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                desktopLifetime.MainWindow = dbgWindow = new MainWindow();
            else throw new NotSupportedException("Expected ApplicationLifetime to be Desktop");
            //else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
            //    singleViewLifetime.MainView = new MainView();

            base.OnFrameworkInitializationCompleted();
        }
    }
}
