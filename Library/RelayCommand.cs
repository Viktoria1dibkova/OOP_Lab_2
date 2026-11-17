using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Library
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute_;
        private readonly Predicate<object> canExecute_;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            execute_ = execute ?? throw new ArgumentNullException(nameof(execute));
            canExecute_ = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute_ == null || canExecute_(parameter);
        public void Execute(object parameter) => execute_(parameter);
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
