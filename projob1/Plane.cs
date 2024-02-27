
namespace Lab1;
public class Plane : IFactory
{
    public UInt64 ID { get; private set; }
    public string Serial { get; private set; }
    public string Country { get; private set; }
    public string Model { get; private set; }
    public Plane(string[] args)
    {
        ID = UInt64.Parse(args[0]);
        Serial = args[1];
        Country = args[2];
        Model = args[3];
    }
}
public class CargoPlane : Plane
{
    public Single MaxLoad { get; private set; }
    public CargoPlane(string[] args) : base(args.Take(4).ToArray())
    {
        MaxLoad = Single.Parse(args[4]);
    }
}

public class PassengerPlane : Plane
{
    public UInt16 FirstClassSize { get; private set; }
    public UInt16 BusinessClassSize { get; private set; }
    public UInt16 EconomyClassSize { get; private set; }
    public PassengerPlane(string[] args) : base(args.Take(4).ToArray())
    {
        FirstClassSize = UInt16.Parse(args[4]);
        BusinessClassSize = UInt16.Parse(args[5]);
        EconomyClassSize = UInt16.Parse(args[6]);
    }
}

