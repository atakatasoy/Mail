using System;
using System.Windows.Input;

namespace MailSender
{
    public class ParameterizedRelayCommand : ICommand
    {

        private Action<object> mAction;

        public event EventHandler CanExecuteChanged = (sender, e) => { };


        public ParameterizedRelayCommand(Action<object> action)
        {
            mAction = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction(parameter);
        }
    }
}
