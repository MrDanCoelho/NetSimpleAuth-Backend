using Dapper.FluentMap.Dommel.Mapping;
using NetSimpleAuth.Backend.Domain.Entities;

namespace NetSimpleAuth.Backend.Infra.Maps;

public class RefreshTokenMap: DommelEntityMap<RefreshTokenEntity>
{
    public RefreshTokenMap()
    {
        ToTable("RefreshToken");
        Map(p => p.Id).IsKey();
    }
}