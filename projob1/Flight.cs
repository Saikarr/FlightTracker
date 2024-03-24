
using Avalonia.Data;
using Mapsui.Projections;

namespace Lab1;

public class Flight : IFactory
{
    public UInt64 ID { get; private set; }
    public UInt64 Origin_as_ID { get; private set; }
    public UInt64 Target_as_ID { get; private set; }
    public string TakeoffTime { get; private set; }
    public string LandingTime { get; private set; }
    public Single? Longitude { get; private set; }
    public Single? Latitude { get; private set; }
    public Single? AMSL { get; private set; }
    public UInt64 Plane_ID { get; private set; }
    public UInt64[] Crew_as_IDs { get; private set; }
    public UInt64[] Load_as_IDs { get; private set; }
    public Flight(UInt64 iD, UInt64 origin_as_ID, UInt64 target_as_ID, string takeoffTime, string landingTime, Single? longitude,
        Single? latitude, Single? aMSL, UInt64 plane_ID, UInt64[] crew_as_IDs, UInt64[] load_as_IDs)
    {
        ID = iD;
        Origin_as_ID = origin_as_ID;
        Target_as_ID = target_as_ID;
        TakeoffTime = takeoffTime;
        LandingTime = landingTime;
        Longitude = longitude;
        Latitude = latitude;
        AMSL = aMSL;
        Plane_ID = plane_ID;
        Crew_as_IDs = crew_as_IDs;
        Load_as_IDs = load_as_IDs;
    }

    public double CalcRot(Flight flight, Dictionary<int,Airport> airports)
    {
        (double, double) tuple = SphericalMercator.FromLonLat(airports[(int)flight.Origin_as_ID].Longitude,
            airports[(int)flight.Origin_as_ID].Latitude);
        (double, double) tuple2 = SphericalMercator.FromLonLat(airports[(int)flight.Target_as_ID].Longitude,
            airports[(int)flight.Target_as_ID].Latitude);
        double num = Math.Atan2(tuple2.Item2 - tuple.Item2, tuple.Item1 - tuple2.Item1);
        if (!(num < 0.0))
        {
            return Math.PI + num + Math.PI / 2.0;
        }

        return num - Math.PI / 2.0;
    }

    public (TimeOnly, TimeOnly) FromTakeLand(Flight flight)
    {
        string[] pom1 = flight.TakeoffTime.Split()[1].Split(':');
        TimeOnly takeoff = new TimeOnly(int.Parse(pom1[0]), int.Parse(pom1[1]), int.Parse(pom1[2]));
        string[] pom2 = flight.LandingTime.Split()[1].Split(':');
        TimeOnly landing = new TimeOnly(int.Parse(pom2[0]), int.Parse(pom2[1]), int.Parse(pom2[2]));
        return (takeoff, landing);
    }

    public WorldPosition CalcPos(Flight flight, Dictionary<int, Airport> airports, TimeOnly takeoff, TimeOnly landing, bool sameday)
    {
        TimeOnly cur = TimeOnly.FromDateTime(DateTime.Now);
        TimeSpan flightlength;
        if (sameday)
        {
            flightlength = landing - takeoff;
        }
        else
        {
            flightlength = landing - takeoff + new TimeSpan(24,0,0);
        }
        TimeSpan curflight = cur - takeoff;
        (float, float) distance = (airports[(int)flight.Target_as_ID].Latitude - airports[(int)flight.Origin_as_ID].Latitude,
            airports[(int)flight.Target_as_ID].Longitude - airports[(int)flight.Origin_as_ID].Longitude);
        return new WorldPosition(airports[(int)flight.Origin_as_ID].Latitude + (curflight / flightlength) * distance.Item1,
            airports[(int)flight.Origin_as_ID].Longitude + (curflight / flightlength) * distance.Item2);
    }
}