
using System.Data;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Lab1;
public abstract class Factory
{
    protected abstract IFactory MakeFromFTR(string[] args);
    public IFactory GetObjectFromFTR(string[] args)
    {
        return MakeFromFTR(args);
    }
    protected abstract IFactory MakeFromBin(byte[] args);
    public IFactory GetObjectFromBin(byte[] args)
    {
        return MakeFromBin(args);
    }
}

public class CrewFactory : Factory
{

    protected override IFactory MakeFromFTR(string[] args)
    {
        return new Crew(UInt64.Parse(args[0]), args[1], UInt64.Parse(args[2]), args[3], args[4], UInt16.Parse(args[5]), args[6]);
    }

    protected override IFactory MakeFromBin(byte[] args)
    {
        int IDOffset = 7;
        int NLOffset = 15;
        UInt16 NameLength = BitConverter.ToUInt16(args, NLOffset);
        int NameOffset = 17;
        int AgeOffset = 17 + NameLength;
        int PhoneOffset = 19 + NameLength;
        int PhoneLength = 12;
        int ELOffset = 31 + NameLength;
        UInt16 EmailLength = BitConverter.ToUInt16(args, ELOffset);
        int EmailOffset = 33 + NameLength;
        int PracticeOffset = 33 + NameLength + EmailLength;
        int RoleOffset = 35 + NameLength + EmailLength;

        UTF8Encoding utf8 = new UTF8Encoding();
        UInt64 ID = BitConverter.ToUInt64(args, IDOffset);
        string Name = utf8.GetString(args, NameOffset, NameLength);
        UInt16 Age = BitConverter.ToUInt16(args, AgeOffset);
        string Phone = utf8.GetString(args, PhoneOffset, PhoneLength);
        string Email = utf8.GetString(args, EmailOffset, EmailLength);
        UInt16 Practice = BitConverter.ToUInt16(args, PracticeOffset);
        string Role = utf8.GetString(args, RoleOffset, 1);

        return new Crew(ID, Name, Age, Phone, Email, Practice, Role);
    }
}

public class PassengerFactory : Factory
{
    protected override IFactory MakeFromFTR(string[] args)
    {
        return new Passenger(UInt64.Parse(args[0]), args[1], UInt64.Parse(args[2]), args[3], args[4], args[5], UInt64.Parse(args[6]));
    }

    protected override IFactory MakeFromBin(byte[] args)
    {
        int IDOffset = 7;
        int NLOffset = 15;
        UInt16 NameLength = BitConverter.ToUInt16(args, NLOffset);
        int NameOffset = 17;
        int AgeOffset = 17 + NameLength;
        int PhoneOffset = 19 + NameLength;
        int PhoneLength = 12;
        int ELOffset = 31 + NameLength;
        UInt16 EmailLength = BitConverter.ToUInt16(args, ELOffset);
        int EmailOffset = 33 + NameLength;
        int ClassOffset = 33 + NameLength + EmailLength;
        int MilesOffset = 34 + NameLength + EmailLength;

        UTF8Encoding utf8 = new UTF8Encoding();
        UInt64 ID = BitConverter.ToUInt64(args, IDOffset);
        string Name = utf8.GetString(args, NameOffset, NameLength);
        UInt16 Age = BitConverter.ToUInt16(args, AgeOffset);
        string Phone = utf8.GetString(args, PhoneOffset, PhoneLength);
        string Email = utf8.GetString(args, EmailOffset, EmailLength);
        string Class = utf8.GetString(args, ClassOffset, 1);
        UInt64 Miles = BitConverter.ToUInt64(args, MilesOffset);

        return new Passenger(ID, Name, Age, Phone, Email, Class, Miles);
    }
}
public class CargoFactory : Factory
{
    protected override IFactory MakeFromFTR(string[] args)
    {
        return new Cargo(UInt64.Parse(args[0]), Single.Parse(args[1]), args[2], args[3]);
    }

