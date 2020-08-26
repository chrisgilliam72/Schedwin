using Microsoft.Maps.MapControl.WPF;
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
using System.Xml;

namespace Schedwin.Setup
{
    /// <summary>
    /// Interaction logic for MapCntrlWindowView.xaml
    /// </summary>
    public partial class MapCntrlWindowView : Window
    {
        public MapCntrlWindowView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MapCntrlWindowViewModel;
            viewModel.View = this;
            viewModel.Refresh();        
            SchedwinMap.SetView(new Location { Latitude = viewModel.PosDecLat, Longitude = viewModel.PosDecLong },8);
            SchedwinMap.Children.Clear();
            AddPushpinToMap(viewModel.PosDecLat, viewModel.PosDecLong, viewModel.AirstripName);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MapCntrlWindowViewModel;
            viewModel.FindPlaces();
        }

        public void DisplayResults(XmlDocument nearbyPOI)
        {
            SchedwinMap.Children.Clear();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nearbyPOI.NameTable);
            nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            //Get the the entityID for each POI entity in the response
            XmlNodeList displayNameList = nearbyPOI.SelectNodes("//rest:Location", nsmgr);

            //Provide entity information and put a pushpin on the map.
     
            if (displayNameList.Count > 0)
            {
 
                XmlNodeList latitudeList = nearbyPOI.SelectNodes(".//rest:Latitude", nsmgr);
                XmlNodeList longitudeList = nearbyPOI.SelectNodes(".//rest:Longitude", nsmgr);
                XmlNodeList NameList= nearbyPOI.SelectNodes(".//rest:Name", nsmgr);
                for (int i = 0; i < displayNameList.Count; i++)
                {
                    AddPushpinToMap(Convert.ToDouble(latitudeList[i].InnerText), Convert.ToDouble(longitudeList[i].InnerText), NameList[i].InnerText);
                }
                SchedwinMap.Visibility = Visibility.Visible;

                SchedwinMap.Focus(); //allows '+' and '-' to zoom the map
            }

        }

        private void AddPushpinToMap(double latitude, double longitude, string pinLabel)
        {
            Location location = new Location(latitude, longitude);
            Pushpin pushpin = new Pushpin();
            pushpin.ToolTip = pinLabel;
            pushpin.Location = location;
            SchedwinMap.Children.Add(pushpin);
        }

        private void SchedwinMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as MapCntrlWindowViewModel;
            SchedwinMap.Children.Clear();
            var pushpin = new Pushpin();
            var mouspos = Mouse.GetPosition(this);
            pushpin.Location = SchedwinMap.ViewportPointToLocation(mouspos);
            SchedwinMap.Children.Add(pushpin);

            viewModel.UpdateLocation(pushpin.Location.Longitude, pushpin.Location.Latitude);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnShowNearby_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClearNearby_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
