using System.Web.Http;

namespace AntNet45Tests.Controller
{
    [RoutePrefix("api/reservations")]
    [CustomAuthorize("read")]
    public class ReservationsController : ApiController
    {
        private readonly ISomeDependency someDependency;

        public ReservationsController(ISomeDependency someDependency)
        {
            this.someDependency = someDependency;
        }
        
        [Route("")]
        public IHttpActionResult Get()
        {
            someDependency.Do();
            return Ok();
        }

        
        [HttpGet]
        [AllowAnonymous]
        [Route("v2/complex/all")]
        public IHttpActionResult GetMoreComplexRoute()
        {
            someDependency.Do();
            return Ok();
        }

        [AllowAnonymous]
        [Route("v2/complex/another")]
        public IHttpActionResult OptionsMoreComplexRoute()
        {
            someDependency.Do();
            return Ok();
        }

        [Route("")]
        [HttpPost]
        [CustomAuthorize("write")]
        public IHttpActionResult Post([FromBody] ReservationDto reservation)
        {
            someDependency.Do();
            return Ok();
        }
    }
}

