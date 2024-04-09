namespace Lab1;

public interface IVisitor
{
    string VisitAirport(Airport element);
    string VisitCargoPlane(CargoPlane element);
    string VisitPassengerPlane(PassengerPlane element);
}