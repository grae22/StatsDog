using System.Web.Http;
using StatsDog.Dtos;

namespace StatsDog.Controllers.Api
{
  public class StatsController : ApiController
  {
    //-------------------------------------------------------------------------
    // POST: /api/stats

    [HttpPost]
    public IHttpActionResult AddRecord(StatsDto statsDto)
    {
      return Ok();
    }

    //-------------------------------------------------------------------------
  }
}
