using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using PandaTcpExam.Common;

namespace PandaTcpExam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<QuoteDataViewModel> transDataCollection;

        private Dictionary<string, int> quoteSymbolIndex = new(); // Helps for fast indexing into the observableCollection

        public MainWindow()
        {
            InitializeComponent();
            transDataCollection = new ObservableCollection<QuoteDataViewModel>();
            MainDataGrid.ItemsSource = transDataCollection;
        }


        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            QuotesClient transStream = new QuotesClient(transDataCollection,quoteSymbolIndex);
            await transStream.StartReading();
        }
    }

    public class QuoteDataViewModel
    {
        public static QuoteDataViewModel FromQuoteData(QuoteData data)
        {
            return new QuoteDataViewModel
            {
                AssetName = data.Symbol,
                Bid = data.Bid,
                Ask = data.Ask,
                Time = new DateTime(data.TickTime).ToString("dd/MM/yyyy-HH:mm:ss.fff")
            };
        }

        public string Time { get; set; }

        public decimal Ask { get; set; }

        public decimal Bid { get; set; }

        public string AssetName { get; set; }
    }

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
}
