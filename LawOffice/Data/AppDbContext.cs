using LawOffice.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LawOffice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Lawyer> Lawyers => Set<Lawyer>();
        public DbSet<Staff> Staffs => Set<Staff>();
        public DbSet<CaseType> CaseTypes { get; set; }

        public DbSet<Case> Cases => Set<Case>();
        public DbSet<CaseDocument> CaseDocuments => Set<CaseDocument>();
        public DbSet<CaseNote> CaseNotes => Set<CaseNote>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<CourtSession> CourtSessions => Set<CourtSession>();
        public DbSet<Financial> Financials => Set<Financial>();
        public DbSet<SmsLog> SmsLogs => Set<SmsLog>();
        public DbSet<CaseStatus> CaseStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Soft delete filter
            modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Client>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Lawyer>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Case>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<CaseDocument>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Appointment>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<CourtSession>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Financial>().HasQueryFilter(e => !e.IsDeleted);

            // Unique constraints
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Client>().HasIndex(c => c.NationalCode).IsUnique();
            modelBuilder.Entity<Case>().HasIndex(c => c.CaseCode).IsUnique();

            // Decimal precision
            modelBuilder.Entity<Financial>()
                .Property(f => f.Amount)
                .HasPrecision(18, 2);

            // ✅ Configure cascade delete behavior to prevent cycles
            modelBuilder.Entity<Case>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Cases)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Case>()
                .HasOne(c => c.Lawyer)
                .WithMany(l => l.Cases)
                .HasForeignKey(c => c.LawyerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>()
    .HasOne(u => u.Staff)
    .WithOne(s => s.User)
    .HasForeignKey<Staff>(s => s.UserId);


            modelBuilder.Entity<CaseDocument>()
                .HasOne(cd => cd.Case)
                .WithMany(c => c.Documents)
                .HasForeignKey(cd => cd.CaseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CaseNote>()
                .HasOne(cn => cn.Case)
                .WithMany(c => c.Notes)
                .HasForeignKey(cn => cn.CaseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CaseNote>()
                .HasOne(cn => cn.Author)
                .WithMany()
                .HasForeignKey(cn => cn.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Client)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Lawyer)
                .WithMany(l => l.Appointments)
                .HasForeignKey(a => a.LawyerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CourtSession>()
                .HasOne(cs => cs.Case)
                .WithMany(c => c.CourtSessions)
                .HasForeignKey(cs => cs.CaseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Financial>()
                .HasOne(f => f.Case)
                .WithMany(c => c.Financials)
                .HasForeignKey(f => f.CaseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Financial>()
                .HasOne(f => f.Client)
                .WithMany(c => c.Financials)
                .HasForeignKey(f => f.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed admin user
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "Mehrdad",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("zx7997op??"),
                FirstName = "مهرداد",
                LastName = "محجل",
                Role = UserRole.Lawyer,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 2,
                Username = "Elvin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("zx7997op??"),
                FirstName = "ائلوین",
                LastName = "محجل",
                Role = UserRole.Client,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 3,
                Username = "Samira",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("zx7997op??"),
                FirstName = "سمیرا",
                LastName = "نورمحمدی",
                Role = UserRole.Staff,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

            modelBuilder.Entity<CaseType>().HasData(
     new CaseType { Id = 1, Name = "Civil", PersianTitle = "حقوقی",Perfix="CVL" },
     new CaseType { Id = 2, Name = "Criminal", PersianTitle = "کیفری" ,Perfix="CRM"},
     new CaseType { Id = 3, Name = "Family", PersianTitle = "خانواده" , Perfix = "FAM" },
     new CaseType { Id = 4, Name = "Labor", PersianTitle = "کار", Perfix = "LBR" },
     new CaseType { Id = 5, Name = "Commercial", PersianTitle = "تجاری", Perfix = "COM" },
     new CaseType { Id = 6, Name = "Administrative", PersianTitle = "اداری", Perfix = "ADM" },
     new CaseType { Id = 7, Name = "Real_Estate", PersianTitle = "ملکی", Perfix = "RES" },
     new CaseType { Id = 8, Name = "Other", PersianTitle = "سایر", Perfix = "OTH" }
 );
            modelBuilder.Entity<CaseStatus>().HasData(
        new CaseStatus { Id = 1, Name = "Open", PersianTitle = "جاری", DisplayOrder = 1 },
        new CaseStatus { Id = 2, Name = "Closed", PersianTitle = "مختومه", DisplayOrder = 2 },
        new CaseStatus { Id = 3, Name = "Pending", PersianTitle = "معلق", DisplayOrder = 3 }
    );
        }
    }
}
