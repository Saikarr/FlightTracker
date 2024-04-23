
using System.Numerics;
using System.Xml.Linq;

namespace Lab1;

public class Person : IFactory
{
    public UInt64 ID { get;  set; }
    public string Name { get;  set; }
    public UInt64 Age { get;  set; }
    public string Phone { get;  set; }
    public string Email { get;  set; }

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
    public UInt16 Practice { get;  set; }
    public string Role { get;  set; }
    public Crew(UInt64 iD, string name, UInt64 age, string phone, string email, UInt16 practice, string role) : base(iD, name, age, phone, email)
    {
        Practice = practice;
        Role = role;
    }
}

public class Passenger : Person
{
    public string Class { get;  set; }
    public UInt64 Miles { get;  set; }

    public Passenger(UInt64 iD, string name, UInt64 age, string phone, string email, string @class, UInt64 miles) : base(iD, name, age, phone, email)
    {
        Class = @class;
        Miles = miles;
    }
}