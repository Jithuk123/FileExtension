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
        private string fName;
        private string fcontent;
        //Define an event based on delegates
        public event PropertyChangedEventHandler PropertyChanged;
        //Raise the event
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string fileName
        {
            get
            {
                return fName;
            }
            set
            {
                fName = value;
                OnPropertyChanged("Name");
            }
        }
        public string FileContent
        {
            get
            {
                return fcontent;
            }
            set
            {
                fcontent = value;
                OnPropertyChanged("Name");
            }
        }

    }
}