    protected override IFactory MakeFromBin(byte[] args)
    {
        int IDOffset = 7;
        int WeightOffset = 15;
        int CodeOffset = 19;
        int CodeLength = 6;
        int DLOffset = 25;
        UInt16 DescriptionLength = BitConverter.ToUInt16(args, DLOffset);
        int DescriptionOffset = 27;

        UTF8Encoding utf8 = new UTF8Encoding();
        UInt64 ID = BitConverter.ToUInt64(args, IDOffset);
        Single Weight = BitConverter.ToSingle(args, WeightOffset);
        string Code = utf8.GetString(args, CodeOffset, CodeLength);
        string Descritption = utf8.GetString(args, DescriptionOffset, DescriptionLength);

        return new Cargo(ID, Weight, Code, Descritption);
    }
}
public class CargoPlaneFactory : Factory
{
    protected override IFactory MakeFromFTR(string[] args)
    {
        return new CargoPlane(UInt64.Parse(args[0]), args[1], args[2], args[3], Single.Parse(args[4]));
    }

    protected override IFactory MakeFromBin(byte[] args)
    {
        int IDOffset = 7;
        int SerialOffset = 15;
        int SerialLength = 6;
        int CountryOffset = 25;
        int CountryLength = 3;
        int MLOffset = 28;
        UInt16 ModelLength = BitConverter.ToUInt16(args, MLOffset);
        int ModelOffset = 30;
        int MaxLoadOffset = 30 + ModelLength;

        UTF8Encoding utf8 = new UTF8Encoding();
        UInt64 ID = BitConverter.ToUInt64(args, IDOffset);
        string Serial = utf8.GetString(args, SerialOffset, SerialLength);
        string Country = utf8.GetString(args, CountryOffset, CountryLength);
        string Model = utf8.GetString(args, ModelOffset, ModelLength);
        Single MaxLoad = BitConverter.ToSingle(args, MaxLoadOffset);

        return new CargoPlane(ID, Serial, Country, Model, MaxLoad);
    }
}

public class PassengerPlaneFactory : Factory
{
    protected override IFactory MakeFromFTR(string[] args)
    {
        return new PassengerPlane(UInt64.Parse(args[0]), args[1], args[2], args[3], UInt16.Parse(args[4]), UInt16.Parse(args[5]), UInt16.Parse(args[6]));
    }

    protected override IFactory MakeFromBin(byte[] args)
    {
        int IDOffset = 7;
        int SerialOffset = 15;
        int SerialLength = 6;
        int CountryOffset = 25;
        int CountryLength = 3;
        int MLOffset = 28;
        UInt16 ModelLength = BitConverter.ToUInt16(args, MLOffset);
        int ModelOffset = 30;
        int FirstClassOffset = 30 + ModelLength;
        int BusinessClassOffset = 32 + ModelLength;
        int EconomyClassOffset = 34 + ModelLength;


        UTF8Encoding utf8 = new UTF8Encoding();
        UInt64 ID = BitConverter.ToUInt64(args, IDOffset);
        string Serial = utf8.GetString(args, SerialOffset, SerialLength);
        string Country = utf8.GetString(args, CountryOffset, CountryLength);
        string Model = utf8.GetString(args, ModelOffset, ModelLength);
        UInt16 FirstClassSize = BitConverter.ToUInt16(args, FirstClassOffset);
        UInt16 BusinessClassSize = BitConverter.ToUInt16(args, BusinessClassOffset);
        UInt16 EconomyClassSize = BitConverter.ToUInt16(args, EconomyClassOffset);

        return new PassengerPlane(ID, Serial, Country, Model, FirstClassSize, BusinessClassSize, EconomyClassSize);
    }
}
public class AirportFactory : Factory
{
    protected override IFactory MakeFromFTR(string[] args)
    {
        return new Airport(UInt64.Parse(args[0]), args[1], args[2], Single.Parse(args[3]), Single.Parse(args[4]), Single.Parse(args[5]), args[6]);
    }

