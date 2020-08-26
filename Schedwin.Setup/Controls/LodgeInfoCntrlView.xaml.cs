using Schedwin.Common;
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
    /// Interaction logic for LodgeInfoCntrlView.xaml
    /// </summary>
    public partial class LodgeInfoCntrlView : ItemControlBase
    {

        public LodgeInfoCntrlView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as LodgeInfoCntrlViewModel;
            viewModel.Init();
        }

        private void btnTPLookup_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as LodgeInfoCntrlViewModel;
            viewModel.TPLookUp();
        }
    }
}
