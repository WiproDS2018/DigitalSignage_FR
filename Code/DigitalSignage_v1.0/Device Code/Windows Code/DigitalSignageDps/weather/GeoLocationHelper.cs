using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalSignageDps
{
        class GeoLocationHelper
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            GeoCoordinate currentLocation = new GeoCoordinate(0, 0);

            public GeoLocationHelper()
            {
                watcher.PositionChanged += LocationChanged;
                watcher.Start();
            }
            public GeoCoordinate Location
            {
                get
                {
                while (currentLocation.Latitude == 0.0 && currentLocation.Longitude == 0.0)
                    Thread.Sleep(1000);
                    return currentLocation;
                }

            }
            private void LocationChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
            {
                currentLocation = args.Position.Location;
            Logger.LogToPlayer($"Latitude : {args.Position.Location.Latitude} longitude : {args.Position.Location.Longitude}");
            }
        }
}
