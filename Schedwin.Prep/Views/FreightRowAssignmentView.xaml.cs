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
using System.Windows.Shapes;

namespace Schedwin.Prep
{
    /// <summary>
    /// Interaction logic for FreightRowAssignmentView.xaml
    /// </summary>
    public partial class FreightRowAssignmentView : Window
    {
        public FreightRowAssignmentView()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new FreightRowAssignmentViewModel();
            viewModel.Clear();

        }
    }
}
