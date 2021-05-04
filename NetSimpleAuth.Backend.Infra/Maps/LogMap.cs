using Dapper.FluentMap.Dommel.Mapping;
using NetSimpleAuth.Backend.Domain.Entities;

namespace NetSimpleAuth.Backend.Infra.Maps
{
    public class LogMap : DommelEntityMap<LogEntity>
    {
        public LogMap()
        {
            ToTable("Log");
            Map(p => p.Id).IsKey();
        }
    }
}