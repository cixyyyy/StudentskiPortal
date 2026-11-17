using System.Collections.Generic;

namespace PrijavaIspita.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? IsActive { get; set; }
        public string? Username { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
