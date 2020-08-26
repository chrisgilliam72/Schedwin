using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Common
{
    public struct LongLatPosition
    {
        public String Longitude { get; set; }
        public String Latitude { get; set; }
    }

    public class CoordinateCoverter
    {
        public static Tuple<int, int, int> DecimalToDegrees(decimal decimalValue)
        {
            return Tuple.Create(Convert.ToInt32(decimal.Truncate(decimalValue)), Convert.ToInt32((decimal.Truncate(Math.Abs(decimalValue) * 60)) % 60), Convert.ToInt32((Math.Abs(decimalValue) * 3600) % 60));
        }
        
        public static LongLatPosition DecimalToLongLatString(double Long, double Lat)
        {
            var returnPos = new LongLatPosition();
            var degLong = DecimalToDegrees(Convert.ToDecimal(Long));
            var degLat = DecimalToDegrees(Convert.ToDecimal(Lat));



            returnPos.Latitude = Math.Abs(degLat.Item1) + "°" + degLat.Item2 + "'" + degLat.Item3 + "\"";
            returnPos.Latitude += Lat < 0 ? "S" : "N";
            returnPos.Longitude = Math.Abs(degLong.Item1) + "°" + degLong.Item2 + "'" + degLong.Item3 + "\"";
            returnPos.Longitude += Long < 0 ? "W" : "E";

            return returnPos;

        }


        public static double ConvertDegreesToDecimal(string coordinate)
        {
            double decimalCoordinate = 0.0;
            double degrees = 0;
            double minutes = 0;
            double seconds = 0;
            var coordsString =coordinate.Split('.');

            if (coordsString[0].Length==3)
             degrees = Double.Parse(coordsString[0].Substring(1, 2));
            if (coordsString[0].Length == 4)
                degrees = Double.Parse(coordsString[0].Substring(1, 3));
            minutes = Double.Parse(coordsString[1].Substring(0, 2)) / 60;
            seconds = Double.Parse(coordsString[2].Substring(0, 2)) / 3600;
            decimalCoordinate = (degrees + minutes + seconds);

            if (Char.ToUpper(coordinate.First()) == 'S' || Char.ToUpper(coordinate.First()) == 'W')
                decimalCoordinate *= -1;
            return decimalCoordinate;
        }

        //public static double ConvertDegreesToDecimal(string coordinate)
        //{
        //    double decimalCoordinate=0.0;
          
        //    double degrees = Double.Parse(coordinate.Substring(0,2));
        //    double minutes = Double.Parse(coordinate.Substring(3, 2)) / 60;
        //    double seconds = Double.Parse(coordinate.Substring(6, 2)) / 3600;  
        //    decimalCoordinate = (degrees + minutes + seconds);

        //    if (Char.ToUpper(coordinate.Last()) == 'S' || Char.ToUpper(coordinate.Last()) == 'W')
        //        decimalCoordinate *= -1;
        //    return decimalCoordinate;
        //}
    }
}
