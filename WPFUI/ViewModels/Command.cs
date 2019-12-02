using System;
using System.Windows.Input;

namespace WPFUI.ViewModels
{
    /// <summary>
    /// Class for implementing ICommand interface, from Kevin Boost at IntelliTect
    /// </summary>
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action Method { get; }

        public Command(Action commandAction)
        {
            Method = commandAction ?? throw new ArgumentNullException(nameof(commandAction));
        }

        public bool CanExecute(object paremeter)
        {
            return true;
        }

        public void Execute(object paremeter) => Method?.Invoke();
    }
}
