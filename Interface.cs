
using System.Data;
using System.Text.Json.Serialization;

namespace Lab1;


[JsonDerivedType(typeof(Airport))]
[JsonDerivedType(typeof(Cargo))]
[JsonDerivedType(typeof(CargoPlane))]
[JsonDerivedType(typeof(Crew))]
[JsonDerivedType(typeof(Flight))]
[JsonDerivedType(typeof(Passenger))]
[JsonDerivedType(typeof(PassengerPlane))]
public interface IFactory
{

}

public class Person : IFactory
{
    public UInt64 ID { get; private set; }
    public string Name { get; private set; }
    public UInt64 Age { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }

    public Person(string[] args)
    {
        ID = UInt64.Parse(args[0]);
        Name = args[1];
        Age = UInt64.Parse(args[2]);
        Phone = args[3];
        Email = args[4];
    }
}

public class Crew : Person
{
    public UInt16 Practice { get; private set; }
    public string Role { get; private set; }
    public Crew(string[] args) : base(args.Take(5).ToArray())
    {
        Practice = UInt16.Parse(args[5]);
        Role = args[6];
    }
    
}

public class Passenger : Person
{
    public string Class {  get; private set; }
    public UInt64 Miles { get; private set; }
    public Passenger(string[] args) : base(args.Take(5).ToArray())
    {
        Class = args[5];
        Miles = UInt64.Parse(args[6]);
    }
}

public class Cargo : IFactory
{
    public UInt64 ID {  get; private set; }
    public Single Weight { get; private set; }
    public string Code { get; private set; }
    public string Description { get; private set; }
    public Cargo(string[] args)
    {
        ID = UInt64.Parse(args[0]);
        Weight = Single.Parse(args[1]);
        Code = args[2];
        Description = args[3];
    }
}

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
public class Flight : IFactory
{
    public UInt64 ID { get; private set; }
    public UInt64 Origin_as_ID { get; private set; }
    public UInt64 Target_as_ID { get; private set; }
    public string TakeoffTime { get; private set; }
    public string LandingTime { get; private set; }
    public Single Longitude {  set; private get; }
    public Single Latitude { set; private get; }
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
        string[] pom = args[9].Trim('[',']').Split(';');
        Crew_as_IDs = Array.ConvertAll(pom, UInt64.Parse);
        string[] pom2 = args[10].Trim('[',']').Split(';');
        Load_as_IDs = Array.ConvertAll(pom2, UInt64.Parse);
    }
}