using System.Collections.Generic;

namespace NetPOC.Backend.Domain.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
        public string PasswordSalt { get; set; }
    }
}