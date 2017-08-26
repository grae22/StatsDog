using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StatsDog.Models
{
  public class Stats
  {
    public int Id { get; set; }

    [DisplayName("Application Name")]
    [StringLength(64)]
    public string ApplicationName { get; set; }

    [DisplayName("Application Version")]
    [StringLength(16)]
    public string ApplicationVersion { get; set; }

    [DisplayName("Source")]
    [StringLength(128)]
    public string SourceName { get; set; }

    [DisplayName("Event")]
    [StringLength(32)]
    public string EventName { get; set; }

    [DisplayName("Timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.Now;
  }
}