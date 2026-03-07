using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend.API.Models;

namespace Backend.API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // UserGroup
            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            // Schedule
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.User)
                .WithMany(u => u.Schedules)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Schedules)
                .HasForeignKey(s => s.GroupId);

            // Substitution
            modelBuilder.Entity<Substitution>()
                .HasOne(s => s.CreatedByUser)
                .WithMany()
                .HasForeignKey(s => s.CreatedByUserId);

            // SubstitutionGroup
            modelBuilder.Entity<SubstitutionGroup>()
                .HasOne(sg => sg.Substitution)
                .WithMany(s => s.SubstitutionGroups)
                .HasForeignKey(sg => sg.SubstitutionId);

            modelBuilder.Entity<SubstitutionGroup>()
                .HasOne(sg => sg.Group)
                .WithMany(g => g.SubstitutionGroups)
                .HasForeignKey(sg => sg.GroupId);

            // SubstitutionNotification
            modelBuilder.Entity<SubstitutionNotification>()
                .HasOne(sn => sn.Substitution)
                .WithMany(s => s.SubstitutionNotifications)
                .HasForeignKey(sn => sn.SubstitutionId);

            modelBuilder.Entity<SubstitutionNotification>()
                .HasOne(sn => sn.User)
                .WithMany(u => u.SubstitutionNotifications)
                .HasForeignKey(sn => sn.UserId);

            // SubstitutionTaken
            modelBuilder.Entity<SubstitutionTaken>()
                .HasOne(st => st.Substitution)
                .WithMany(s => s.SubstitutionTakens)
                .HasForeignKey(st => st.SubstitutionId);

            modelBuilder.Entity<SubstitutionTaken>()
                .HasOne(st => st.TakenByUser)
                .WithMany(u => u.SubstitutionTakens)
                .HasForeignKey(st => st.TakenByUserId);

            // Overtime
            modelBuilder.Entity<Overtime>()
                .HasOne(o => o.User)
                .WithMany(u => u.Overtimes)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Overtime>()
                .HasOne(o => o.Substitution)
                .WithMany(s => s.Overtimes)
                .HasForeignKey(o => o.SubstitutionId);
        }
    }
}