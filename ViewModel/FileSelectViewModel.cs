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
using iTextSharp.text.pdf;
using System.Net;


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
        public ICommand SaveAsPDFBtn { get; set; }

        public ICommand UploadBtn { get; set; }
        public FileSelectViewModel()
        {
            OpenFileBtnClick = new RelayCommand(OpenFileBtnClickExecute, OpenFileBtnClickCanExecute);
            SaveAsTextBtn = new RelayCommand(SaveAsTextBtnExecute, SaveAsTextBtnCanExecute);
            SaveAsPDFBtn = new RelayCommand(SaveAsPDFBtnExecute, SaveAsPDFBtnCanExecute);
            UploadBtn = new RelayCommand(UploadBtnExecute, UploadBtnCanExecute);
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
                if (SaveFileDialog.ShowDialog() == true && FilesDetails.SelectedFileContent.Length > 0)
                    File.WriteAllText(SaveFileDialog.FileName, FilesDetails.SelectedFileContent);
                if (SaveFileDialog.FileName.Length > 0)
                {
                    FilesDetails.SelectedFileContent = "";
                    MessageBox.Show("Text File Saved Succesfully");
                }

            }
            catch (System.Exception)
            {
                MessageBox.Show("Invalid File error");
            }
        }

        public bool SaveAsPDFBtnCanExecute(object param)
        {
            return true;
        }

        public void SaveAsPDFBtnExecute(object param)
        {

            SaveFileDialog ConvertToPdf = new SaveFileDialog();
            string FileExtFilter = ConfigurationManager.AppSettings.Get("SupportingFilePDFFilter");
            ConvertToPdf.Filter = FileExtFilter;
            try
            {
                if (ConvertToPdf.ShowDialog() == true && FilesDetails.SelectedFileContent.Length > 0)
                {
                    iTextSharp.text.Document doc = new iTextSharp.text.Document();
                    PdfWriter.GetInstance(doc, new FileStream(ConvertToPdf.FileName, FileMode.Create));
                    doc.Open();
                    doc.Add(new iTextSharp.text.Paragraph(FilesDetails.SelectedFileContent));
                    doc.Close();
                    FilesDetails.SelectedFileContent = "";
                    MessageBox.Show("PDF Saved Succesfully", "PDF File");
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("Invalid File error", "PDF File");
            }
        }

        public bool UploadBtnCanExecute(object param)
        {
            return true;
        }

        public void UploadBtnExecute(object param)
        {


            string HostAddress = ConfigurationManager.AppSettings.Get("HostAddress");
            string UserId = ConfigurationManager.AppSettings.Get("UserId");
            string Password = ConfigurationManager.AppSettings.Get("Password");
            // ConvertToPdf.Filter = FileExtFilter;
            try
            {
                string fileNameOnly = Path.GetFileName(FilesDetails.SelectedFileName);
                //  Console.WriteLine("file name, {0}", result);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(HostAddress + @"/" + fileNameOnly);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(UserId, Password);
                // Copy the contents of the file to the request stream.

                StreamReader sourceStream = new StreamReader(@FilesDetails.SelectedFileName);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                FilesDetails.SelectedFileContent = "";
                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                MessageBox.Show("File Uploaded!", " File");

                response.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Invalid File error", "PDF File");
            }
        }

    }
}
