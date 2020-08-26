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
    /// Interaction logic for AircraftInfoCntrlView.xaml
    /// </summary>
    public partial class AircraftInfoCntrlView : ItemControlBase
    {

        public AircraftInfoCntrlView()
        {
            InitializeComponent();
            var viewModel = DataContext as AircraftInfoCntrlViewModel;
            var dataDocsViewModel = DataDocsView.DataContext as DataDocumentsCnrlViewModel;
            viewModel.DataDocsViewModel = dataDocsViewModel;
        }

      
    }
}
