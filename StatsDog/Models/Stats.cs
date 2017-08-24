using System.ComponentModel.DataAnnotations;

namespace StatsDog.Models
{
  public class Stats
  {
    public int Id { get; set; }

    [StringLength(64)]
    public string ApplicationName { get; set; }

    [StringLength(16)]
    public string ApplicationVersion { get; set; }

    [StringLength(128)]
    public string SourceName { get; set; }

    [StringLength(32)]
    public string EventName { get; set; }
  }
}