using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StatsDog.Models
{
  public class StatsSummary
  {
    //-------------------------------------------------------------------------

    public struct UniqueSourceCountByDate
    {
      public DateTime Date { get; set; }
      public int Count { get; set; }
    }

    //-------------------------------------------------------------------------

    [DisplayName("Avg. unique sources per day")]
    public uint AverageUniqueSourcesPerDay { get; set; }

    [DisplayName("Recent unique sources per day")]
    public List<UniqueSourceCountByDate> RecentUniqueSourcesPerDay { get; set; }

    //-------------------------------------------------------------------------
  }
}