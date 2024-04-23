
using NetworkSourceSimulator;

namespace Lab1;
public class Cargo : IFactory
{
    public UInt64 ID { get;  set; }
    public Single Weight { get;  set; }
    public string Code { get;  set; }
    public string Description { get;  set; }

    public Cargo(ulong iD, float weight, string code, string description)
    {
        ID = iD;
        Weight = weight;
        Code = code;
        Description = description;
    }

    public void Update(Dictionary<ulong, Flight> flights, IDUpdateArgs e, Dictionary<ulong, Cargo> cargos)
    {
        ID = e.NewObjectID;
        var pom = this;
        cargos.Remove(e.ObjectID);
        cargos.Add(e.NewObjectID, pom);
        lock (flights)
        {
            foreach (var item in flights.Values)
            {
                for (int i = 0; i < item.Load_as_IDs.Length; i++)
                {
                    if (item.Load_as_IDs[i] == e.ObjectID) item.Load_as_IDs[i] = e.NewObjectID;
                }
            }
        }
    }
}

