using AutoMapper;
using StatsDog.Dtos;
using StatsDog.Models;

namespace StatsDog
{
  public static class AutoMapperConfig
  {
    public static void Initialise()
    {
      Mapper.Initialize(cfg =>
        {
          cfg.CreateMap<StatsDto, Stats>()
            .ForMember(member => member.Id, option => option.Ignore())
            .ForMember(member => member.Timestamp, option => option.Ignore());
        });

      Mapper.AssertConfigurationIsValid();
    }
  }
}