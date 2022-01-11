using BallScanner.Data.Tables;
using System.Data.Entity;

namespace BallScanner.Data
{
    public class AppDbContext : DbContext
    {
        // Entities
        public virtual DbSet<User> Users { get; set; }

        public AppDbContext() : base("DefaultConnection") { }


    }
}
