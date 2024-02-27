
namespace Lab1;
public class Airport : IFactory
{
    public UInt64 ID { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }
    public Single Longitude { get; private set; }
    public Single Latitude { get; private set; }
    public Single AMSL { get; private set; }
    public string Country { get; private set; }
    public Airport(string[] args)
    {
        ID = UInt64.Parse(args[0]);
        Name = args[1];
        Code = args[2];
        Longitude = Single.Parse(args[3]);
        Latitude = Single.Parse(args[4]);
        AMSL = Single.Parse(args[5]);
        Country = args[6];
    }
}

