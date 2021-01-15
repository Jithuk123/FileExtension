using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using MVVM.Model;
using Microsoft.Win32;
using System.Configuration;
using MVVMFileMange.Command;

using System.IO;
using iTextSharp.text.pdf;
using System.ComponentModel;
using System.Data;
using WinSCP;
using System.Threading;
using System.Linq;
using Renci.SshNet;

namespace MVVM.ViewModel
{

    public class FileSelectViewModel
    {

        private BackgroundWorker worker = null;//
        double filesizedownloaded = 0, filesize = 0;//
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
            //UploadBtn = new RelayCommand(UploadBtnExecute, UploadBtnCanExecute);
            _FileDetails = new Files();
            sftpServerWatcher();
        }




        private void OnPropertyChanged(string v)
        {
            throw new NotImplementedException();
        }

        //private SourceFolder _DetailView;
        //public SourceFolder DetailView
        //{
        //    get
        //    {
        //        return _DetailView;
        //    }
        //    set
        //    {
        //        if (_DetailView != value)
        //        {
        //            _DetailView = value;
        //            RaisePropertyChanged(() => this.DetailView);
        //        }
        //    }
        //}



        public bool OpenFileBtnClickCanExecute(object param)
        {
            return true;
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


        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.CancelAsync();
            worker = null;
        }

        //public void UploadBtnExecute(object param)
        //{ 

        //    FilesDetails.progressBar1 = 100;
        //    if (worker == null)
        //    {
        //        worker = new BackgroundWorker();
        //        //Adding method to the invocation list of an existing delegate instance. 
        //        worker.DoWork += worker_DoWork;
        //        worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        //        worker.ProgressChanged += worker_ProgressChanged;
        //        worker.WorkerReportsProgress = true;
        //        worker.WorkerSupportsCancellation = true;

        //    }
        //    if (worker.IsBusy != true)
        //    {
        //        // Start the asynchronous operation.
        //        worker.RunWorkerAsync();
        //    }
        //}
        static double ConvertBytesToMegabytes(long bytes)
        {
            //convert bytes to  GB
            return (bytes / 1024f) / 1024f;
        }

        //private void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    string HostAddress = ConfigurationManager.AppSettings.Get("HostAddress");
        //    string UserId = ConfigurationManager.AppSettings.Get("UserId");
        //    string Password = ConfigurationManager.AppSettings.Get("Password");
        //    try
        //    {
        //        string fileNameOnly = Path.GetFileName(FilesDetails.SelectedFileName);
        //        //  Console.WriteLine("file name, {0}", result);
        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(HostAddress + @"/" + fileNameOnly);
        //        request.Method = WebRequestMethods.Ftp.UploadFile;

        //        request.Credentials = new NetworkCredential(UserId, Password);
        //        // Copy the contents of the file to the request stream.

        //        StreamReader sourceStream = new StreamReader(@FilesDetails.SelectedFileName);
        //        byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
        //        sourceStream.Close();
        //        request.ContentLength = fileContents.Length;
        //        filesize = ConvertBytesToMegabytes(request.ContentLength);
        //        Console.WriteLine("hello,{0}", filesize);
        //        // Console.WriteLine("hellosss,{0}", request.ContentLength);
        //        Stream requestStream = request.GetRequestStream();
        //        Console.WriteLine("here,{0}", requestStream);



        //        requestStream.Write(fileContents, 0, fileContents.Length);
        //        filesizedownloaded = ConvertBytesToMegabytes(requestStream.Length);
        //        Console.WriteLine("hellosss,{0}", filesizedownloaded);
        //        worker.ReportProgress(Convert.ToInt32((Convert.ToInt32(filesizedownloaded) / filesize) * 100));

        //        requestStream.Close();
        //        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        //        FilesDetails.SelectedFileContent = "";
        //        Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
        //        MessageBox.Show("File Uploaded!", " File");
        //        response.Close();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Your internet connection appears to be down or URL not found. Please check it and try again",
        //            "Communications Error", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}


        //void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{

        //    FilesDetails.progressBar1 = e.ProgressPercentage;
        //    Console.WriteLine("here,{0}", FilesDetails.progressBar1);

        //    if ((FilesDetails.progressBar1 / 2) == 0)
        //    {
        //        FilesDetails.lblStatus = "Downloading.";
        //    }
        //    else if ((FilesDetails.progressBar1 / 3) == 0)
        //    {
        //        FilesDetails.lblStatus = "Downloading..";
        //    }
        //    else if ((FilesDetails.progressBar1 / 5) == 0)
        //    {
        //        FilesDetails.lblStatus = "Downloading...";
        //    }

        //    FilesDetails.lblStatus = "Download " + filesizedownloaded.ToString("F2") + " / " + filesize.ToString("F2")
        //        + " ( " + FilesDetails.progressBar1.ToString() + " ) % Complete.";
        //}



