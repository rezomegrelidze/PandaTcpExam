using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
}
