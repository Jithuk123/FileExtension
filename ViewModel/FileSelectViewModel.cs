using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MVVM.Model;
using Microsoft.Win32;
using System.Configuration;
using MVVMFileMange.Command;
using System.ComponentModel;

namespace MVVM.ViewModel
{
    public class FileSelectViewModel:INotifyPropertyChanged
    {
       
        public ICommand relayCommand { get; set; }
        public Files FilesDetails { 
            get
            {
                return _FileDetails;
            }
            set
            {
                _FileDetails = value;
                OnPropertyChanged("FilesDetails");
            }}
  public string FilesContent { 
            get
            {
                return _Filecontent;
            }
            set
            {
                _Filecontent = value;
                OnPropertyChanged("FilesContent");
            }}
            private Files _FileDetails;
            private string _Filecontent;
             public event PropertyChangedEventHandler PropertyChanged;
        //Raise the event
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                //publishing the event in current classs
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public FileSelectViewModel()
        {
            relayCommand = new RelayCommand(Execute,CanExecute);
            _FileDetails=new Files();
        }
        public bool CanExecute(object param)
        {
            return true;
        }
        public void Execute(object param)
        {
            // var file= new Files();
            //     var supportFileFormat = ConfigurationManager.AppSettings["fileFormat"];
            //   Console.WriteLine(supportFileFormat); 
            OpenFileDialog openFileDlg = new OpenFileDialog();
            // Set filter for file extension and default file extension  
            openFileDlg.DefaultExt = ".txt";
            openFileDlg.Filter = "Text documents (.txt)|*.txt";
            //  openFileDlg.DefaultExt = supportFileFormat;

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
                    FilesDetails.selectedFileName = openFileDlg.FileName;
                    FilesDetails.selectedFileContent = System.IO.File.ReadAllText(openFileDlg.FileName);

                    FilesContent= openFileDlg.FileName;
                }
                
            }
            catch (System.Exception)
            {

                MessageBox.Show("A handled exception just occurred");
            }
        }
    }
}
    
/*
        public ICommand mUpdater;
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
        public string fileName { get; set; }
        // private string fileName;
        public string fileContent { get; set; }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }
        //Raised when something has changed that will affect the ability of commands to execute.
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
           

        }
    }

    #endregion
}
*/



