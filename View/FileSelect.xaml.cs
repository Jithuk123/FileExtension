using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MVVM.View
{
    /// <summary>

    /// </summary>
    public partial class FileSelect : Window
    {
        public FileSelect()
        {
            InitializeComponent();

            MVVM.Model.Files Files = new MVVM.Model.Files();

            this.DataContext = Files;

        }
    }
}