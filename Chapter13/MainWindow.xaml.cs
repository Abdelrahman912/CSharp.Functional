using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using CSharp.Functional.Extensions;

namespace Chapter13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private RestResponse Query()
        {
            var client = new RestClient("https://api.apilayer.com/fixer/convert?to=USD&from=EUR&amount=100");
            //client.a.Timeout = -1;

            var request = new RestRequest();
            request.AddHeader("apikey", "Y6vgsKtCVq7TMNEjX0nVk6GXlLDbYt0N");
            Thread.Sleep(5000);
            return client.Get(request);
        }

        private Task<RestResponse> QueryAsync()
        {
            var client = new RestClient("https://api.apilayer.com/fixer/convert?to=USD&from=EUR&amount=100");
            //client.a.Timeout = -1;

            var request = new RestRequest();
            request.AddHeader("apikey", "Y6vgsKtCVq7TMNEjX0nVk6GXlLDbYt0N");
            return Task.Run(() =>
            {
                Thread.Sleep(5000);
                return client.GetAsync(request);
            });

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //var content = await (from response in QueryAsync()
            //                     select response.Content);
            var f = "USD";
            var to = "EUR";
            Func<Task<string>> layer1 = () => CurrencyLayer1.GetRate(f, to)
                                                            .Map(r => $"From Layer1 \n {r.Content}");

            Func<Task<string>> layer2 = () => CurrencyLayer2.GetRate(f, to)
                                                            .Map(r => $"From Layer2 \n {r.Content}");



            var content = await layer1.Retry(5, 1000)
                                      .OrElse(() => layer2.Retry(5, 1000))
                                      .Recover(ex => ex.Message);

            MessageBox.Show(content);
        }

        private static Func<Flight, Flight, Flight> PickCheaper =>
            (l, r) => l.Price < r.Price ? l : r;

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IAirline ryanair = null; ;
            IAirline easyJet = null;
            var from = "California";
            var to = "England";
            var on = DateTime.Now;
            await PickCheaper.Async()
                       .Apply(ryanair.BestFare(from, to, on))
                       .Apply(easyJet.BestFare(from, to, on));
            //var response = Query();
            //var content = response.Content;
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var from = "California";
            var to = "England";
            var on = DateTime.Now;
            var airlines = new List<IAirline>() { new Airline1(), new Airline2(), new Airline3() };
            airlines.Select(line => line.Flights(from, to, on));//Using select -> IEnumerable<Task<Ienumerable<Flight>>>

            var flights = await airlines.TraverseA(line => line.Flights(from, to, on).Recover(ex=>Enumerable.Empty<Flight>()))
                                .Map(ls => ls.Flatten().OrderBy(f => f.Price).ToList());


        }
    }
}
