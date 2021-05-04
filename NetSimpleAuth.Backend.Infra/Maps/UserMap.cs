using Dapper.FluentMap.Dommel.Mapping;
using NetSimpleAuth.Backend.Domain.Entities;

namespace NetSimpleAuth.Backend.Infra.Maps
{
    public class UserMap: DommelEntityMap<UserEntity>
    {
        public UserMap()
        {
            ToTable("User");
            Map(p => p.Id).IsKey();
        }
    }
}