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
    public enum NewItemType { Lodge,Airstrip,Flight,Aircraft, AircraftType,User,Pilot, Company}
    /// <summary>
    /// Interaction logic for AddINewItemView.xaml
    /// </summary>
    public partial class AddNewItemView : Window
    {
        public NewItemType ItemType { get; set; }

        public AddNewItemView(NewItemType type,String Server,String Database,int  ParentIDX)
        {
            ItemType = type;        
            InitializeComponent();
            Init(Server,Database, ParentIDX);
        }

        public void Init(String Server, String Database, int ParentIDX)
        {
           
            var viewModel = DataContext as AddNewItemViewModel;
            this.cntrlNewSetupCntentControl.Content=viewModel.Init(Server, Database, ItemType,this, ParentIDX);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AddNewItemViewModel;
          
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AddNewItemViewModel;
            if (viewModel.Validate())
            {
                var result = await viewModel.Save();
                if (result)
                {
                    DialogResult = true;
                    Close();
                }
            }

        }
    }
}
