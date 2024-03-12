
namespace Lab1;
public class Plane : IFactory
{
    public UInt64 ID { get; private set; }
    public string Serial { get; private set; }
    public string Country { get; private set; }
    public string Model { get; private set; }
    public Plane(UInt64 iD, string serial, string country, string model)
    {
        ID = iD;
        Serial = serial;
        Country = country;
        Model = model;
    }
}
public class CargoPlane : Plane
{
    public Single MaxLoad { get; private set; }
    public CargoPlane(UInt64 iD, string serial, string country, string model, Single maxLoad)
        : base(iD, serial, country, model)
    {
        MaxLoad = maxLoad;
    }
}

public class PassengerPlane : Plane
{
    public UInt16 FirstClassSize { get; private set; }
    public UInt16 BusinessClassSize { get; private set; }
    public UInt16 EconomyClassSize { get; private set; }

    public PassengerPlane(UInt64 iD, string serial, string country, string model, UInt16 firstClassSize,
        UInt16 businessClassSize, UInt16 economyClassSize) : base(iD, serial, country, model)
    {
        FirstClassSize = firstClassSize;
        BusinessClassSize = businessClassSize;
        EconomyClassSize = economyClassSize;
    }
}

