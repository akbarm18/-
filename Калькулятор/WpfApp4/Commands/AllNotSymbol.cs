using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp4.Commands
{
    public class AllNotSymbol : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action action1;
        public AllNotSymbol(Action action1)
        {
            this.action1 = action1;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action1.Invoke();
        }
    }
}
