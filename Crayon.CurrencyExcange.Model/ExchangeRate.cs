using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Crayon.CurrencyExchange.Service.Models
{
    public class ExchangeRate
    {
        [JsonProperty(PropertyName = "rates")]
        public Dictionary<DateTime, Rate> Rates { get; set; }
    }

    public class Rate
    {
        [JsonExtensionData]
        public Dictionary<string, JToken> Fields { get; set; }
    }
}