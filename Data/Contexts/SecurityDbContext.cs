using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo_List_API.Data.Entity;

namespace Todo_List_API.Data.Contexts
{
    public class SecurityDbContext : IdentityDbContext<User>
    {
        DbSet<TodoItem> TodoItems { get; set; }

        DbSet<User> Users { get; set; }

        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.AspNetUsersId);
        }
    }
}
