using System;
using System.Web.Http;
using Autofac;
using Crayon.CurrencyExchange.Business;

namespace Crayon.CurrencyExchange.Service.Controllers
{
    public class CurrencyController : ApiController
    {
        #region Fields

        private static IContainer Container { get; set; }

        #endregion        

        #region Constructors

        public CurrencyController()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CurrencyExchangeBusiness>().As<ICurrencyExchangeBusiness>();
            Container = builder.Build();
        }

        #endregion

        public IHttpActionResult Get(string dates, string baseCurrency, string targetCurrency)
        {
            try
            {       
                using (var scope = Container.BeginLifetimeScope())
                {
                    var currencyBusiness = scope.Resolve<ICurrencyExchangeBusiness>();
                    var result = currencyBusiness.GetCurrency(dates, baseCurrency, targetCurrency);
                    return Ok(result);
                }              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
