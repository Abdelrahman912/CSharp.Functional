using RestSharp;
using System.Threading.Tasks;

namespace Chapter13
{
    public class CurrencyLayer2
    {
        public static Task<RestResponse> GetRate(string from , string to)
        {
            var client = new RestClient($"https://api.apilayer.com/exchangerates_data/convert?to={to}&from={from}&amount=1");

            var request = new RestRequest();
            request.AddHeader("apikey", "Y6vgsKtCVq7TMNEjX0nVk6GXlLDbYt0N");

            return client.GetAsync(request);
        }
    }
}
