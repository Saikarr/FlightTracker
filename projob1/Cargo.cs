
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
}

