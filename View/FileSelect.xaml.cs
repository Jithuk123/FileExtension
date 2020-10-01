using System.Windows;
using MVVM.ViewModel;

namespace MVVM.View
{
    /// <summary>

    /// </summary>
    public partial class FileSelect : Window
    {
        public FileSelect()
        {
            InitializeComponent();

            FileSelectViewModel fileSelectViewModel = new FileSelectViewModel();

            this.DataContext = fileSelectViewModel;
        }





    }
}