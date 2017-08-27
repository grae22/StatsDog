using System.ComponentModel;

namespace StatsDog.Models
{
  public class StatsSummary
  {
    [DisplayName("Avg. unique sources per day")]
    public uint AverageUniqueSourcesPerDay { get; set; }
  }
}