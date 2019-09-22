using System;

namespace Crayon.CurrencyExchange.Service.Extensions
{
    public static class CurrencyExchangeExtensions
    {
        public static string ToShortDateTime(this DateTime input)
        {
            return input.ToString("yyyy-MM-dd");
        }
    }
}