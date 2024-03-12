
using System.Numerics;
using System.Xml.Linq;

namespace Lab1;

public class Person : IFactory
{
    public UInt64 ID { get; private set; }
    public string Name { get; private set; }
    public UInt64 Age { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }

    public Person(UInt64 iD, string name, UInt64 age, string phone, string email)
    {
        ID = iD;
        Name = name;
        Age = age;
        Phone = phone;
        Email = email;
    }
}

public class Crew : Person
{
    public UInt16 Practice { get; private set; }
    public string Role { get; private set; }
    public Crew(UInt64 iD, string name, UInt64 age, string phone, string email, UInt16 practice, string role) : base(iD, name, age, phone, email)
    {
        Practice = practice;
        Role = role;
    }
}

public class Passenger : Person
{
    public string Class { get; private set; }
    public UInt64 Miles { get; private set; }

    public Passenger(UInt64 iD, string name, UInt64 age, string phone, string email, string @class, UInt64 miles) : base(iD, name, age, phone, email)
    {
        Class = @class;
        Miles = miles;
    }
}