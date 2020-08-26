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
    /// Interaction logic for ReportParametersWindow.xaml
    /// </summary>
    public partial class ReportParametersWindow : RadWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public String _date1Label;
        public String Date1Label
        {
            get
            {
                return _date1Label;
            }
            set
            {
                _date1Label = value;
                NotifyPropertyChanged("Date1Label");
            }
        }

        public DateTime _date1;
        public DateTime Date1
        {
            get
            {
                return _date1;
            }
            set
            {
                _date1 = value;
                NotifyPropertyChanged("Date1");
            }
        }


        public String _date2Label;
        public String Date2Label
        {
            get
            {
                return _date2Label;
            }
            set
            {
                _date2Label = value;
                NotifyPropertyChanged("Date21abel");
            }
        }

        public DateTime _date2;
        public DateTime Date2
        {
            get
            {
                return _date2;
            }
            set
            {
                _date2 = value;
                NotifyPropertyChanged("Date2");
            }
        }

        public bool _showDate2;
        public bool ShowDate2
        {
            get
            {
                return _showDate2;
            }
            set
            {
                _showDate2 = value;
                NotifyPropertyChanged("ShowDate2");
            }
        }

       


        public bool _showList2;
        public bool ShowList2
        {
            get
            {
                return _showList2;
            }
            set
            {
                _showList2 = value;
                NotifyPropertyChanged("ShowList2");
            }
        }

        public String _list1Label;
        public String List1Label
        {
            get
            {
                return _list1Label;
            }
            set
            {
                _list1Label = value;
                NotifyPropertyChanged("List1Label");
            }
        }


        public String _list2Label;
        public String List2Label
        {
            get
            {
                return _list2Label;
            }
            set
            {
                _list2Label = value;
                NotifyPropertyChanged("List2Label");
            }
        }

        public ListboxItem SelectedItem1 { get; set; }

        public ListboxItem SelectedItem2 { get; set; }

        private String _windowTitle;
        public String WindowTitle
        {
            get
            {
                return _windowTitle;
            }
            set
            {
                _windowTitle = value;
                NotifyPropertyChanged("WindowTitle");
            }
        }

        public RangeObservableCollection<ListboxItem> List1 { get; set; }
        public RangeObservableCollection<ListboxItem> List2 { get; set; }
        public ReportParametersWindow()
        {
            InitializeComponent();
            List1 = new RangeObservableCollection<ListboxItem>();
            List2 = new RangeObservableCollection<ListboxItem>();
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

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NotifyPropertyChanged("List1");
            NotifyPropertyChanged("List2");
        }
    }
}
