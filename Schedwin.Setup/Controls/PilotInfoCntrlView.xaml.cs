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
    /// Interaction logic for PilotInfoCntrlView.xaml
    /// </summary>
    public partial class PilotInfoCntrlView : ItemControlBase
    {
        public PilotInfoCntrlView()
        {
            InitializeComponent();
        }

        private void btnLoadImageClick(object sender, RoutedEventArgs e)
        {
            var viewModel=DataContext as  PilotInfoCntrlViewModel;
            viewModel.AddNewDocument();
        }
    }
}
