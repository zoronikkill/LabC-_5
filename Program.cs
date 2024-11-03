using System;

namespace CityTransportSchedule
{
    class Program
    {
        static void Main()
        {
            TransportSystem transportSystem = new TransportSystem();
            transportSystem.LoadData("../../../schedule.txt");

            while (true)
            {
                Console.Write("Enter station: ");
                string stationName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(stationName))
                    break;

                transportSystem.PrintNextDepartures(stationName);
            }
        }
    }
}