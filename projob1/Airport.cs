
namespace Lab1;
public class Airport : IFactory, IReportable
{
    public UInt64 ID { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }
    public Single Longitude { get; private set; }
    public Single Latitude { get; private set; }
    public Single AMSL { get; private set; }
    public string Country { get; private set; }

    public Airport(ulong iD, string name, string code, float longitude, float latitude, float aMSL, string country)
    {
        ID = iD;
        Name = name;
        Code = code;
        Longitude = longitude;
        Latitude = latitude;
        AMSL = aMSL;
        Country = country;
    }
    public string Accept(IVisitor visitor)
    {
        return visitor.VisitAirport(this);
    }
}

