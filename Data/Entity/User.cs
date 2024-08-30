using Microsoft.AspNetCore.Identity;

namespace Todo_List_API.Data.Entity
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public User() : base() { }
    }
}
