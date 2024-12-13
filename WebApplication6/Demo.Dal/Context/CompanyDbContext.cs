using Demo.Dal.Entities;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Demo.DAL.Context
{
    public class CompanyDbContext: IdentityDbContext<AppUser>
    {
       
        public CompanyDbContext(DbContextOptions<CompanyDbContext>options):base(options) 
        {

        }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(/*"server=.; database= LastDemo ; trusted_connection=true"*/);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .IsRequired(false);
            base.OnModelCreating(modelBuilder);
        }

    }
}
 