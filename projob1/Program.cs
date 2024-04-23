using System.Text.Json;

namespace Lab1;

using System.Diagnostics;
using Avalonia.Data;
using DynamicData;
using FlightTrackerGUI;
using Mapsui.Projections;
using static Mapsui.Nts.Providers.Shapefile.Indexing.QuadTree;
using NetworkSourceSimulator;
using System;

public class Program
{
    private List<IFactory> FTRObjects;
    //private List<Airport> Airports = new();
    //private List<CargoPlane> CargoPlanes = new();
    //private List<PassengerPlane> PassengerPlanes = new();
    //private List<Cargo> Cargos = new();
    //private List<Crew> Crews = new();
    //private List<Passenger> Passenger = new();
    //private List<Flight> Flights = new();
    public Dictionary<ulong, Airport> Airports = new();
    public Dictionary<ulong, CargoPlane> CargoPlanes = new();
    public Dictionary<ulong, PassengerPlane> PassengerPlanes = new();
    public Dictionary<ulong, Cargo> Cargos = new();
    public Dictionary<ulong, Crew> Crews = new();
    public Dictionary<ulong, Passenger> Passenger = new();
    public Dictionary<ulong, Flight> Flights = new();
    public Dictionary<ulong, Airport> AirDict = new();
    private SourceReading SourceReading;
    public StreamWriter LogWriter;
    public static void Main()
    {
        var program = new Program();
        program.LoadObjects();
        program.StartMap();
        
        //program.WaitForInput();
    }

    public Program()
    {
        FTRObjects = new List<IFactory>();
        SourceReading = new SourceReading(this);
        string file = $"Logs_{DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd")}.txt";
        LogWriter = new StreamWriter(file,true);
        LogWriter.AutoFlush = true;
        LogWriter.WriteLine();
        LogWriter.WriteLine("-----------------------");
        LogWriter.WriteLine();
    }
    public void Report()
    {
        var tv1 = new Television("Telewizja Abelowa");
        var tv2 = new Television("Kanał TV-tensor");
        var radio1 = new Radio("Radio Kwantyfikator");
        var radio2 = new Radio("Radio Shmem");
        var paper1 = new Newspaper("Gazeta Kategoryczna");
        var paper2 = new Newspaper("Dziennik Politechniczny");
        List<Media> medias = new List<Media>() { tv1, tv2, radio1, radio2, paper1, paper2 };
        List<IReportable> reps = [.. Airports.Values];
        reps.AddRange(PassengerPlanes.Values);
        reps.AddRange(CargoPlanes.Values);
        var news = new NewsGenerator(medias, reps);
        news.PrintAll();
    }
    public void StartMap()
    {
        ThreadStart threadDelegate = new ThreadStart(UpdateMap);
        Thread newthread = new Thread(threadDelegate);
        newthread.IsBackground = true;
        newthread.Start();
        Runner.Run();
    }
    public void UpdateMap()
    {
        SourceReading.MakeThread();
        FlightsGUIData data = new FlightsGUIData();
        while (true)
        {
            var t = Task.Run(async delegate
            {
                await Task.Delay(1000);
            });
            //Monitor.Enter(SourceReading.Flights);
            //List<Flight> flights = SourceReading.Flights.ToList();
            //Monitor.Exit(SourceReading.Flights);
            List<FlightGUI> list = new List<FlightGUI>();
            lock (Flights)
            {
                foreach (Flight flight in Flights.Values)
                {
                    ModifyFlightGUIList(list, flight);
                }
            }
            data.UpdateFlights(list);
            Runner.UpdateGUI(data);
            t.Wait();
        }
    }

    public void ModifyFlightGUIList(List<FlightGUI> list, Flight flight)
    {
        (TimeOnly takeoff, TimeOnly landing) = flight.FromTakeLand();
        bool sameday = true;
        if (takeoff > landing)
        {
            sameday = false;
        }
        TimeOnly cur = TimeOnly.FromDateTime(DateTime.Now);
        if ((sameday && cur > takeoff && cur < landing) || (!sameday && (cur > takeoff || cur < landing)))
        {
            WorldPosition curpos = flight.CalcPos(AirDict, takeoff, landing, sameday);
            double rotation = flight.CalcRot(AirDict);
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
        //SourceReading.MakeThread();
        bool exit = false;
        Action action;
        Dictionary<string, Action> actions = new Dictionary<string, Action>
        {
            //{"print", Print},
            {"report", Report},
            {"exit", action = () => {
            exit = true;}
            }
        };
        string? command;

        while (true)
        {
            Console.WriteLine("report or exit?");
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
        Action<string, string[]> action;
        var AddObj = new Dictionary<string, Action<string, string[]>>
        {
            {"C", action = (type, args)=> Crews.Add(UInt64.Parse(args[0]), (Crew)FactoryDictionary.CreateFromFTR(type, args)) },
            {"P", action = (type, args)=> Passenger.Add(UInt64.Parse(args[0]), (Passenger)FactoryDictionary.CreateFromFTR(type, args)) },
            {"CA", action = (type, args)=> Cargos.Add(UInt64.Parse(args[0]), (Cargo)FactoryDictionary.CreateFromFTR(type, args)) },
            {"CP", action = (type, args)=> CargoPlanes.Add(UInt64.Parse(args[0]), (CargoPlane)FactoryDictionary.CreateFromFTR(type, args)) },
            {"PP", action =(type, args) => PassengerPlanes.Add(UInt64.Parse(args[0]), (PassengerPlane) FactoryDictionary.CreateFromFTR(type, args)) },
            {"AI", action =(type, args) => {Airports.Add(UInt64.Parse(args[0]), (Airport) FactoryDictionary.CreateFromFTR(type, args));
            AirDict.Add(UInt64.Parse(args[0]), (Airport) FactoryDictionary.CreateFromFTR(type, args));} },
            {"FL", action =(type, args) => Flights.Add(UInt64.Parse(args[0]), (Flight) FactoryDictionary.CreateFromFTR(type, args)) }
        };
        string path = "example_data.ftr";
        using StreamReader sr = new StreamReader(path);
        string? s;
        while ((s = sr.ReadLine()) != null)
        {
            string[] tab = s.Split(',');
            string type = tab[0];
            string[] args = tab.Skip(1).ToArray();
            AddObj[type](type, args);
            //FTRObjects.Add(FactoryDictionary.CreateFromFTR(type, args));
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
