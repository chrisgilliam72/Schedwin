using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Schedwin.Setup
{
    public class MapCntrlWindowViewModel : ViewModelBase
    {
        public double PosDecLong { get; set; }
        public double PosDecLat { get; set; }

        public MapCntrlWindowView View { get; set; }
        public String PlaceSearch { get; set; }
        public String AirstripName { get; set; }

        public String Longitude { get; set; }

        public String Latitude { get; set; }

        public Tuple<int, int, int> DecimalToDegrees(decimal decimalValue)
        {
            return Tuple.Create(Convert.ToInt32(decimal.Truncate(decimalValue)), Convert.ToInt32((decimal.Truncate(Math.Abs(decimalValue) * 60)) % 60), Convert.ToInt32((Math.Abs(decimalValue) * 3600) % 60));
        }

        public MapCntrlWindowViewModel()
        {
            PosDecLong = 23.6480555555556;
            PosDecLat = -19.0927777777778;
        }

        public void Init(String stripName, double Longitude, double Latitude, bool IsNew = false)
        {
            AirstripName = stripName;
            if (!IsNew)
                UpdateLocation(Longitude, Latitude);
            else
                UpdateLocation(PosDecLong, PosDecLat);


        }

        public void FindPlaces()
        {
            var xmlResults=Geocode(PlaceSearch);
            if (View != null)
                View.DisplayResults(xmlResults);
        }

        public void Refresh()
        {
            NotifyPropertyChanged("AirstripName");
            NotifyPropertyChanged("CenterLoc");
            NotifyPropertyChanged("PushPinLoc");
        }

        public XmlDocument Geocode(string addressQuery)
        {
            //Create REST Services geocode request using Locations API
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&key=" + "C0vx646enR6ewdPZynFd~3EGZlsBuA3NGxFsp_HeJKw~AggciBx1Ya7MXOzjRb3kYPL7A8NNfLv2lTmOkK8p7sISNmes8MyuOVvKVqgEy2KI";


            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);

            return (geocodeResponse);
        }

        private XmlDocument GetXmlResponse(string requestUrl)
        {
            System.Diagnostics.Trace.WriteLine("Request URL (XML): " + requestUrl);
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return xmlDoc;
            }
        }

        public void UpdateLocation(double Long, double Lat)
        {
            var degLong = DecimalToDegrees(Convert.ToDecimal(Long));
            var degLat = DecimalToDegrees(Convert.ToDecimal(Lat));

            PosDecLat = Lat;
            PosDecLong = Long;

            Latitude = Math.Abs(degLat.Item1) +"°"  + degLat.Item2 + "'" + degLat.Item3+"\"";
            Latitude += Lat < 0 ? " S" : " N";
            Longitude = Math.Abs(degLong.Item1) + "°" + degLong.Item2 + "'" + degLong.Item3 + "\"";
            Longitude += Long < 0 ? " W" : " E";

          

            NotifyPropertyChanged("Longitude");
            NotifyPropertyChanged("Latitude");
         }
    }
}
