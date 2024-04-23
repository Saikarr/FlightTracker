
using System.Numerics;
using System.Xml.Linq;
using NetworkSourceSimulator;

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
    public void Update(Dictionary<ulong, Flight> flights, IDUpdateArgs e, Dictionary<ulong, Crew> crews)
    {
        ID = e.NewObjectID;
        var pom = this;
        crews.Remove(e.ObjectID);
        crews.Add(e.NewObjectID, pom);
        lock (flights)
        {
            foreach (var item in flights.Values)
            {
                for (int i = 0; i < item.Crew_as_IDs.Length; i++)
                {
                    if (item.Crew_as_IDs[i] == e.ObjectID) item.Crew_as_IDs[i] = e.NewObjectID;
                }
            }
        }
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
    public void Update(IDUpdateArgs e, Dictionary<ulong, Passenger> passenger)
    {
        ID = e.NewObjectID;
        var pom = passenger[e.ObjectID];
        passenger.Remove(e.ObjectID);
        passenger.Add(e.NewObjectID, pom);
    }
}