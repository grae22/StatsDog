using System;
using System.Diagnostics;
using System.IO;
using System.Text;
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
      var stringBuilder = new StringBuilder();

      stringBuilder.Append("StatsEvent: |");
      stringBuilder.Append($" {DateTime.Now:yyyy-MM-dd HH:mm:ss} |");
      stringBuilder.Append($" {statsDto.ApplicationName.PadRight(64)} |");
      stringBuilder.Append($" {statsDto.ApplicationVersion.PadRight(16)} |");
      stringBuilder.Append($" {statsDto.SourceName.PadRight(128)} |");
      stringBuilder.Append($" {statsDto.EventName.PadRight(32)} |");

      string output = stringBuilder.ToString();

      Debug.WriteLine(output);

      using (var writer = File.AppendText(@"d:\dev\projects\statsdog\deploy\stats.txt"))
      {
        writer.WriteLine(output);
      }

      return Ok();
    }

    //-------------------------------------------------------------------------
  }
}
