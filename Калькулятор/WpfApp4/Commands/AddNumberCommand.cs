using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;

namespace WpfApp4.Commands
{
    public class AddNumberCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action<object> action;
        public AddNumberCommand(Action<object> action)
        {
            this.action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is TextBlock)
            {
                action.Invoke((parameter as TextBlock).Text);
            }
            else if(parameter is Image)
            {
                action.Invoke((parameter as Image).Source);
            }
           
        }
    }
}
