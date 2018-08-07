using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SignageFaceRecognition
{
    class WeatherHelper
    {
        GeoLocationHelper locationHelper = new GeoLocationHelper();
        XmlSerializer serializer = new XmlSerializer(typeof(query));

        // Initialize Web Client and set its encoding to UTF8
        HttpClient httpClient = new HttpClient();
        public queryResultsChannel GetWeatherData()
        {
            GeoCoordinate location = locationHelper.Location;
            string XMLresult = httpClient.GetStringAsync($"https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(SELECT%20woeid%20FROM%20geo.places%20WHERE%20text%3D%22({location.Latitude}%2C{location.Longitude})%22)&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys").Result;
            //  string XMLresult = wc.DownloadString($"https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(SELECT%20woeid%20FROM%20geo.places%20WHERE%20text%3D%22({location.Latitude}%2C{location.Longitude})%22)&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");

            // Form Stream from the string we got
            byte[] byteArray = Encoding.UTF8.GetBytes(XMLresult);
            MemoryStream ms = new MemoryStream(byteArray);
            StreamReader reader = new StreamReader(ms);

            // Deserialize XML
            object obj = serializer.Deserialize(reader);
            query weatherData = (query)obj;
            reader.Close();
            return weatherData.results.channel;
        }
    }
}
