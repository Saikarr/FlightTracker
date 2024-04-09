using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1;

public class Newspaper : Media
{
    public Newspaper(string name) : base(name) { }
    public override string VisitAirport(Airport element)
    {
        return $"{Name} - A report from the {element.Name} airport, {element.Country}.";
    }
    public override string VisitCargoPlane(CargoPlane element)
    {
        return $"{Name} - An interview with the crew of {element.Serial}.";
    }
    public override string VisitPassengerPlane(PassengerPlane element)
    {
        return $"{Name} - Breaking news! {element.Model} aircraft loses EASA fails certification after inspection of {element.Serial}";
    }
}