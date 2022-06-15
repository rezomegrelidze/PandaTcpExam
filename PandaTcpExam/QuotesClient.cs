using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PandaTcpExam.Common;

namespace PandaTcpExam;

public class QuotesClient
{
    private readonly ObservableCollection<QuoteDataViewModel> _collection;
    private readonly Dictionary<string, int> _symbolIndex;

    public QuotesClient(ObservableCollection<QuoteDataViewModel> collection,Dictionary<string,int> symbolIndex)
    {
        _collection = collection;
        _symbolIndex = symbolIndex;
    }

    public async Task StartReading()
    {
        await OpenTcpStream();
    }

    private async Task OpenTcpStream()
    {
        var tcpClient = new TcpClient();
        tcpClient.Connect("localhost", 50000);

        using (var n = tcpClient.GetStream())
        using (var reader = new StreamReader(n))
        {
            while (!reader.EndOfStream)
            {
                string json = await reader.ReadLineAsync();
                if (string.IsNullOrEmpty(json))
                {
                    throw new Exception("Server error!");
                }
                QuoteData data = JsonConvert.DeserializeObject<QuoteData>(json);

                if (_symbolIndex.TryGetValue(data.Symbol, out int index))
                {
                    _collection[index] = QuoteDataViewModel.FromQuoteData(data);
                }
                else
                {
                    _collection.Add(QuoteDataViewModel.FromQuoteData(data));
                    _symbolIndex[data.Symbol] = _collection.Count - 1;
                }
            }
        }
    }
}