using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using StatsDog.Models;

namespace StatsDog.Controllers
{
  public class StatsController : Controller
  {
    //-------------------------------------------------------------------------

    private readonly ApplicationDbContext _context = new ApplicationDbContext();

    //-------------------------------------------------------------------------

    protected override void Dispose(bool disposing)
    {
      _context.Dispose();
    }

    //-------------------------------------------------------------------------
    // GET: /Stats/Index

    public ViewResult Index()
    {
      Response.AddHeader("Refresh", "300");
      return View(_context.Stats);
    }

    //-------------------------------------------------------------------------

    public ViewResult Summary()
    {
      Response.AddHeader("Refresh", "300");

      var summary = new StatsSummary();

      summary.AverageUniqueSourcesPerDay = GetAverageUniqueSourcePerDay(summary);
      summary.RecentUniqueSourcesPerDay = GetRecentUniqueSourcesPerDay();

      return View(summary);
    }

    //-------------------------------------------------------------------------

    private uint GetAverageUniqueSourcePerDay(StatsSummary summary)
    {
      var query = new StringBuilder();
      query.Append("SELECT ROUND(AVG(CAST(NumberOfUniqueSourceNamesByDay.[Count] AS float)), 0) ");
      query.Append("FROM ( ");
      query.Append("SELECT UniqueSourceNameByDay.[Day], COUNT(*) [Count] ");
      query.Append("FROM ( ");
      query.Append("SELECT CAST(Timestamp AS DATE) [Day], SourceName ");
      query.Append("FROM dbo.Stats ");
      query.Append("GROUP BY CAST(Timestamp AS DATE), SourceName ) UniqueSourceNameByDay ");
      query.Append("GROUP BY UniqueSourceNameByDay.[Day] ) NumberOfUniqueSourceNamesByDay");

      List<double?> results = _context.Database.SqlQuery<double?>(query.ToString()).ToList();

      if (results.Count == 0 ||
          results[0] == null)
      {
        return 0;
      }

      return (uint)results[0];
    }

    //-------------------------------------------------------------------------

    private List<StatsSummary.UniqueSourceCountByDate> GetRecentUniqueSourcesPerDay()
    {
      var query = new StringBuilder();
      query.Append("SELECT UniqueSourceNameByDay.[Day] [Date], COUNT(*) [Count] ");
      query.Append("FROM ( ");
      query.Append("SELECT CAST(Timestamp AS DATE) [Day], SourceName ");
      query.Append("FROM dbo.Stats ");
      query.Append("WHERE DATEDIFF(DAY, Timestamp, GETDATE()) < 7 ");
      query.Append("GROUP BY CAST(Timestamp AS DATE), SourceName ) UniqueSourceNameByDay ");
      query.Append("GROUP BY UniqueSourceNameByDay.[Day] ");
      query.Append("ORDER BY UniqueSourceNameByDay.[Day] DESC");

      return _context.Database.SqlQuery<StatsSummary.UniqueSourceCountByDate>(query.ToString()).ToList();
    }

    //-------------------------------------------------------------------------
  }
}