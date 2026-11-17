namespace PrijavaIspita.Models
{
    public class UserRole
    {
        public int UserRoleID { get; set; }

        public int KorisnikID { get; set; }
        public int RoleID { get; set; }

        public virtual Korisnik? Korisnik { get; set; } = null!;
        public virtual Role? Role { get; set; } = null!;
    }
}
