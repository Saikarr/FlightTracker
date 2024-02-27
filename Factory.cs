
using System.Text.Json.Serialization;

namespace Lab1;
public abstract class Factory
{
    public abstract IFactory Make(string[] args);
    public IFactory GetObject(string[] args) 
    {
        return Make(args);
    }
}

public class CrewFactory : Factory
{
    public override IFactory Make(string[] args)
    { 
        return new Crew(args);
    }

}
public class PassengerFactory : Factory
{
    public override IFactory Make(string[] args)
    {
        return new Passenger(args);
    }
}
public class CargoFactory : Factory
{
    public override IFactory Make(string[] args)
    {
        return new Cargo(args);
    }
}
public class CargoPlaneFactory : Factory
{
    public override IFactory Make(string[] args)
    {
        return new CargoPlane(args);
    }
}
public class PassengerPlaneFactory : Factory
{
    public override IFactory Make(string[] args)
    {
        return new PassengerPlane(args);
    }
}
public class AirportFactory : Factory
{
    public override IFactory Make(string[] args)
    {
        return new Airport(args);
    }
}
public class FlightFactory : Factory
{
    public override IFactory Make(string[] args)
    {
        return new Flight(args);
    }
}

class FactoryDictionary
{
    public static Dictionary<string, Factory> factories = new Dictionary<string, Factory>{
        {"C", new CrewFactory() },
        {"P", new PassengerFactory() },
        {"CA", new CargoFactory() },
        {"CP", new CargoPlaneFactory() },
        {"PP", new PassengerPlaneFactory() },
        {"AI", new AirportFactory() },
        {"FL", new FlightFactory() }
    };

    public static IFactory Create(string type, string[] data)
    {
        return factories[type].Make(data);
    }
}
