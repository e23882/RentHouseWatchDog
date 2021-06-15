using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Watchdog
{
    public class MainWindowViewModel:ViewModelBase
    {
        #region Declarations
        #endregion

        #region Property
        public RelayCommand ButtonClickCommand 
        {
            get 
            {
                return new RelayCommand(ButtonClickCommandAction); 
            }
        }

        private void ButtonClickCommandAction(object obj)
        {
            ((App)Application.Current).ShowMessage("Test", "hello", NotificationType.Error);
            ((App)Application.Current).ShowMessage("Test", "hello", NotificationType.Information);
            ((App)Application.Current).ShowMessage("Test", "hello", NotificationType.Success);
            ((App)Application.Current).ShowMessage("Test", "hello", NotificationType.Warning);
        }


        
        #endregion

        #region Memberfunction
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel() 
        {
        }
        #endregion
    }
}
