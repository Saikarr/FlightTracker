
namespace Lab1;

public class Person : IFactory
{
    public UInt64 ID { get; private set; }
    public string Name { get; private set; }
    public UInt64 Age { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public Person(string[] args)
    {
        ID = UInt64.Parse(args[0]);
        Name = args[1];
        Age = UInt64.Parse(args[2]);
        Phone = args[3];
        Email = args[4];
    }
}

public class Crew : Person
{
    public UInt16 Practice { get; private set; }
    public string Role { get; private set; }
    public Crew(string[] args) : base(args.Take(5).ToArray())
    {
        Practice = UInt16.Parse(args[5]);
        Role = args[6];
    }
}

public class Passenger : Person
{
    public string Class { get; private set; }
    public UInt64 Miles { get; private set; }
    public Passenger(string[] args) : base(args.Take(5).ToArray())
    {
        Class = args[5];
        Miles = UInt64.Parse(args[6]);
    }
}