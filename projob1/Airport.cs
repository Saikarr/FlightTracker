
namespace Lab1;
public class Airport : IFactory, IReportable
{
    public UInt64 ID { get;  set; }
    public string Name { get;  set; }
    public string Code { get;  set; }
    public Single Longitude { get;  set; }
    public Single Latitude { get;  set; }
    public Single AMSL { get;  set; }
    public string Country { get;  set; }

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

