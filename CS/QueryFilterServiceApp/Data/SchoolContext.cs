using Microsoft.EntityFrameworkCore;

namespace QueryFilterServiceApp.Data {
    public class SchoolContext : DbContext {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