        private void worker_DoWorks(object sender, DoWorkEventArgs e)
        {
            var ode = new Files();
            string FileExtType = ConfigurationManager.AppSettings.Get("SupportingFileType");
            string FileExtFilter = ConfigurationManager.AppSettings.Get("SupportingFileTxtFilter");

            OpenFileDialog openFileDlg = new OpenFileDialog();

            openFileDlg.Multiselect = true;
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
                List<string> selectedFile = new List<string>();
                if (result == true)
                {
                    foreach (string sltfiles in openFileDlg.FileNames)
                    {
                        selectedFile.Add(sltfiles);

                    }
                    FilesDetails.SelectedFiles = selectedFile;

                    //Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        var test = new MVVMFileMange.View.SourceFolder(selectedFile);
                        test.ShowDialog();
                    };
                    // should consider this while writing code 

                    //FilesDetails.SelectedFileName = openFileDlg.FileName;
                    //FilesDetails.SelectedFileContent = System.IO.File.ReadAllText(openFileDlg.FileName);

                }
            }
            catch (Exception ex)

            {

                MessageBox.Show(ex.Message, MessageBoxButton.OK.ToString());
            }
        }
        public void OpenFileBtnClickExecute(object param)
        {
            var ode = new Files();
            string FileExtType = ConfigurationManager.AppSettings.Get("SupportingFileType");
            string FileExtFilter = ConfigurationManager.AppSettings.Get("SupportingFileTxtFilter");

            OpenFileDialog openFileDlg = new OpenFileDialog();

            openFileDlg.Multiselect = true;
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
                List<string> selectedFile = new List<string>();
                if (result == true)
                {
                    foreach (string sltfiles in openFileDlg.FileNames)
                    {
                        selectedFile.Add(sltfiles);

                    }
                    FilesDetails.SelectedFiles = selectedFile;


                    {
                        var test = new MVVMFileMange.View.SourceFolder(selectedFile);
                        test.ShowDialog();
                    };
                    // should consider this while writing code 

                    //FilesDetails.SelectedFileName = openFileDlg.FileName;
                    //FilesDetails.SelectedFileContent = System.IO.File.ReadAllText(openFileDlg.FileName);

                }
            }
            catch (Exception ex)

            {

                MessageBox.Show(ex.Message, MessageBoxButton.OK.ToString());
            }
            //if (worker == null)
            //{
            //    worker = new BackgroundWorker();
            //    //Adding method to the invocation list of an existing delegate instance.
            //    worker.DoWork += worker_DoWorks;
            //    worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            //    worker.WorkerSupportsCancellation = true;
            //}
            //if (worker.IsBusy != true)
            //{
            //    // Start the asynchronous operation.
            //    worker.RunWorkerAsync();
            //}
        }

        public int sftpServerWatcher()
        {
            var todaysDate = DateTime.Now.ToString("dd-MM-yyyy");
            string resultLocalPath = @"C:\Users\jithu.jose\Desktop\Results\" + todaysDate;
            try
            {
                if (!Directory.Exists(resultLocalPath))
                {
                    Directory.CreateDirectory(resultLocalPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                // Setup session options
                WinSCP.SessionOptions sessionOptions = new WinSCP.SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = ConfigurationManager.AppSettings.Get("HostAddress"),
                    UserName = ConfigurationManager.AppSettings.Get("UserId"),
                    Password = ConfigurationManager.AppSettings.Get("Password"),
                    SshHostKeyFingerprint = "ssh-ed25519 255 qUGYw97LMnEx+lW/85u0sux6DK7vHetI1r5ZQcGRmHY=ssh - ed25519 255 ed:9f:cb: a1: d2: b0: af: 44:b7: d5: 0e:7d:57:33:fb: 7c",

                };
                //string uploadFolderWithDate = @"C:\Users\jithu.jose\Desktop\Results";
                string resultRemotePath = ConfigurationManager.AppSettings.Get("ResultRemoteFile");



                using (WinSCP.Session session = new WinSCP.Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    List<string> prevFiles = null;


                    while (true)
                    {
                        SynchronizationResult result =
                            session.SynchronizeDirectories(
                                SynchronizationMode.Local, resultRemotePath, resultLocalPath, true);
                        result.Check();
                        // You can inspect result.Downloads for a list for updated files

                        Thread.Sleep(10000);
                    }
                    //while (true)
                    //{
                    //    // Collect file list
                    //    List<string> files =
                    //        session.EnumerateRemoteFiles(
                    //            remotePath, "*.*", WinSCP.EnumerationOptions.AllDirectories)
                    //            .Select(fileInfo => fileInfo.FullName)
                    //            .ToList();
                    //    if (prevFiles == null)
                    //    {
                    //        // In the first round, just print number of files found
                    //        Console.WriteLine("Found {0} files", files.Count);
                    //    }
                    //    else
                    //    {
                    //        // Then look for differences against the previous list
                    //        IEnumerable<string> added = files.Except(prevFiles);
                    //        if (added.Any())
                    //        {
                    //            Console.WriteLine("Added files:");
                    //            foreach (string path in added)
                    //            {
                    //                DownloadFile();
                    //            }
                    //        }

                    //        IEnumerable<string> removed = prevFiles.Except(files);
                    //        if (removed.Any())
                    //        {
                    //            Console.WriteLine("Removed files:");
                    //            foreach (string path in removed)
                    //            {
                    //                Console.WriteLine(path);
                    //            }
                    //        }
                    //    }

                    //    prevFiles = files;

                    //    Console.WriteLine("Sleeping 10s...");
                    //    Thread.Sleep(10000);
                    //}
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                return 1;
            }
        }


        /////
        public void DownloadFile(string remoteFilePath, string localFilePath)
        {
            string hostName = ConfigurationManager.AppSettings.Get("HostAddress");
            string userName = ConfigurationManager.AppSettings.Get("UserId");
            string password = ConfigurationManager.AppSettings.Get("Password");
            string port = ConfigurationManager.AppSettings.Get("Port");
            using var client = new SftpClient(hostName, Int32.Parse(port), userName, password);
            try
            {
                client.Connect();
                using var s = File.Create(localFilePath);
                client.DownloadFile(remoteFilePath, s);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                client.Disconnect();
            }
        }

        //////////
    }
}
