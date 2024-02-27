
namespace Lab1;

public class Flight : IFactory
{
    public UInt64 ID { get; private set; }
    public UInt64 Origin_as_ID { get; private set; }
    public UInt64 Target_as_ID { get; private set; }
    public string TakeoffTime { get; private set; }
    public string LandingTime { get; private set; }
    public Single Longitude { get; private set; }
    public Single Latitude { get; private set; }
    public Single AMSL { get; private set; }
    public UInt64 Plane_ID { get; private set; }
    public UInt64[] Crew_as_IDs { get; private set; }
    public UInt64[] Load_as_IDs { get; private set; }
    public Flight(string[] args)
    {
        ID = UInt64.Parse(args[0]);
        Origin_as_ID = UInt64.Parse(args[1]);
        Target_as_ID = UInt64.Parse(args[2]);
        TakeoffTime = args[3];
        LandingTime = args[4];
        Longitude = Single.Parse(args[5]);
        Latitude = Single.Parse(args[6]);
        AMSL = Single.Parse(args[7]);
        Plane_ID = UInt64.Parse(args[8]);
        string[] pom = args[9].Trim('[', ']').Split(';');
        Crew_as_IDs = Array.ConvertAll(pom, UInt64.Parse);
        string[] pom2 = args[10].Trim('[', ']').Split(';');
        Load_as_IDs = Array.ConvertAll(pom2, UInt64.Parse);
    }
}