    protected override IFactory MakeFromBin(byte[] args)
    {
        int IDOffset = 7;
        int NLOffset = 15;
        UInt16 NameLength = BitConverter.ToUInt16(args, NLOffset);
        int NameOffset = 17;
        int CodeLength = 3;
        int CodeOffset = 17 + NameLength;
        int LongitudeOffset = 20 + NameLength;
        int LatittudeOffset = 24 + NameLength;
        int AMSLOffset = 28 + NameLength;
        int CountryOffset = 32 + NameLength;
        int CountryLength = 3;

        UTF8Encoding utf8 = new UTF8Encoding();
        UInt64 ID = BitConverter.ToUInt64(args, IDOffset);
        string Name = utf8.GetString(args, NameOffset, NameLength);
        string Code = utf8.GetString(args, CodeOffset, CodeLength);
        Single Longitude = BitConverter.ToSingle(args, LongitudeOffset);
        Single Latitude = BitConverter.ToSingle(args, LatittudeOffset);
        Single ASML = BitConverter.ToSingle(args, AMSLOffset);
        string Country = utf8.GetString(args, CountryOffset, CountryLength);
        return new Airport(ID, Name, Code, Longitude, Latitude, ASML, Country);
    }
}
public class FlightFactory : Factory
{
    protected override IFactory MakeFromFTR(string[] args)
    {
        return new Flight(UInt64.Parse(args[0]), UInt64.Parse(args[1]), UInt64.Parse(args[2]), args[3], args[4], Single.Parse(args[5]), Single.Parse(args[6]), Single.Parse(args[7]),
            UInt64.Parse(args[8]), Array.ConvertAll(args[9].Trim('[', ']').Split(';'), UInt64.Parse), Array.ConvertAll(args[10].Trim('[', ']').Split(';'), UInt64.Parse));
    }

    protected override IFactory MakeFromBin(byte[] args)
    {
        int IDOffset = 7;
        int OriginOffset = 15;
        int TargetOffset = 23;
        int TakeoffTimeOffset = 31;
        int LandingTimeOffset = 39;
        int PlaneIDOffset = 47;
        int CCOffset = 55;
        UInt16 CrewCount = BitConverter.ToUInt16(args, CCOffset);
        int CrewOffset = 57;
        int PCCOffset = 57 + 8 * CrewCount;
        UInt16 PassengerCargoCount = BitConverter.ToUInt16(args, PCCOffset);
        int PassengersCargoOffset = 59 + 8 * CrewCount;

        UTF8Encoding utf8 = new UTF8Encoding();
        UInt64 ID = BitConverter.ToUInt64(args, IDOffset);
        UInt64 Origin = BitConverter.ToUInt64(args, OriginOffset);
        UInt64 Target = BitConverter.ToUInt64(args, TargetOffset);
        string TakeoffTime = (DateTime.UnixEpoch + TimeSpan.FromMilliseconds(BitConverter.ToUInt64(args, TakeoffTimeOffset))).ToString();
        string LandingTime = (DateTime.UnixEpoch + TimeSpan.FromMilliseconds(BitConverter.ToUInt64(args, LandingTimeOffset))).ToString();
        UInt64 PlaneID = BitConverter.ToUInt64(args, PlaneIDOffset);
        UInt64[] Crew = new UInt64[CrewCount];
        for (int i = 0; i<CrewCount; i++)
        {
            Crew[i] = BitConverter.ToUInt64(args, CrewOffset + i*8);
        }
        UInt64[] PassengerCargo = new UInt64[PassengerCargoCount];
        for (int i = 0; i < PassengerCargoCount; i++)
        {
            PassengerCargo[i] = BitConverter.ToUInt64(args, PassengersCargoOffset + i * 8);
        }
        return new Flight(ID, Origin, Target, TakeoffTime, LandingTime, null, null, null, PlaneID, Crew, PassengerCargo);
    }
}

class FactoryDictionary
{
    public static Dictionary<string, Factory> factories = new Dictionary<string, Factory>{
        {"C", new CrewFactory() },
        {"NCR", new CrewFactory() },
        {"P", new PassengerFactory() },
        {"NPA", new PassengerFactory() },
        {"CA", new CargoFactory() },
        {"NCA", new CargoFactory() },
        {"CP", new CargoPlaneFactory() },
        {"NCP", new CargoPlaneFactory() },
        {"PP", new PassengerPlaneFactory() },
        {"NPP", new PassengerPlaneFactory() },
        {"AI", new AirportFactory() },
        {"NAI", new AirportFactory() },
        {"FL", new FlightFactory() },
        {"NFL", new FlightFactory() }
    };
    public static IFactory CreateFromFTR(string type, string[] data)
    {
        return factories[type].GetObjectFromFTR(data);
    }
    public static IFactory CreateFromBin(byte[] args)
    {
        UTF8Encoding utf8 = new UTF8Encoding();
        return factories[utf8.GetString(args, 0, 3)].GetObjectFromBin(args);
    }
}
