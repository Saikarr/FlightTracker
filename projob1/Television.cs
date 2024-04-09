using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1;

public class Television : Media
{
    public Television(string name) : base(name) { }
    public override string VisitAirport(Airport element)
    {
        return $"<An image of {element.Name} airport>";
    }
    public override string VisitCargoPlane(CargoPlane element)
    {
        return $"<An image of {element.Serial} cargo plane>";
    }
    public override string VisitPassengerPlane(PassengerPlane element)
    {
        return $"<An image of {element.Serial} passenger plane>";
    }
}

