using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MVVM.Model;
using Microsoft.Win32;

namespace MVVM.ViewModel
{
    public class FileSelectViewModel
    {


        private ICommand mUpdater;
        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }
    }
    class Updater : ICommand
    {
        private string fileName;
        private string fileContent;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            // Set filter for file extension and default file extension  
            openFileDlg.DefaultExt = ".txt";
            openFileDlg.Filter = "Text documents (.txt)|*.txt";

            // Launch OpenFileDialog by calling ShowDialog method
            //specifies whether the activity was accepted (true) or canceled (false).
            //ShowDialog() is called on a window that is closing (Closing) or has been closed (Closed).
            try
            {
                Nullable<bool> result = openFileDlg.ShowDialog();
                // Get the selected file name and display in a TextBox.
                // Load content of file in a TextBlock
                if (result == true)
                {
                    fileName = openFileDlg.FileName;
                    fileContent = System.IO.File.ReadAllText(openFileDlg.FileName);
                }
            }
            catch (System.Exception)
            {

                MessageBox.Show("A handled exception just occurred");
            }

        }
    }

    #endregion
}




