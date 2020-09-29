using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMFileMange
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MVVM.View.FileSelect files = new MVVM.View.FileSelect();
            MVVM.ViewModel.FileSelectViewModel FileSelectViewModel = new MVVM.ViewModel.FileSelectViewModel();
            files.DataContext = FileSelectViewModel;
            files.Show();
        }
    }
}
