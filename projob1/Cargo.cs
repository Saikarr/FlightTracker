
namespace Lab1;
public class Cargo : IFactory
{
    public UInt64 ID { get; private set; }
    public Single Weight { get; private set; }
    public string Code { get; private set; }
    public string Description { get; private set; }

    public Cargo(ulong iD, float weight, string code, string description)
    {
        ID = iD;
        Weight = weight;
        Code = code;
        Description = description;
    }
}

