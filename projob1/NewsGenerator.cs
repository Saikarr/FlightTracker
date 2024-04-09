using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Input;

namespace Lab1;
public class NewsGenerator
{
    private List<Media> medias;
    private List<IReportable> reportables;

    private int MediasIter = 0;
    private int ReportablesIter = 0;

    public NewsGenerator(List<Media> medias, List<IReportable> reportables)
    {
        this.medias = medias;
        this.reportables = reportables;
    }

    public string GenerateNextNews()
    {
        string news = reportables[ReportablesIter].Accept(medias[MediasIter]);
        MediasIter++;
        if (MediasIter == medias.Count)
        {
            MediasIter = 0;
            ReportablesIter++;
            if (ReportablesIter == reportables.Count)
            {
                ReportablesIter = 0;
                MediasIter = 0;
                return null;
            }
        }
        return news;
    }

    public void PrintAll()
    {
        string news = GenerateNextNews();
        while(news!= null)
        {
            Console.WriteLine(news);
            news = GenerateNextNews();
        }
    }
}

