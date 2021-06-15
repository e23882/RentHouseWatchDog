using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Watchdog
{
    public class RelayCommand : ICommand
    {
        #region Declarations
        private Action<object> _executer;

        public event EventHandler CanExecuteChanged;
        #endregion

        #region Memberfunction
        public RelayCommand(Action<object> executer)
        {
            _executer = executer;
        }
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executer.Invoke(parameter);
        }
        #endregion
    }
}
