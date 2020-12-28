using Dapper.FluentMap.Dommel.Mapping;
using NetPOC.Backend.Domain.Entities;

namespace NetPOC.Backend.Infra.Maps
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