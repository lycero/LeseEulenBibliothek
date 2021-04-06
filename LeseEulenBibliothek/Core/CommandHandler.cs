using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LeseEulenBibliothek.Core
{
    public class CommandHandler : ICommand
    {
        private readonly Action<object> m_Exec;
        private readonly Func<object, bool>? m_CanExecute;

        public event EventHandler? CanExecuteChanged;

        public void InvokeCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return m_CanExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            m_Exec?.Invoke(parameter);
        }

        public CommandHandler(Action<object> exec, Func<object, bool>? canExecute = null)
        {
            this.m_Exec = exec;
            this.m_CanExecute = canExecute;
        }
    }
}
