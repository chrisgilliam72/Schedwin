using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
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

namespace Schedwin.Common
{
    /// <summary>
    /// Interaction logic for PrintWindowContainer.xaml
    /// </summary>
    public partial class PrintWindowContainer : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private String _windowtitle;
        public String WindowTitle
        {
            get
            {
                return _windowtitle;
            }
            set
            {
                _windowtitle = value;
                NotifyPropertyChanged("WindowTitle");
            }
        }

        private PrintedControl _printedControl;
        public PrintedControl PrintedControl
        {
            get
            {
                return _printedControl;
            }
            set
            {
                _printedControl = value;
                NotifyPropertyChanged("PrintedControl");
            }
        }

        private int _totalPages;
        public int TotalPages
        {
            get
            {
                return _totalPages;
            }
            set
            {
                _totalPages = value;
                NotifyPropertyChanged("TotalPages");
            }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                NotifyPropertyChanged("CurrentPage");
            }
        }

        public bool Landscape { get; set; }
        public PrintWindowContainer()
        {

            InitializeComponent();
            Landscape = false;
        }

        private void Button_ClickPrint(object sender, RoutedEventArgs e)
        {
            // Create the print dialog object and set options
            PrintDialog pDialog = new PrintDialog();
            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
            pDialog.UserPageRangeEnabled = true;

            // Display the dialog. This returns true if the user presses the Print button.
            Nullable<Boolean> print = pDialog.ShowDialog();
            if (print == true)
            {
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    if (Landscape)
                        pDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;

                    if (pDialog.CurrentPageEnabled)
                        pDialog.PrintVisual(PrintedControl, "Schedwin document");
                    else
                    {
                        PrintedControl.FirstPage();
                        for (int i = 1; i <= TotalPages; i++)
                        {
                            PrintedControl.UpdateLayout();
                            pDialog.PrintVisual(PrintedControl, "Schedwin document");
                            PrintedControl.NextPage();
                        }
                    }
                }
            }

            Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (PrintedControl != null)
            {
                TotalPages = PrintedControl.TotalPages();
                CurrentPage = PrintedControl.CurrentPage();
            }
        }

        private void Button_ClickNextPage(object sender, RoutedEventArgs e)
        {
            if (PrintedControl != null)
            {
                PrintedControl.NextPage();
                CurrentPage = PrintedControl.CurrentPage();
            }
               
        }

        private void Button_ClickPrevPage(object sender, RoutedEventArgs e)
        {
            if (PrintedControl != null)
            {
                PrintedControl.PrevPage();
                CurrentPage = PrintedControl.CurrentPage();
            }
               
        }

        private void Button_ClickFirstPage(object sender, RoutedEventArgs e)
        {
            if (PrintedControl != null)
            {
                PrintedControl.FirstPage();
                CurrentPage = PrintedControl.CurrentPage();
            }
        }

        private void Button_ClickLastPage(object sender, RoutedEventArgs e)
        {
            if (PrintedControl != null)
            {
                PrintedControl.LastPage();
                CurrentPage = PrintedControl.CurrentPage();
            }
        }
    }
}
