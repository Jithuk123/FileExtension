//using MVVM.Model;
//using MVVMFileMange.Command;
//using Renci.SshNet;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Windows;
//using System.Windows.Input;

//namespace MVVMFileMange.ViewModel
//{
//    class UploadWindowViewModel
//    {
//        private Files _FileDetails;
//        private BackgroundWorker worker = null;//
//        double filesizedownloaded = 0, filesize = 0;//

//        //

//        //
//        public Files FilesDetails
//        {
//            get
//            {
//                return _FileDetails;
//            }
//            set
//            {
//                _FileDetails = value;
//            }
//        }
//        public ICommand UploadBtnClick { get; set; }
//        public ICommand btnCloseClick { get; set; }
//        //public ICommand<Window> btnCloseClick { get; private set; }
//        public object Server { get; private set; }
//        //List<string> selectedFiles = new List<string>();
//        private List<string> selectedFiles;
//        public UploadWindowViewModel(List<string> selectedFiles)
//        {

//            hello(selectedFiles);
//            FileManagement(selectedFiles);
//            UploadBtnClick = new RelayCommand(UploadBtnExecute, UploadBtnCanExecute);
//            btnCloseClick = new RelayCommand(CloseBtnExecute, CloseBtnCanExecute);
//            //this.DataContext = this;
//            //this.Row = row;
//            this.selectedFiles = selectedFiles;
//            //this.btnCloseClick = new ICommand<Window>(this.CloseWindow);



//        }

//        //private void UploadBtnClick(object sender, RoutedEventArgs e)

//        //{

//        //    string HostAddress = ConfigurationManager.AppSettings.Get("HostAddress");
//        //    string UserId = ConfigurationManager.AppSettings.Get("UserId");
//        //    string Password = ConfigurationManager.AppSettings.Get("Password");
//        //    string port = ConfigurationManager.AppSettings.Get("port");
//        //    var data = selectedFiles;

//        //}

//        public bool UploadBtnCanExecute(object param)
//        {
//            return true;
//        }
//        public void UploadBtnExecute(object param)
//        {

//            if (worker == null)
//            {
//                worker = new BackgroundWorker();
//                //Adding method to the invocation list of an existing delegate instance. 
//                worker.DoWork += worker_DoWork;
//                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
//                //worker.ProgressChanged += worker_ProgressChanged;
//                worker.WorkerReportsProgress = true;
//                worker.WorkerSupportsCancellation = true;

//            }
//            if (worker.IsBusy != true)
//            {
//                // Start the asynchronous operation.

//                worker.RunWorkerAsync(argument: "selectedFiles");
//            }
//        }

//        private void worker_DoWork(object sender, DoWorkEventArgs e)
//        {
//            var todaysDate = DateTime.Now.ToString("dd-MM-yyyy");
//            string uploadFolderWithDate = @"C:\Users\jithu.jose\Desktop\Upload\" + todaysDate;
//            string inputFolderPath = @"C:\Users\jithu.jose\Desktop\input\" + todaysDate;

//            if (!Directory.Exists(uploadFolderWithDate))
//            {
//                Directory.CreateDirectory(uploadFolderWithDate);
//            }


//            string hostAddress = ConfigurationManager.AppSettings.Get("HostAddress");
//            string userId = ConfigurationManager.AppSettings.Get("UserId");
//            string password = ConfigurationManager.AppSettings.Get("Password");
//            string port = ConfigurationManager.AppSettings.Get("Port");
//            var inputfiles = selectedFiles;
//            foreach (string uploadingFile in selectedFiles)
//            {
//                try
//                {

//                    FileInfo f = new FileInfo(uploadingFile);
//                    string uploadfile = f.FullName;
//                    Console.WriteLine(f.Name);
//                    Console.WriteLine("uploadfile" + uploadfile);
//                    var client = new SftpClient(hostAddress, Int32.Parse(port), userId, password);
//                    client.Connect();
//                    if (client.IsConnected)
//                    {
//                        Console.WriteLine("I AM CONNECTED");
//                    }
//                    var fileStream = new FileStream(uploadfile, FileMode.Open);
//                    if (fileStream != null)
//                    {
//                        Console.WriteLine("YOU ARE NOT NULL");
//                    }
//                    client.BufferSize = 4 * 1024;
//                    client.UploadFile(fileStream, "/test1/" + f.Name, null);


