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

namespace Schedwin.Common.Windows
{
    /// <summary>
    /// Interaction logic for SelectDatesWindow.xaml
    /// </summary>
    public partial class SelectDatesWindow : RadWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public SelectDatesWindow()
        {
            InitializeComponent();

        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartDate = DateTime.Today.AddDays(-14);
            EndDate = DateTime.Today;

            NotifyPropertyChanged("StartDate");
            NotifyPropertyChanged("EndDate");

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
