using MVVM.Model;
using MVVMFileMange.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MVVMFileMange.View
{

    /// <summary>
    /// Interaction logic for SourceFolder.xaml
    /// </summary>
    public partial class SourceFolder : Window
    {
        private Files _FileDetails;
        private BackgroundWorker worker = null;//
        double filesizedownloaded = 0, filesize = 0;//

        //

        //
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
        public ICommand UploadBtnClick { get; set; }
        public ICommand btnCloseClick { get; set; }
        public object Server { get; private set; }
        //List<string> selectedFiles = new List<string>();
        public SourceFolder(List<string> selectedFiles)
        {
            InitializeComponent();
            hello(selectedFiles);
            FileManagement(selectedFiles);
            UploadBtnClick = new RelayCommand(UploadBtnExecute, UploadBtnCanExecute);
            btnCloseClick = new RelayCommand(CloseBtnExecute, CloseBtnCanExecute);
            this.DataContext = this;
            //this.Row = row;

        }

        public bool UploadBtnCanExecute(object param)
        {
            return true;
        }
        public void UploadBtnExecute(object param)
        {

            if (worker == null)
            {
                worker = new BackgroundWorker();
                //Adding method to the invocation list of an existing delegate instance. 
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                //worker.ProgressChanged += worker_ProgressChanged;
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;

            }
            if (worker.IsBusy != true)
            {
                // Start the asynchronous operation.

                worker.RunWorkerAsync(argument: "selectedFiles");
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            //List<string> selectedFiles = (List<string>)e.Argument;     
            //MessageBox.Show(selectedFiles[1]);      

            string HostAddress = ConfigurationManager.AppSettings.Get("HostAddress");
            string UserId = ConfigurationManager.AppSettings.Get("UserId");
            string Password = ConfigurationManager.AppSettings.Get("Password");
            string port = ConfigurationManager.AppSettings.Get("port");
            try
            {
                string fileNameOnly = Path.GetFileName("s");
                //  Console.WriteLine("file name, {0}", result);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(HostAddress + @"/" + fileNameOnly+":" +port);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(UserId, Password);
                // Copy the contents of the file to the request stream.

                StreamReader sourceStream = new StreamReader(@FilesDetails.SelectedFileName);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;
                filesize = ConvertBytesToMegabytes(request.ContentLength);
                Console.WriteLine("hello,{0}", filesize);
                // Console.WriteLine("hellosss,{0}", request.ContentLength);
                Stream requestStream = request.GetRequestStream();
                Console.WriteLine("here,{0}", requestStream); 



                requestStream.Write(fileContents, 0, fileContents.Length);
                filesizedownloaded = ConvertBytesToMegabytes(requestStream.Length);
                Console.WriteLine("hellosss,{0}", filesizedownloaded);
                worker.ReportProgress(Convert.ToInt32((Convert.ToInt32(filesizedownloaded) / filesize) * 100));

                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                FilesDetails.SelectedFileContent = "";
                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                MessageBox.Show("File Uploaded!", " File");
                response.Close();
            }
            catch
            {
                MessageBox.Show("Your internet connection appears to be down or URL not found. Please check it and try again",
                    "Communications Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }



           


        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.CancelAsync();
            worker = null;
        }
        public void CloseBtnExecute(object param)
        {
            this.Close();
        }

        public bool CloseBtnCanExecute(object param)
        {
            return true;
        }

        public void hello(List<string> SelectedFiles)
        {
            List<string> selected = new List<string>();
            string fileName = string.Empty;
            foreach (string singleFile in SelectedFiles)
            {
               
                fileName = System.IO.Path.GetFileName(singleFile);
                string name = fileName;
                selected.Add(fileName);

                selected.Add(new string(Name = fileName));
               
            }
            try
            {
                
                listbox.DataContext = selected;
                //FilesDetails.SelectedFiles = SelectedFiles;
                //lvDataBinding.ItemsSource = SelectedFiles;

                //selected = selectedFiles;

            }

            //FilesDetails.SelectedFiles = selectedFiles;
            //files.SelectedFile= selectedFiles;        
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }


        static double ConvertBytesToMegabytes(long bytes)
        {
            //convert bytes to  GB
            return (bytes / 1024f) / 1024f;
        }
        public void FileManagement(List<string> selectedFiles)
        {
            CreateFolders();
            FileCopy(selectedFiles);
        }

        public void CreateFolders()
        {
            var todaysDate = DateTime.Now.ToString("dd-MM-yyyy");
            string rootInputFolder = @"C:\Users\jithu.jose\Desktop\Input";
            string innerFolderWithDate = @"C:\Users\jithu.jose\Desktop\Input\" + todaysDate;

            // If directory does not exist, create it. 
            try
            {
                if (!Directory.Exists(rootInputFolder))
                {
                    Directory.CreateDirectory(rootInputFolder);
                }

                if (!Directory.Exists(innerFolderWithDate))
                {
                    Directory.CreateDirectory(innerFolderWithDate);
                }
            }

            //    DirectoryInfo di = Directory.CreateDirectory(subPath);


            //    //// Delete the directory.
            //    //di.Delete();
            //    //Console.WriteLine("The directory was deleted successfully.");
            catch (Exception ex)
            {
                //code
            }
        }
        private void FileCopy(List<string> selectedFiles)
        {

            var todaysDate = DateTime.Now.ToString("dd-MM-yyyy");
            string destinationPath = @"C:\Users\jithu.jose\Desktop\Input\" + todaysDate;
            string fileName = string.Empty;
            string destFile = string.Empty;

            foreach (string s in selectedFiles)
            {

                fileName = System.IO.Path.GetFileName(s);
                destFile = System.IO.Path.Combine(destinationPath, fileName);
                System.IO.File.Copy(s, destFile, true);
            }
        }


        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    double primScreenHeight = System.Windows.SystemParameters.FullPrimaryScreenHeight;
        //    double primScreenWidth = System.Windows.SystemParameters.FullPrimaryScreenWidth;
        //    this.Top = (primScreenHeight - this.Height) / 2;
        //   this.Left = (primScreenWidth - this.Width) / 2;
        //}




    }
}
