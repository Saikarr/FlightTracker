
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

    public double CalcRot(Dictionary<ulong, Airport> airports)
    {
        (double lon, double lat) lonlatorigin = SphericalMercator.FromLonLat
            (airports[Origin_as_ID].Longitude, airports[Origin_as_ID].Latitude);
        (double lon, double lat) lonlattarget = SphericalMercator.FromLonLat
            (airports[Target_as_ID].Longitude,airports[Target_as_ID].Latitude);
        double rot = Math.Atan2(lonlattarget.lat - lonlatorigin.lat, lonlatorigin.lon - lonlattarget.lon);

        return rot - Math.PI / 2.0;
    }

    public (TimeOnly, TimeOnly) FromTakeLand()
    {
        TimeOnly takeoff = TimeOnly.Parse(TakeoffTime.Split()[1]);
        TimeOnly landing = TimeOnly.Parse(LandingTime.Split()[1]);

        return (takeoff, landing);
    }

    public WorldPosition CalcPos(Dictionary<ulong, Airport> airports, TimeOnly takeoff, TimeOnly landing, bool sameday)
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
        (float lat, float lon) distance = (airports[Target_as_ID].Latitude - airports[Origin_as_ID].Latitude, 
            airports[Target_as_ID].Longitude - airports[Origin_as_ID].Longitude);

        return new WorldPosition(airports[Origin_as_ID].Latitude + (curflight / flightlength) * distance.lat, 
            airports[Origin_as_ID].Longitude + (curflight / flightlength) * distance.lon);
    }
}