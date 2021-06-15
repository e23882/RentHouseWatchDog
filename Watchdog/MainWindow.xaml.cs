using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Watchdog
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Declarations
        
        private readonly MainWindowViewModel _main;
        #endregion

        #region Property
        #endregion

        #region Memberfunction
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            _main = new MainWindowViewModel();
            this.DataContext = _main;
        }
    }
}
