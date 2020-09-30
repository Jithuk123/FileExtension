using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMFileMange.Command
{

    public class RelayCommand : ICommand
    {
        //these delegetes will points to function in viewmodel
        public Action<object> ExecuteMethod { get; set; }
        public Func<object, bool> CanExecuteMethod { get; set; }
        
        public RelayCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.ExecuteMethod = executeMethod;
            this.CanExecuteMethod = canExecuteMethod;
        }
        
        public bool CanExecute(object parameter)
        {
            return CanExecuteMethod(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            ExecuteMethod(parameter);
        }
    }
}