//                    client.Disconnect();
//                    client.Dispose();
//                    string fileName = f.Name;
//                    IEnumerable<string> files = Directory.GetFiles(inputFolderPath).Where(f => f.Contains(fileName));
//                    string sourceFilePath = files.FirstOrDefault();
//                    System.IO.File.Move(sourceFilePath, uploadFolderWithDate + "/" + f.Name);


//                    var test = new SourceFolder(null);
//                    test.Close();
//                    MessageBox.Show("Upload Sucess!!");
//                    //string str = uploadFolderWithDate, Replace();
//                    //if (!File.Exists(str))
//                    //{
//                    //    File.Copy(file, str);
//                    //}
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message,
//                        "Communications Error", MessageBoxButton.OK, MessageBoxImage.Information);
//                }
//            }

//        }
//        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
//        {
//            worker.CancelAsync();
//            worker = null;
//        }
//        public void CloseBtnExecute(object param)
//        {
//            this.Close();
//        }

//        public bool CloseBtnCanExecute(object param)
//        {
//            return true;
//        }

//        public void hello(List<string> SelectedFiles)
//        {

//            List<string> selected = new List<string>();
//            string fileName = string.Empty;
//            foreach (string singleFile in SelectedFiles)
//            {

//                fileName = System.IO.Path.GetFileName(singleFile);
//                string name = fileName;
//                selected.Add(fileName);

//                //selected.Add(new string(Name = fileName));

//            }
//            listbox.ItemsSource = selected;
//            try
//            {
//                listbox.DataContext = selected;//this will work if you change to mvvm pattern
//                                               // FilesDetails.SelectedFiles = SelectedFiles;
//                                               //lvDataBinding.ItemsSource = SelectedFiles;

//                //selected = selectedFiles;
//            }

//            //FilesDetails.SelectedFiles = selectedFiles;
//            //files.SelectedFile= selectedFiles;        
//            catch (Exception ex)
//            {
//                //MessageBox.Show(ex.Message);
//            }
//        }


//        static double ConvertBytesToMegabytes(long bytes)
//        {
//            //convert bytes to  GB
//            return (bytes / 1024f) / 1024f;
//        }
//        public void FileManagement(List<string> selectedFiles)
//        {
//            CreateFolders();
//            FileCopy(selectedFiles);
//        }

//        public void CreateFolders()
//        {
//            var todaysDate = DateTime.Now.ToString("dd-MM-yyyy");
//            string rootInputFolder = @"C:\Users\jithu.jose\Desktop\Input";
//            string innerFolderWithDate = @"C:\Users\jithu.jose\Desktop\Input\" + todaysDate;

//            // If directory does not exist, create it. 
//            try
//            {
//                if (!Directory.Exists(rootInputFolder))
//                {
//                    Directory.CreateDirectory(rootInputFolder);
//                }

//                if (!Directory.Exists(innerFolderWithDate))
//                {
//                    Directory.CreateDirectory(innerFolderWithDate);
//                }
//            }

//            //    DirectoryInfo di = Directory.CreateDirectory(subPath);


//            //    //// Delete the directory.
//            //    //di.Delete();
//            //    //Console.WriteLine("The directory was deleted successfully.");
//            catch (Exception ex)
//            {
//                //code
//            }
//        }
//        private void FileCopy(List<string> selectedFiles)
//        {

//            var todaysDate = DateTime.Now.ToString("dd-MM-yyyy");
//            string destinationPath = @"C:\Users\jithu.jose\Desktop\Input\" + todaysDate;
//            string fileName = string.Empty;
//            string destFile = string.Empty;

//            foreach (string s in selectedFiles)
//            {

//                fileName = System.IO.Path.GetFileName(s);
//                destFile = System.IO.Path.Combine(destinationPath, fileName);
//                System.IO.File.Copy(s, destFile, true);
//            }

//        }


//        //private void Window_Loaded(object sender, RoutedEventArgs e)
//        //{
//        //    double primScreenHeight = System.Windows.SystemParameters.FullPrimaryScreenHeight;
//        //    double primScreenWidth = System.Windows.SystemParameters.FullPrimaryScreenWidth;
//        //    this.Top = (primScreenHeight - this.Height) / 2;
//        //   this.Left = (primScreenWidth - this.Width) / 2;
//        //}




//    }
//}

