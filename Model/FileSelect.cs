using System.ComponentModel;

namespace MVVM.Model
{
    public class Files : INotifyPropertyChanged
    {
        private string _fName;
        private  string _fContent;
        
        //Define an event based on delegates
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

        public string SelectedFileName
        {
            get
            {
                return _fName;
            }
            set
            {
                _fName = value;
                OnPropertyChanged("SelectedFileName");
            }
        }
        public string SelectedFileContent
        {
            get
            {
                return _fContent;
            }
            set
            {
                _fContent = value;
                OnPropertyChanged("SelectedFileContent");
            }
        }
    }
}
