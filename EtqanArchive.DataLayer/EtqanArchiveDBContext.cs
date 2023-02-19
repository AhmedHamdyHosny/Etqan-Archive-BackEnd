using DataLayer.Security.TableEntity;
using EtqanArchive.DataLayer.TableEntity;
using EtqanArchive.DataLayer.ViewEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using Tazkara.DataLayer.ViewEntity;

namespace EtqanArchive.DataLayer
{
    public class EtqanArchiveDBContext : IdentityDbContext<User, Role, Guid, UserClaim,
        UserRole, UserLogin, RoleClaim, UserToken>
    {
        public EtqanArchiveDBContext()
        {

        }

        public EtqanArchiveDBContext(DbContextOptions<EtqanArchiveDBContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        //for test
        //protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        //options.UseSqlServer(@"Server=.;Database=EtqanArchive;User Id=sa;Password=Sh@ms2018;");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetEntityIndex(ref modelBuilder);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);

            SetSecurityEntity(ref modelBuilder);
            SetViewEntity(ref modelBuilder);
        }

        private void SetEntityIndex(ref ModelBuilder modelBuilder)
        {
        }

        private void SetSecurityEntity(ref ModelBuilder modelBuilder)
        {
            #region Security Tables
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "User");
                entity.Property(x => x.Id).HasColumnName("UserId");
                entity.Property(x => x.PhoneNumberConfirmed).HasDefaultValue(false);
                entity.Property(x => x.TwoFactorEnabled).HasDefaultValue(false);
                entity.Property(x => x.LockoutEnabled).HasDefaultValue(false);
                entity.Property(x => x.AccessFailedCount).HasDefaultValue(0);
                entity.Property(x => x.EmailConfirmed).HasDefaultValue(false);

                //entity.Ignore(x => x.NormalizedUserName);
                //entity.Ignore(x => x.NormalizedEmail);

                entity.HasMany(g => g.UserClaims)
                .WithOne(s => s.User).HasForeignKey(s => s.UserId);

                entity.HasMany(g => g.UserLogins)
                .WithOne(s => s.User).HasForeignKey(s => s.UserId);

                entity.HasMany(g => g.UserTokens)
                .WithOne(s => s.User).HasForeignKey(s => s.UserId);

                entity.HasMany(g => g.UserRoles)
                .WithOne(s => s.User).HasForeignKey(s => s.UserId);


            });


            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Role");
                entity.Property(x => x.Id).HasColumnName("RoleId");
                //entity.Ignore(x => x.Name);
                //entity.Ignore(x => x.NormalizedName);
                entity.Ignore(x => x.ConcurrencyStamp);

                entity.HasMany(g => g.RoleClaims)
                .WithOne(s => s.Role).HasForeignKey(s => s.RoleId);

                entity.HasMany(g => g.UserRoles)
               .WithOne(s => s.Role).HasForeignKey(s => s.RoleId);
            });

            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaim");
                entity.Property(e => e.Id).HasColumnName("RoleClaimId");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable(name: "UserRole");
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId).IsRequired();

                entity.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId).IsRequired();
            });

            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.ToTable(name: "UserClaim");
                entity.Property(x => x.Id).HasColumnName("UserClaimId");
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.ToTable(name: "UserLogin");
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.ToTable(name: "UserToken");
            });


            #endregion
        }


        private void SetViewEntity(ref ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserView>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.ToView("UserView");
            });
            modelBuilder.Entity<ProjectFileView>(entity =>
            {
                entity.HasKey(e => e.ProjectFileId);
                entity.ToView("ProjectFileView");
            });
        }

        #region Tables
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ContentType> ContentTypes { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<FileExtension> FileExtensions { get; set; }
        public virtual DbSet<ProjectFile> ProjectFiles { get; set; }

        #endregion

        #region Views
        public virtual DbSet<UserView> UserView { get; set; }
        public virtual DbSet<ProjectFileView> ProjectFileView { get; set; }
        #endregion
    }
}
