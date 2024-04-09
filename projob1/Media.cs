namespace Lab1;

public abstract class Media : IVisitor
{
    protected string Name;
    public Media(string name)
    {
        Name = name;
    }

    public abstract string VisitAirport(Airport element);
    public abstract string VisitCargoPlane(CargoPlane element);
    public abstract string VisitPassengerPlane(PassengerPlane element);

}