
namespace Lab1;
public class Cargo : IFactory
{
    public UInt64 ID { get; private set; }
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

