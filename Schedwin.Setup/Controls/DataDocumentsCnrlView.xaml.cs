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

namespace Schedwin.Setup
{
    /// <summary>
    /// Interaction logic for DataDocumentsCnrlView.xaml
    /// </summary>
    public partial class DataDocumentsCnrlView : UserControl
    {
        public DataDocumentsCnrlView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as DataDocumentsCnrlViewModel;
            viewModel.Init();
        }

        private void DocumentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as DataDocumentsCnrlViewModel;
            viewModel.RefreshDataDocuments();
        }

        private void Document_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as DataDocumentsCnrlViewModel;
            viewModel.RefreshDocumentImage();

        }

        private void AddDocument_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as DataDocumentsCnrlViewModel;
            viewModel.AddNewDocument();
        }
    }
}
