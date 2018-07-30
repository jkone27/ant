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
        [HttpGet]
        public IHttpActionResult Get()
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

