using System;
using System.Diagnostics;
using System.Text;
using System.Web.Http;
using AutoMapper;
using StatsDog.Dtos;
using StatsDog.Models;

namespace StatsDog.Controllers.Api
{
  public class StatsController : ApiController
  {
    //-------------------------------------------------------------------------

    private readonly ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

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

      Debug.WriteLine(stringBuilder.ToString());

      Stats stats = Mapper.Map<StatsDto, Stats>(statsDto);

      stats.Data = stats.Data ?? "";

      _applicationDbContext.Stats.Add(stats);
      _applicationDbContext.SaveChanges();

      return Ok();
    }

    //-------------------------------------------------------------------------
  }
}
