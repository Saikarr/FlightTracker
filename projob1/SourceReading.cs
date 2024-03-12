using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1;
using NetworkSourceSimulator;
using System.Threading;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design;

namespace Lab1;

public class SourceReading
{
    public NetworkSourceSimulator.NetworkSourceSimulator Source;
    public List<IFactory> BinObjects;
    public CancellationToken Token { get; set; }

    public SourceReading(CancellationToken token)
    {
        Source = new NetworkSourceSimulator.NetworkSourceSimulator("example_data.ftr", 5, 10);
        BinObjects = new List<IFactory>();
        Token = token;
    }
    public void MakeThread()
    {  
        ThreadStart threadDelegate = new ThreadStart(ThreadWork);
        Thread newthread = new Thread(threadDelegate);
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
        Token.ThrowIfCancellationRequested();
        Message message = ((NetworkSourceSimulator.NetworkSourceSimulator)sender).GetMessageAt(e.MessageIndex);
        Creator(message);
    }
    public void Creator(Message message)
    {
        BinObjects.Add(FactoryDictionary.CreateFromBin(message.MessageBytes));
    }
}

