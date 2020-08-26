using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Telerik.Windows.Controls;

namespace Schedwin.Common
{
    /// <summary>
    /// Interaction logic for WarnWindow.xaml
    /// </summary>
    public partial class WarnWindow : RadWindow, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private String _message;
        public String Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                NotifyPropertyChanged("Message");
            }
        }

        public static void Display(String message)
        {

            var wnd = new WarnWindow();
            //wnd.Owner = Application.Current.MainWindow;
            wnd.Message = message;
            wnd.ShowDialog();
        }
        public WarnWindow()
        {
            InitializeComponent();
        }
    }
}
