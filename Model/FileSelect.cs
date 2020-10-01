using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Model
{
    public class Files : INotifyPropertyChanged
    {
        public string fName;
        public string fcontent;
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

        public string selectedFileName
        {
            get
            {
                return fName;
            }
            set
            {
                fName = value;
                OnPropertyChanged("selectedFileName");
            }
        }
        public string selectedFileContent
        {
            get
            {
                return fcontent;
            }
            set
            {
                fcontent = value;
                OnPropertyChanged("selectedFileContent");
            }
        }

    }
}
