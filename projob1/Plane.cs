
using NetworkSourceSimulator;

namespace Lab1;
public abstract class Plane : IFactory, IReportable
{
    public UInt64 ID { get;  set; }
    public string Serial { get;  set; }
    public string Country { get;  set; }
    public string Model { get;  set; }
    public Plane(UInt64 iD, string serial, string country, string model)
    {
        ID = iD;
        Serial = serial;
        Country = country;
        Model = model;
    }
    public abstract string Accept(IVisitor visitor);
}
public class CargoPlane : Plane
{
    public Single MaxLoad { get;  set; }
    public CargoPlane(UInt64 iD, string serial, string country, string model, Single maxLoad)
        : base(iD, serial, country, model)
    {
        MaxLoad = maxLoad;
    }

    public override string Accept(IVisitor visitor)
    {
        return visitor.VisitCargoPlane(this);
    }
    public void Update(Dictionary<ulong, Flight> flights, IDUpdateArgs e, Dictionary<ulong, CargoPlane> cargoPlanes)
    {
        ID = e.NewObjectID;
        var pom = this;
        cargoPlanes.Remove(e.ObjectID);
        cargoPlanes.Add(e.NewObjectID, pom);
        lock (flights)
        {
            foreach (var item in flights.Values)
            {
                if (item.Plane_ID == e.ObjectID) item.Plane_ID = e.NewObjectID;
            }
        }
    }
}

public class PassengerPlane : Plane
{
    public UInt16 FirstClassSize { get;  set; }
    public UInt16 BusinessClassSize { get;  set; }
    public UInt16 EconomyClassSize { get;  set; }

    public PassengerPlane(UInt64 iD, string serial, string country, string model, UInt16 firstClassSize,
        UInt16 businessClassSize, UInt16 economyClassSize) : base(iD, serial, country, model)
    {
        FirstClassSize = firstClassSize;
        BusinessClassSize = businessClassSize;
        EconomyClassSize = economyClassSize;
    }

    public override string Accept(IVisitor visitor)
    {
        return visitor.VisitPassengerPlane(this);
    }
    public void Update(Dictionary<ulong, Flight> flights, IDUpdateArgs e, Dictionary<ulong, PassengerPlane> passengerPlanes)
    {
        ID = e.NewObjectID;
        var pom = this;
        passengerPlanes.Remove(e.ObjectID);
        passengerPlanes.Add(e.NewObjectID, pom);
        lock (flights)
        {
            foreach (var item in flights.Values)
            {
                if (item.Plane_ID == e.ObjectID) item.Plane_ID = e.NewObjectID;
            }
        }
    }
}

