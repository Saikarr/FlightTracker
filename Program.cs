using System.Reflection;
using System.Text.Json;

namespace Lab1;
public class Program
{
    private List<IFactory> list = new List<IFactory>();

    public static void Main()
    {
        var program = new Program();
        program.LoadObjects();
        program.Serialize();
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
            list.Add(FactoryDictionary.Create(type, args));
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
        s = JsonSerializer.Serialize(list, options);
        sw2.Write(s);
        //foreach (var obj in list)
        //{
        //    s = JsonSerializer.Serialize<object>(obj, options);
        //    sw2.Write(s);
        //}

    }

}
