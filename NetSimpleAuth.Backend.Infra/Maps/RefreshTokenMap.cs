using Dapper.FluentMap.Dommel.Mapping;
using NetPOC.Backend.Domain.Entities;

namespace NetPOC.Backend.Infra.Maps
{
    public class RefreshTokenMap: DommelEntityMap<RefreshTokenEntity>
    {
        public RefreshTokenMap()
        {
            ToTable("RefreshToken");
            Map(p => p.Id).IsKey();
        }
    }
}