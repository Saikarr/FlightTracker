
using Avalonia.Data;
using Mapsui.Projections;
using NetworkSourceSimulator;

namespace Lab1;

public class Flight : IFactory
{
    public UInt64 ID { get;  set; }
    public UInt64 Origin_as_ID { get;  set; }
    public UInt64 Target_as_ID { get;  set; }
    public string TakeoffTime { get;  set; }
    public string LandingTime { get;  set; }
    public Single? Longitude { get;  set; }
    public Single? Latitude { get; set; }
    public TimeOnly? LastTime { get; set; } = null;
    public Single? AMSL { get;  set; }
    public UInt64 Plane_ID { get;  set; }
    public UInt64[] Crew_as_IDs { get;  set; }
    public UInt64[] Load_as_IDs { get;  set; }
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
        (double lon, double lat) lonlatorigin;
        if (LastTime == null)
        {
            lonlatorigin = SphericalMercator.FromLonLat
            (airports[Origin_as_ID].Longitude, airports[Origin_as_ID].Latitude);
        }
        else
        {
            lonlatorigin = SphericalMercator.FromLonLat
            ((double)Longitude, (double)Latitude);
        }
        (double lon, double lat) lonlattarget = SphericalMercator.FromLonLat
                (airports[Target_as_ID].Longitude, airports[Target_as_ID].Latitude);
        double rot = Math.Atan2(lonlattarget.lat - lonlatorigin.lat, lonlatorigin.lon - lonlattarget.lon);

        return rot - Math.PI / 2.0;
    }

    public (TimeOnly, TimeOnly) FromTakeLand()
    {
        TimeOnly takeoff = TimeOnly.Parse(TakeoffTime);
        TimeOnly landing = TimeOnly.Parse(LandingTime);

        return (takeoff, landing);
    }

    public WorldPosition CalcPos(Dictionary<ulong, Airport> airports, TimeOnly takeoff, TimeOnly landing, bool sameday)
    {
        TimeOnly cur = TimeOnly.FromDateTime(DateTime.Now);
        if (LastTime == null)
        {
            TimeSpan flightlength;
            if (sameday)
            {
                flightlength = landing - takeoff;
            }
            else
            {
                flightlength = landing - takeoff + new TimeSpan(24, 0, 0);
            }
            TimeSpan curflight = cur - takeoff;
            (float lat, float lon) distance = (airports[Target_as_ID].Latitude - airports[Origin_as_ID].Latitude,
                airports[Target_as_ID].Longitude - airports[Origin_as_ID].Longitude);
            LastTime = cur;
            Latitude = (float?)(airports[Origin_as_ID].Latitude + (curflight / flightlength) * distance.lat);
            Longitude = (float?)(airports[Origin_as_ID].Longitude + (curflight / flightlength) * distance.lon);

            return new WorldPosition((double)Latitude,(double)Longitude);
        }
        else
        {
            TimeSpan curflight = (TimeSpan)(cur - LastTime);
            TimeSpan flightlength = (TimeSpan)(landing - LastTime);
            (float lat, float lon) distance = (airports[Target_as_ID].Latitude - (float)Latitude,
                airports[Target_as_ID].Longitude - (float)Longitude);
            Latitude = (float?)(Latitude + (curflight / flightlength) * distance.lat);
            Longitude = (float?)(Longitude + (curflight / flightlength) * distance.lon);
            LastTime = cur;
            return new WorldPosition((double)Latitude, (double)Longitude);
        }
    }
    public void Update(Dictionary<ulong, Flight> flights, IDUpdateArgs e)
    {
        lock (flights)
        {
            ID = e.NewObjectID;
            var pom = this;
            flights.Remove(e.ObjectID);
            flights.Add(e.NewObjectID, pom);
        }
    }
}