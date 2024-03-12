using System.Text.Json.Serialization;

namespace Lab1;

[JsonDerivedType(typeof(Crew))]
[JsonDerivedType(typeof(Passenger))]
[JsonDerivedType(typeof(Cargo))]
[JsonDerivedType(typeof(CargoPlane))]
[JsonDerivedType(typeof(PassengerPlane))]
[JsonDerivedType(typeof(Airport))]
[JsonDerivedType(typeof(Flight))]
public interface IFactory
{

}