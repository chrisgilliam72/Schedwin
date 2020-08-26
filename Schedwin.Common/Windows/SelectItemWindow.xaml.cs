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
    /// Interaction logic for SelectItemWindow.xaml
    /// </summary>
    public partial class SelectItemWindow :  RadWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public RangeObservableCollection<ListboxItem> Items { get; set; }

        public ListboxItem SelectedItem { get; set; }


        public String WindowTitle { get; set; }


        public SelectItemWindow()
        {
            Items = new RangeObservableCollection<ListboxItem>();
            InitializeComponent();
          
        }

        public void Init(String titleMessage, List<ListboxItem> items)
        {
            Items.AddRange(items.OrderBy(x=>x.Description).ToList());
            WindowTitle = titleMessage;
            NotifyPropertyChanged("WindowTitle");
            NotifyPropertyChanged("Items");
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
