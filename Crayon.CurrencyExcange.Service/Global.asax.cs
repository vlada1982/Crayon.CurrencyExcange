using System.Web.Http;
using Crayon.CurrencyExhcange.Service;

namespace Crayon.CurrencyExchange.Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
