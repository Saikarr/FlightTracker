using System.Text.Json;

namespace Lab1;

using Avalonia.Data;
using DynamicData;
using FlightTrackerGUI;
using Mapsui.Projections;
using NetworkSourceSimulator;

public class Program
{
    private List<IFactory> FTRObjects;
    private CancellationTokenSource TokenSource;
    private SourceReading SourceReading;
    public static void Main()
    {
        var program = new Program();
        program.Pom();
    }

    public void Pom()
    {
        ThreadStart threadDelegate = new ThreadStart(CreateMap);
        Thread newthread = new Thread(threadDelegate);
        newthread.IsBackground = true;
        newthread.Start();
        Runner.Run();
    }

    public Program()
    {
        FTRObjects = new List<IFactory>();
        TokenSource = new CancellationTokenSource();
        //var token = TokenSource.Token;
        SourceReading = new SourceReading();
    }

    public void CreateMap()
    {
        SourceReading.MakeThread();
        FlightsGUIData data = new FlightsGUIData();
        while (true)
        {
            Monitor.Enter(SourceReading.Flights);
            List<Flight> flights = SourceReading.Flights.ToList();
            Monitor.Exit(SourceReading.Flights);
            List<FlightGUI> list = new List<FlightGUI>();
            foreach (Flight flight in flights)
            {
                ModifyFlightGUIList(list, flight);
            }
            data.UpdateFlights(list);
            Runner.UpdateGUI(data);
            Thread.Sleep(1000);
        }
    }

    public void ModifyFlightGUIList(List<FlightGUI> list, Flight flight)
    {
        (TimeOnly takeoff, TimeOnly landing) = flight.FromTakeLand(flight);
        bool sameday = true;
        if (takeoff > landing)
        {
            sameday = false;
        }
        TimeOnly cur = TimeOnly.FromDateTime(DateTime.Now);
        if (sameday && cur > takeoff && cur < landing)
        {
            WorldPosition curpos = flight.CalcPos(flight, SourceReading.Airports, takeoff, landing, sameday);
            double rotation = flight.CalcRot(flight, SourceReading.Airports);
            list.Add(new FlightGUI
            {
                ID = flight.ID,
                WorldPosition = curpos,
                MapCoordRotation = rotation
            });
        }
        else if (!sameday && (cur > takeoff || cur < landing))
        {
            WorldPosition curpos = flight.CalcPos(flight, SourceReading.Airports, takeoff, landing, sameday);
            double rotation = flight.CalcRot(flight, SourceReading.Airports);
            list.Add(new FlightGUI
            {
                ID = flight.ID,
                WorldPosition = curpos,
                MapCoordRotation = rotation
            });
        }
    }

    public void WaitForInput()
    {
        SourceReading.MakeThread();
        Action action;
        bool exit = false;
        Dictionary<string, Action> actions = new Dictionary<string, Action>
        {
            {"print", Print},
            {"exit", action = () => {
            TokenSource.Cancel();
            exit = true; }
            }
        };
        string? command;

        while (true)
        {
            Console.WriteLine("print or exit?");
            command = Console.ReadLine();
            try
            {
                actions[command]();
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("wrong command");
            }
            if (exit) return;
        }
    }

    public void Print()
    {
        string s;
        JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            WriteIndented = true
        };
        DateTime localDate = DateTime.Now;
        string filename = $"snapshot_{localDate.ToString("HH_mm_ss")}.json";
        using StreamWriter sw = new StreamWriter(filename);
        s = JsonSerializer.Serialize(SourceReading.BinObjects, options);
        sw.Write(s);
    }
    public void LoadObjects()
    {
        string path = "example_data.ftr";
        using StreamReader sr = new StreamReader(path);
        string? s;
        while ((s = sr.ReadLine()) != null)
        {
            string[] tab = s.Split(',');
            string type = tab[0];
            string[] args = tab.Skip(1).ToArray();
            FTRObjects.Add(FactoryDictionary.CreateFromFTR(type, args));
        }
    }
    public void Serialize()
    {
        string s;
        JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            WriteIndented = true
        };
        string filename = "objects.json";
        using StreamWriter sw2 = new StreamWriter(filename);
        s = JsonSerializer.Serialize(FTRObjects, options);
        sw2.Write(s);
    }
}
