using BallScanner.Data.Tables;
using System.Data.Entity;

namespace BallScanner.Data
{
    // singleton
    public class AppDbContext : DbContext
    {
        private static AppDbContext instance = null;

        // Entities
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Report> Reports { get; set; }

        private AppDbContext() : base("DefaultConnection") { }

        public static AppDbContext GetInstance()
        {
            if (instance == null)
                instance = new AppDbContext();
            return instance;
        }

    }
}
