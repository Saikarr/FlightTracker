using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1;

public class Radio : Media
{
    public Radio(string name) : base(name) { }
    public override string VisitAirport(Airport element)
    {
        return $"Reporting for {Name}, Ladies and gentelmen, we are at the {element.Name} airport.";
    }
    public override string VisitCargoPlane(CargoPlane element)
    {
        return $"Reporting for {Name}, Ladies and gentelmen, we are seeing the {element.Serial} aircraft fly above us.";
    }
    public override string VisitPassengerPlane(PassengerPlane element)
    {
        return $"Reporting for {Name}, Ladies and gentelmen, , we’ve just witnessed {element.Serial} take off.";
    }
}
