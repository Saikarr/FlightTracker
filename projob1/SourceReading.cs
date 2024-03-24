using NetworkSourceSimulator;
using FlightTrackerGUI;
using System.Text;
using DynamicData;
namespace Lab1;

public class SourceReading
{
    public NetworkSourceSimulator.NetworkSourceSimulator Source;
    public List<IFactory> BinObjects;
    //public CancellationToken Token { get; set; }
    public Dictionary<int, Airport> Airports;
    public List<Flight> Flights;


    public SourceReading(/*CancellationToken token*/)
    {
        Source = new NetworkSourceSimulator.NetworkSourceSimulator("example_data.ftr", 1, 2);
        BinObjects = new List<IFactory>();
        Airports = new Dictionary<int, Airport>();
        Flights = new List<Flight>();
        //Token = token;
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
        try
        {
            Source.Run();
        }
        catch (OperationCanceledException ex)
        {
            return;
        }
    }
    public void MessageReached(object sender, NewDataReadyArgs e)
    {
        //Token.ThrowIfCancellationRequested();
        Message message = ((NetworkSourceSimulator.NetworkSourceSimulator)sender).GetMessageAt(e.MessageIndex);
        Creator(message);
    }
    public void Creator(Message message)
    {
        UTF8Encoding utf8 = new UTF8Encoding();
        Monitor.Enter(Flights); 
        switch (utf8.GetString(message.MessageBytes, 0, 3))
        {
            case "NAI":
                Airport airport = (Airport)FactoryDictionary.CreateFromBin(message.MessageBytes);
                Airports.Add((int)airport.ID, airport);
                break;
            case "NFL":
                Flights.Add((Flight)FactoryDictionary.CreateFromBin(message.MessageBytes));
                break;
            default:
                BinObjects.Add(FactoryDictionary.CreateFromBin(message.MessageBytes));
                break;
        }
        Monitor.Exit(Flights);

    }
}

