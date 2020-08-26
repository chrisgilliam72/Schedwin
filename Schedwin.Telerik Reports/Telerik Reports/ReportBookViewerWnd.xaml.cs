using System;
using System.Collections.Generic;
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

namespace Schedwin.Reports
{
    /// <summary>
    /// Interaction logic for ReportBookViewerWnd.xaml
    /// </summary>
    public partial class ReportBookViewerWnd : Window
    {
        public ReportBookViewerWnd()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReportViewer.RefreshReport();
        }
    }
}
