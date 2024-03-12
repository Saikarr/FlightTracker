
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
}