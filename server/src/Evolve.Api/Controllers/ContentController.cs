using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Evolve.Api.Controllers
{
    public class ContentController : ApiController
    {
        [Route("{type}/{slug?}")]
        public async Task<dynamic> Get(string type, string slug = null)
        {

            return new { type = type, slug = slug };
        }


    }
}
