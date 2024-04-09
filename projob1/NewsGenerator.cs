using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Input;

namespace Lab1;
public class NewsGenerator
{
    private List<Media> Medias;
    private List<IReportable> Reportables;

    private int MediasIter = 0;
    private int ReportablesIter = 0;

    public NewsGenerator(List<Media> medias, List<IReportable> reportables)
    {
        this.Medias = medias;
        this.Reportables = reportables;
    }

    public string GenerateNextNews()
    {
        if (ReportablesIter == Reportables.Count)
        {
            ReportablesIter = 0;
            MediasIter = 0;
            return null;
        }
        string news = Reportables[ReportablesIter].Accept(Medias[MediasIter]);
        MediasIter++;
        if (MediasIter == Medias.Count)
        {
            MediasIter = 0;
            ReportablesIter++;
        }
        return news;
    }

    public void PrintAll()
    {
        string news = GenerateNextNews();
        while (news != null)
        {
            Console.WriteLine(news);
            news = GenerateNextNews();
        }
    }
}

