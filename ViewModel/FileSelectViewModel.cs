using System;
using System.Windows;
using System.Windows.Input;
using MVVM.Model;
using Microsoft.Win32;
using MVVMFileMange.Command;
using System.ComponentModel;

namespace MVVM.ViewModel
{
    public class FileSelectViewModel : INotifyPropertyChanged
    {
        public ICommand relayCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public FileSelectViewModel()
        {
            relayCommand = new RelayCommand(Execute, CanExecute);
            _fileDetails = new Files();
        }

        private Files _fileDetails;
        public Files FileDetails
        {
            get
            {
                return _fileDetails;
            }
            set
            {
                _fileDetails = value;
                OnPropertyChanged("FileDetails");
            }
        }
        private string _fileContent;
        public string FileContent
        {
            get
            {
                return _fileContent;
            }
            set
            {
                _fileContent = value;
                OnPropertyChanged(nameof(FileContent));
            }
        }

        //Raise the event
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                //publishing the event in current classs
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
                    FileDetails.SelectedFileName = openFileDlg.FileName;
                    FileDetails.SelectedFileContent = System.IO.File.ReadAllText(openFileDlg.FileName);

                    FileContent = openFileDlg.FileName;
                }

            }
            catch (System.Exception)
            {

                MessageBox.Show("A handled exception just occurred");
            }
        }
    }
}
