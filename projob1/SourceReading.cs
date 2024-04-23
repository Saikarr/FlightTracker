using NetworkSourceSimulator;
using FlightTrackerGUI;
using System.Text;
using DynamicData;
using System;
using NetTopologySuite.Index.HPRtree;
using System.Xml.Serialization;

namespace Lab1;

public class SourceReading
{
    public NetworkSourceSimulator.NetworkSourceSimulator Source;
    public List<IFactory> BinObjects;
    public Dictionary<ulong, Airport> Airports;
    public List<Flight> Flights;
    public Dictionary<string, Action<Message>> AddObj;
    public Program program;


    public SourceReading(Program program)
    {
        Source = new NetworkSourceSimulator.NetworkSourceSimulator("example.ftre", 10, 100);
        BinObjects = new List<IFactory>();
        Airports = new Dictionary<ulong, Airport>();
        Flights = new List<Flight>();
        Action<Message> action;
        AddObj = new Dictionary<string, Action<Message>>
        {
            {"NAI", action = message => { Airport airport = AirportFactory.MakeBin(message.MessageBytes);
                Airports.Add(airport.ID, airport); }},
            {"NFL", action = message => { Flights.Add(FlightFactory.MakeBin(message.MessageBytes)); }},
            {"NCR", action = message => { BinObjects.Add(FactoryDictionary.CreateFromBin(message.MessageBytes)); }},
            {"NPA", action = message => { BinObjects.Add(FactoryDictionary.CreateFromBin(message.MessageBytes)); }},
            {"NCA", action = message => { BinObjects.Add(FactoryDictionary.CreateFromBin(message.MessageBytes)); }},
            {"NCP", action = message => { BinObjects.Add(FactoryDictionary.CreateFromBin(message.MessageBytes)); }},
            {"NPP", action = message => { BinObjects.Add(FactoryDictionary.CreateFromBin(message.MessageBytes)); }}
        };
        this.program = program;
    }
    public void MakeThread()
    {
        ThreadStart threadDelegate = new ThreadStart(ThreadWork);
        Thread newthread = new Thread(threadDelegate);
        newthread.IsBackground = true;
        newthread.Start();
    }

    public void ThreadWork()
    {
        Source.OnNewDataReady += MessageReached;
        Source.OnIDUpdate += IDUpdate;
        Source.OnContactInfoUpdate += ContactUpdate;
        Source.OnPositionUpdate += PositionUpdate;
        Source.Run();
    }

    public void PositionUpdate(object sender, PositionUpdateArgs e)
    {
        if (program.Flights.ContainsKey(e.ObjectID))
        {
            program.LogWriter.WriteLine($"Position update: ID:{e.ObjectID}, old ASML:{program.Flights[e.ObjectID].AMSL}," +
                $" old Longitude: {program.Flights[e.ObjectID].Longitude}, old Latitude:{program.Flights[e.ObjectID].Latitude}," +
                $"  new ASML: {e.AMSL}, new Longitude: {e.Longitude}, new Latitude: {e.Latitude}");

            lock (program.Flights)
            {
                program.Flights[e.ObjectID].AMSL = e.AMSL;
                program.Flights[e.ObjectID].Latitude = e.Latitude;
                program.Flights[e.ObjectID].Longitude = e.Longitude;
                program.Flights[e.ObjectID].LastTime = TimeOnly.FromDateTime(DateTime.Now);
            }
        }
        else program.LogWriter.WriteLine($"Failed position update: ID:{e.ObjectID}, nonexistent");

    }
    public void ContactUpdate(object sender, ContactInfoUpdateArgs e)
    {
        if (program.Crews.ContainsKey(e.ObjectID))
        {
            program.LogWriter.WriteLine($"Contact update: ID:{e.ObjectID}, old email:{program.Crews[e.ObjectID].Email}," +
                $" old phone: {program.Crews[e.ObjectID].Phone}, new email: {e.EmailAddress}, new phone: {e.PhoneNumber}");
            program.Crews[e.ObjectID].Phone = e.PhoneNumber;
            program.Crews[e.ObjectID].Email = e.EmailAddress;
        }
        else if (program.Passenger.ContainsKey(e.ObjectID))
        {
            program.LogWriter.WriteLine($"Contact update: ID:{e.ObjectID}, old email:{program.Passenger[e.ObjectID].Email}," +
                $" old phone: {program.Passenger[e.ObjectID].Phone}, new email: {e.EmailAddress}, new phone: {e.PhoneNumber}");
            program.Passenger[e.ObjectID].Phone = e.PhoneNumber;
            program.Passenger[e.ObjectID].Email = e.EmailAddress;
        }
        else program.LogWriter.WriteLine($"Failed contact update: ID:{e.ObjectID}, nonexistent");
    }
    public void IDUpdate(object sender, IDUpdateArgs e)
    {
        bool error = false;
        bool repeated = false;
        if (program.Airports.ContainsKey(e.NewObjectID) || program.Cargos.ContainsKey(e.NewObjectID) || program.Flights.ContainsKey(e.NewObjectID) ||
            program.Passenger.ContainsKey(e.NewObjectID) || program.Crews.ContainsKey(e.NewObjectID) || program.PassengerPlanes.ContainsKey(e.NewObjectID) ||
            program.CargoPlanes.ContainsKey(e.NewObjectID))
        {
            repeated = true;
        }

        if (!repeated)
        {
            if (program.Airports.ContainsKey(e.ObjectID))
            {
                program.Airports[e.ObjectID].Update(program.Flights, e, program.Airports);
            }
            else if (program.Cargos.ContainsKey(e.ObjectID))
            {
                program.Cargos[e.ObjectID].Update(program.Flights, e, program.Cargos);
            }
            else if (program.Flights.ContainsKey(e.ObjectID))
            {
                program.Flights[e.ObjectID].Update(program.Flights, e);
            }
            else if (program.Passenger.ContainsKey(e.ObjectID))
            {
                program.Passenger[e.ObjectID].Update(e, program.Passenger);
            }
            else if (program.Crews.ContainsKey(e.ObjectID))
            {
                program.Crews[e.ObjectID].Update(program.Flights, e, program.Crews);
            }
            else if (program.PassengerPlanes.ContainsKey(e.ObjectID))
            {
                program.PassengerPlanes[e.ObjectID].Update(program.Flights, e, program.PassengerPlanes);
            }
            else if (program.CargoPlanes.ContainsKey(e.ObjectID))
            {
                program.CargoPlanes[e.ObjectID].Update(program.Flights, e, program.CargoPlanes);
            }
            else
            {
                program.LogWriter.WriteLine($"Failed ID update: ID:{e.ObjectID}, nonexistent");
                error = true;
            }
        }
        else
        {
            program.LogWriter.WriteLine($"Failed ID update: old ID:{e.ObjectID}, new ID: {e.NewObjectID}, repetition");
            return;
        }

        if (!error) program.LogWriter.WriteLine($"ID update: old ID:{e.ObjectID}, new ID: {e.NewObjectID}");
    }
    public void MessageReached(object sender, NewDataReadyArgs e)
    {
        Message message = ((NetworkSourceSimulator.NetworkSourceSimulator)sender).GetMessageAt(e.MessageIndex);
        Creator(message);
    }
    public void Creator(Message message)
    {
        UTF8Encoding utf8 = new UTF8Encoding();
        Monitor.Enter(Flights);
        AddObj[utf8.GetString(message.MessageBytes, 0, 3)](message);
        Monitor.Exit(Flights);
    }
}