using NetworkSourceSimulator;
using FlightTrackerGUI;
using System.Text;
using DynamicData;
using System;

namespace Lab1;

public class SourceReading
{
    public NetworkSourceSimulator.NetworkSourceSimulator Source;
    public List<IFactory> BinObjects;
    public Dictionary<ulong, Airport> Airports;
    public List<Flight> Flights;
    public Dictionary<string, Action<Message>> AddObj;


    public SourceReading()
    {
        Source = new NetworkSourceSimulator.NetworkSourceSimulator("example_data.ftr", 1, 2);
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
        Source.Run();
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

