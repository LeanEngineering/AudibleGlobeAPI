using System.Web.Http.Cors;
using Owin;
using System.Web.Http;

namespace AudibleGlobeApiWorkerRole
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }
}
