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
using System.IO;

namespace MVVM.ViewModel
{
    public class FileSelectViewModel
    {
        private Files _FileDetails;

        public Files FilesDetails
        {
            get
            {
                return _FileDetails;
            }
            set
            {
                _FileDetails = value;
            }
        }

        public ICommand OpenFileBtnClick { get; set; }
        public ICommand SaveAsTextBtn { get; set; }
        public FileSelectViewModel()
        {
            OpenFileBtnClick = new RelayCommand(OpenFileBtnClickExecute, OpenFileBtnClickCanExecute);
            SaveAsTextBtn = new RelayCommand(SaveAsTextBtnExecute, SaveAsTextBtnCanExecute);
            _FileDetails = new Files();
        }
        public bool OpenFileBtnClickCanExecute(object param)
        {
            return true;
        }
        public void OpenFileBtnClickExecute(object param)
        {
            string FileExtType = ConfigurationManager.AppSettings.Get("SupportingFileType");
            string FileExtFilter = ConfigurationManager.AppSettings.Get("SupportingFileTxtFilter");

            OpenFileDialog openFileDlg = new OpenFileDialog();
            // Set filter for file extension and default file extension  
            openFileDlg.DefaultExt = FileExtType;
            openFileDlg.Filter = FileExtFilter;


            // Launch OpenFileDialog by calling ShowDialog method
            //specifies whether the activity was accepted (true) or canceled (false).
            //ShowDialog() is called on a window that is closing (Closing) or has been closed (Closed).
            try
            {
                Nullable<bool> result = openFileDlg.ShowDialog();
                // Get the selecteds file name and display in a TextBox.
                // Load content of file in a TextBlock
                if (result == true)
                {
                    FilesDetails.SelectedFileName = openFileDlg.FileName;
                    FilesDetails.SelectedFileContent = System.IO.File.ReadAllText(openFileDlg.FileName);
                }
            }
            catch (System.Exception)
            {

                MessageBox.Show("A handled exception just occurred");
            }
        }
        public bool SaveAsTextBtnCanExecute(object param)
        {
            return true;
        }

        public void SaveAsTextBtnExecute(object param)
        {
            string FileExtFilter = ConfigurationManager.AppSettings.Get("SupportingFileTxtFilter");          
            try
            {
                SaveFileDialog SaveFileDialog = new SaveFileDialog();
                SaveFileDialog.Filter = FileExtFilter;            
                if (SaveFileDialog.ShowDialog() == true&&  FilesDetails.SelectedFileContent.Length > 0)
                File.WriteAllText(SaveFileDialog.FileName, FilesDetails.SelectedFileContent);
                MessageBox.Show("Text file created.");
            }
            catch (System.Exception)
            {
                MessageBox.Show("Invalid File error");
            }
        }

    }
}
