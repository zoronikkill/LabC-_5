using System.Collections.Generic;

namespace CityTransportSchedule
{
    public class Route
    {
        public int Number { get; }
        public List<Station> Stations { get; }
        public int FirstDeparture { get; }
        public int LastDeparture { get; }
        public int Interval { get; }
        public Dictionary<Station, int> TravelTimes { get; }

        public Route(int number, List<Station> stations, int firstDeparture, int lastDeparture, int interval,
            Dictionary<Station, int> travelTimes)
        {
            Number = number;
            Stations = stations;
            FirstDeparture = firstDeparture;
            LastDeparture = lastDeparture;
            Interval = interval;
            TravelTimes = travelTimes;
        }

        public List<(int DepartureTime, string Destination)> GetNextDepartures(Station station, int currentTime)
        {
            var departures = new List<(int DepartureTime, string Destination)>();

            if (!TravelTimes.ContainsKey(station))
                return departures;

            int stationArrivalTime = TravelTimes[station];
            int nextDepartureTime = FirstDeparture;

            while (nextDepartureTime + stationArrivalTime <= LastDeparture)
            {
                int departureTime = nextDepartureTime + stationArrivalTime;
                if (departureTime >= currentTime)
                {
                    // Определяем направление
                    string destination = (station == Stations.First()) ? Stations.Last().Name : Stations.First().Name;
                    departures.Add((departureTime, destination));
                }

                nextDepartureTime += Interval;
            }

            return departures;
        }
    }
}