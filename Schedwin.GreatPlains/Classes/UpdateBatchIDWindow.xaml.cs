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

namespace Schedwin.GreatPlains
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class BatchIDWindow : RadWindow, INotifyPropertyChanged
    {
        public bool UseSameIDForAllDebtors { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public String DebtorCode { get; set; }


        public BatchIDWindow( )
        {
            InitializeComponent();
            UseSameIDForAllDebtors = true;

            NotifyPropertyChanged("UseSameIDForAllDebtors");

        }

        private void Button_OKClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Button_CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
