using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CityTransportSchedule
{
    public class TransportSystem
    {
        public List<Route> Routes { get; }

        public TransportSystem()
        {
            Routes = new List<Route>();
        }

        public void AddRoute(Route route) => Routes.Add(route);

        public void PrintNextDepartures(string stationName)
        {
            var stationRoutes = new List<(int RouteNumber, int MinutesUntilDeparture, string Destination)>();

            DateTime now = DateTime.Now;
            int currentTime = now.Hour * 60 + now.Minute; // Переводим текущее время в минуты с начала дня

            foreach (var route in Routes)
            {
                var station = route.Stations.FirstOrDefault(s => s.Name == stationName);
                if (station != null)
                {
                    var departures = route.GetNextDepartures(station, currentTime);
                    foreach (var (departureTime, destination) in departures)
                    {
                        int minutesUntilDeparture = departureTime - currentTime;
                        stationRoutes.Add((route.Number, minutesUntilDeparture, destination));
                    }
                }
            }

            if (stationRoutes.Count == 0)
            {
                Console.WriteLine("No such station!");
            }
            else
            {
                Console.WriteLine($"Current time: {now:HH:mm}");
                Console.WriteLine("Schedule:");
                foreach (var (routeNumber, minutes, destination) in stationRoutes.OrderBy(x => x.MinutesUntilDeparture)
                             .Take(5)) // Show only 5 nearest departures
                {
                    Console.WriteLine($"{routeNumber}, destination {destination}, {minutes} min");
                }
            }
        }

        public void LoadData(string filePath)
        {
            var stationDict = new Dictionary<string, Station>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    int routeNumber = int.Parse(parts[0].Trim());
                    string[] stationNames = parts[1].Trim().Split('-');
                    int firstDeparture = int.Parse(parts[2].Trim());
                    int lastDeparture = int.Parse(parts[3].Trim());
                    int interval = int.Parse(parts[4].Trim());

                    var stations = new List<Station>();
                    var travelTimes = new Dictionary<Station, int>();

                    for (int i = 0; i < stationNames.Length; i++)
                    {
                        if (!stationDict.ContainsKey(stationNames[i]))
                        {
                            stationDict[stationNames[i]] = new Station(stationNames[i]);
                        }

                        stations.Add(stationDict[stationNames[i]]);
                        travelTimes[stationDict[stationNames[i]]] = int.Parse(parts[5 + i].Trim());
                    }

                    var route = new Route(routeNumber, stations, firstDeparture, lastDeparture, interval, travelTimes);
                    AddRoute(route);
                }
            }
        }
    }
}