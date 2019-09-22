using Crayon.CurrencyExchange.Business.Extensions;
using Crayon.CurrencyExchange.Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;

namespace Crayon.CurrencyExchange.Business
{
    public class CurrencyExchangeBusiness : ICurrencyExchangeBusiness
    {
        public string GetCurrency(string dates, string baseCurrency, string targetCurrency)
        {
            var uri = ConfigurationManager.AppSettings["uri"];
            
            var inputDateList = dates.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var datesList = new List<DateTime>();

            if (string.IsNullOrWhiteSpace(baseCurrency))
            {
                throw new ArgumentException("Base currency is required parameter.");         
            }

            if (string.IsNullOrWhiteSpace(targetCurrency))
            {
                throw new ArgumentException("Target currency is required parameter.");
            }

            for (int i = 0; i < inputDateList.Length; i++)
            {
                datesList.Add(DateTime.Parse(inputDateList[i]));
            }

            if (!datesList.Any() || datesList.Count() < 2)
            {
                throw new ArgumentException("Please provide at least two dates.");                
            }

            var minDate = datesList.Min(x => x);
            var maxDate = datesList.Max(x => x);

            var path = string.Format(uri, minDate.ToShortDateTime(), maxDate.ToShortDateTime(), baseCurrency, targetCurrency);
            var request = WebRequest.Create(path);

            var response = request.GetResponse();

            using (var responseStream = response.GetResponseStream())
            {
                var reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                var result = reader.ReadToEnd();

                var fetch = JsonConvert.DeserializeObject<ExchangeRate>(result);

                var minRateItem = fetch.Rates.First();
                var maxRateItem = fetch.Rates.First();
                var avgRate = 0.0d;
                var valueList = new List<double>();

                foreach (var item in fetch.Rates)
                {
                    var rateItem = item.Value.Fields.First(x => x.Key == targetCurrency);

                    minRateItem = double.Parse(minRateItem.Value.Fields.First(x => x.Key == targetCurrency).Value.ToString()) < double.Parse(rateItem.Value.ToString()) ? minRateItem : item;
                    maxRateItem = double.Parse(maxRateItem.Value.Fields.First(x => x.Key == targetCurrency).Value.ToString()) > double.Parse(rateItem.Value.ToString()) ? maxRateItem : item;

                    valueList.Add(double.Parse(rateItem.Value.ToString()));
                }

                avgRate = Math.Round(valueList.Average(), 12);

                var strResponse = string.Format(
                "A min rate of {0} on {1} " +
                "A max rate of {2} on {3} " +
                "An average rate of {4} ", minRateItem.Value.Fields.First(x => x.Key == targetCurrency).Value.ToString(), minRateItem.Key.ToShortDateTime(),
                                          maxRateItem.Value.Fields.First(x => x.Key == targetCurrency).Value.ToString(), maxRateItem.Key.ToShortDateTime(), avgRate);

                return strResponse;
            }
        }
    }
}
