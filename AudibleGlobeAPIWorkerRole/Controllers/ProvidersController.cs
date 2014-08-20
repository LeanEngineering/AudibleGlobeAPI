using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AudibleGlobeApiWorkerRole.Models;
using AudibleGlobeApiWorkerRole.DAO;
using Dapper;

namespace AudibleGlobeApiWorkerRole.Controllers
{
    public class ProvidersRepository
    {
        public IEnumerable<Provider> GetProviders()
        {
            using (var db = Db.AzureSql())
            {
                return db.Query<Provider>("SELECT ProviderID, ProviderName FROM Providers");
            }
        }

        public IEnumerable<Provider> GetProviderByProviderId(int providerId)
        {
            using (var db = Db.AzureSql())
            {
                return db.Query<Provider>("SELECT ProviderID, ProviderName FROM Providers WHERE ProviderID = @ProviderID", new { ProviderID = providerId });
            }
        }
    }

    public class ProvidersController : ApiController
    {
        private readonly ProvidersRepository _providersRepository;

        public ProvidersController()
        {
            _providersRepository = new ProvidersRepository();
        }

        [Route("providers")]
        public IHttpActionResult GetProviders()
        {
            return Ok( _providersRepository.GetProviders());
        }

        [Route("providers/{providerId:int}")]
        public IHttpActionResult GetProviderById(int id)
        {
            var provider = _providersRepository.GetProviderByProviderId(id).FirstOrDefault();

            if (provider != null)
            {
                return Ok(provider);
            }
        
            return NotFound();
        }

    }
}