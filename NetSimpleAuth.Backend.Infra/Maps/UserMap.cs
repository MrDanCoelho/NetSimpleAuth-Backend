using Dapper.FluentMap.Dommel.Mapping;
using NetPOC.Backend.Domain.Entities;

namespace NetPOC.Backend.Infra.Maps
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