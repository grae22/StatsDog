using System;
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

    private struct LastEventDataBySource
    {
      public string SourceName { get; set; }
      public string Data { get; set; }
    }

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
      summary.TotalFilesOpened = GetTotalFilesOpenedCount();
      summary.RecentUniqueSourcesPerDay = GetRecentUniqueSourcesPerDay();
      summary.SourceCountByVersion = GetSourceCountsByVersion();

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

    private uint GetTotalFilesOpenedCount()
    {
      var query = new StringBuilder();
      query.Append("SELECT DISTINCT SourceName, ");
      query.Append("( SELECT TOP 1 Data ");
      query.Append("FROM dbo.Stats ");
      query.Append("WHERE SourceName = A.SourceName ");
      query.Append("ORDER BY Timestamp DESC ) [Data] ");
      query.Append("FROM dbo.Stats A ");

      var results = _context.Database.SqlQuery<LastEventDataBySource>(query.ToString()).ToList();

      return (uint)results.Sum(x => ExtractFilesOpenedCountFromData(x.Data));
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

    private List<StatsSummary.SourceCountPerApplicationVersion> GetSourceCountsByVersion()
    {
      var query = new StringBuilder();
      query.Append("SELECT ApplicationVersion, COUNT(*) [Count] ");
      query.Append("FROM ( ");
      query.Append("SELECT DISTINCT SourceName, ( ");
      query.Append("SELECT TOP 1 ApplicationVersion ");
      query.Append("FROM dbo.Stats ");
      query.Append("WHERE SourceName = A.SourceName ");
      query.Append("AND DATEDIFF(Day, Timestamp, GETDATE()) < 7 ");
      query.Append("ORDER BY Timestamp DESC ) ");
      query.Append("AS ApplicationVersion ");
      query.Append("FROM dbo.Stats AS A ) SourceNameAndVersion ");
      query.Append("WHERE ApplicationVersion IS NOT NULL ");
      query.Append("GROUP BY ApplicationVersion ");
      query.Append("ORDER BY ApplicationVersion DESC");

      return _context.Database.SqlQuery<StatsSummary.SourceCountPerApplicationVersion>(query.ToString()).ToList();
    }

    //-------------------------------------------------------------------------

    private static uint ExtractFilesOpenedCountFromData(string data)
    {
      const string tag = "FilesOpened:";

      if (data == null ||
          data.Any() == false)
      {
        return 0;
      }

      string filesOpenedData =
        data
          .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
          .FirstOrDefault(d => d.StartsWith(tag, StringComparison.OrdinalIgnoreCase));

      if (filesOpenedData == null)
      {
        return 0;
      }

      var countAsString = filesOpenedData.Substring(tag.Length, filesOpenedData.Length - tag.Length);

      uint count;

      if (uint.TryParse(countAsString, out count) == false)
      {
        return 0;
      }

      return count;
    }

    //-------------------------------------------------------------------------
  }
}