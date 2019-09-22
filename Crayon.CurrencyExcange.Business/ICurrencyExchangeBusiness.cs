using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crayon.CurrencyExchange.Business
{
    public interface ICurrencyExchangeBusiness
    {
        string GetCurrency(string dates, string baseCurrency, string targetCurrency);
    }
}
