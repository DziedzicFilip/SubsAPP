using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend.API.Models;
using Backend.API.Models.Entities;
namespace Backend.API.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
            public DbSet<Group> Groups { get; set; }
            public DbSet<UserGroup> UserGroups { get; set; }
            public DbSet<Schedule> Schedules { get; set; }
            public DbSet<Substitution> Substitutions { get; set; }
            public DbSet<SubstitutionGroup> SubstitutionGroups { get; set; }
            public DbSet<SubstitutionNotification> SubstitutionNotifications { get; set; }
            public DbSet<SubstitutionTaken> SubstitutionTakens { get; set; }
            public DbSet<Overtime> Overtimes { get; set;}
            public DbSet<ApplicationUser> ApplicationUsers { get; set; }
            public DbSet<User> Users { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    
    modelBuilder.Entity<UserGroup>()
        .HasKey(ug => new { ug.UserId, ug.GroupId });

    modelBuilder.Entity<UserGroup>()
        .HasOne(ug => ug.User)
        .WithMany(u => u.UserGroups)
        .HasForeignKey(ug => ug.UserId);

    modelBuilder.Entity<UserGroup>()
        .HasOne(ug => ug.Group)
        .WithMany(g => g.UserGroups)
        .HasForeignKey(ug => ug.GroupId);

    
    modelBuilder.Entity<Schedule>()
        .HasOne(s => s.User)
        .WithMany(u => u.Schedules)
        .HasForeignKey(s => s.UserId);

    modelBuilder.Entity<Schedule>()
        .HasOne(s => s.Group)
        .WithMany(g => g.Schedules)
        .HasForeignKey(s => s.GroupId);

    
    modelBuilder.Entity<Substitution>()
        .HasOne(s => s.CreatedByUser)
        .WithMany()
        .HasForeignKey(s => s.CreatedByUserId);

    
    modelBuilder.Entity<SubstitutionGroup>()
        .HasKey(sg => new { sg.SubstitutionId, sg.GroupId });

    modelBuilder.Entity<SubstitutionGroup>()
        .HasOne(sg => sg.Substitution)
        .WithMany(s => s.SubstitutionGroups)
        .HasForeignKey(sg => sg.SubstitutionId);

    modelBuilder.Entity<SubstitutionGroup>()
        .HasOne(sg => sg.Group)
        .WithMany(g => g.SubstitutionGroups)
        .HasForeignKey(sg => sg.GroupId);

    modelBuilder.Entity<SubstitutionNotification>()
        .HasOne(sn => sn.Substitution)
        .WithMany(s => s.SubstitutionNotifications)
        .HasForeignKey(sn => sn.SubstitutionId);

    modelBuilder.Entity<SubstitutionNotification>()
        .HasOne(sn => sn.User)
        .WithMany(u => u.SubstitutionNotifications)
        .HasForeignKey(sn => sn.UserId)
        .OnDelete(DeleteBehavior.NoAction);

    
    modelBuilder.Entity<SubstitutionTaken>()
        .HasOne(st => st.Substitution)
        .WithMany(s => s.SubstitutionTakens)
        .HasForeignKey(st => st.SubstitutionId);

    modelBuilder.Entity<SubstitutionTaken>()
        .HasOne(st => st.TakenByUser)
        .WithMany(u => u.SubstitutionTakens)
        .HasForeignKey(st => st.TakenByUserId)
        .OnDelete(DeleteBehavior.NoAction);

   
    modelBuilder.Entity<Overtime>()
        .HasOne(o => o.User)
        .WithMany(u => u.Overtimes)
        .HasForeignKey(o => o.UserId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Overtime>()
        .HasOne(o => o.Substitution)
        .WithMany(s => s.Overtimes)
        .HasForeignKey(o => o.SubstitutionId)
        .OnDelete(DeleteBehavior.Cascade);
}
    }
}