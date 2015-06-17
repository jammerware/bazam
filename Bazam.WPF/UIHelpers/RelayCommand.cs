using System;
using System.Windows.Input;

namespace Bazam.Wpf.UIHelpers
{
    public class RelayCommand : ICommand
    {
        #region Fields
        readonly Action _execute;
        readonly Predicate<object> _canExecute;
        #endregion

        #region Constructors
        protected RelayCommand() { }
        public RelayCommand(Action execute) : this(execute, null) { }
        public RelayCommand(Action execute, Predicate<object> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region ICommand Members [DebuggerStepThrough]
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public virtual void Execute(object parameter) { _execute(); }
        #endregion
    }

    public class RelayCommand<T> : RelayCommand
    {
        private Predicate<T> _CanExecute;
        private Action<T> _Execute;

        public RelayCommand(Action<T> execute) : this(execute, null) { }
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _CanExecute = canExecute;
            _Execute = execute;
        }

        #region ICommand members [DebuggerStepThrough]
        public override bool CanExecute(object parameter)
        {
            return _CanExecute == null ? true : _CanExecute((T)parameter);
        }

        public override void Execute(object parameter)
        {
            _Execute((T)parameter);
        }
        #endregion
    }
}