using LeseEulenBibliothek.Core;
using LeseEulenBibliothek.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LeseEulenBibliothek
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            string configFilePath = System.IO.File.Exists("config.json") ? "config.json" : "data\\config.json";
            if (e.Args?.Length > 0 && System.IO.File.Exists(e.Args[0]))
                configFilePath = e.Args[0];
            var locale = CultureInfo.CurrentCulture.Name;
            var viewModel = new MainViewModel(configFilePath);
            if (!string.IsNullOrEmpty(viewModel.ConfigView.Data.Language))
                locale = viewModel.ConfigView.Data.Language;
            TranslationService.CurrentLanguage = locale;
            MainWindow mainWindow = new MainWindow(viewModel);
            mainWindow.Show();
        }
    }
}
