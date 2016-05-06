using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CAROIL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            //string[] args = e.Args[0].Split('|');
            //if (args[0] != @"start") { return; }
            //CAROIL.MainWindow.InUser = args[1];

            try
            {
                bool response;
                Mutex m = new Mutex(true, "SingleInstance", out response);
                if (!response) { Application.Current.Shutdown(); }
                MainWindow window = new MainWindow()
                {
                    WindowStartupLocation =  WindowStartupLocation.CenterScreen,
                    ShowTitleBar = false,
                    IsWindowDraggable = true,
                    TitleCaps = false,
                    GlowBrush = new SolidColorBrush(Colors.Black),
                    ShowMaxRestoreButton = false ,
                    ResizeMode =  ResizeMode.CanMinimize
                };
                window.Show();
                
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }
    }
}
