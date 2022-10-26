using System;
using EnterpriseApp.API.Models;
using EnterpriseApp.API.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseApp.API.Data
{
    public class EnterpriseAppContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<DeletedUser> DeletedUsers { get; set; }

        public DbSet<PortalUser> PortalUsers { get; set; }

        public DbSet<ResComment> ResComments { get; set; }

        public DbSet<ResKeySeq> ResKeys { get; set; }

        public DbSet<ResBadgeEntity> ResBadge { get; set; }

        public DbSet<UserRestaurantRequest> ResRequests { get; set; }

        public DbSet<UserResList> UserResLists { get; set; }

        public DbSet<UserType> UserTypes { get; set; }

        public DbSet<UserUType> UserUTypes { get; set; }

        public DbSet<UserVisitedRestaurant> UserVisitedRestaurant { get; set; }

        public DbSet<UserVisitedCountries> UserVisitedCountries { get; set; }

        public DbSet<UserFavouriteRestaurant> UserFavouriteRestaurant { get; set; }

        public DbSet<ResFileUpload> FileUploads { get; set; }

        public DbSet<ResFilterIcon> ResIcons { get; set; }

        public DbSet<ResOpeningHour> ResOpeningHours { get; set; }

        public DbSet<UserFileUpload> UserFileUploads { get; set; }

        public DbSet<UserReward> UserRewards { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<UserRestaurantChoice> UserRestaurantChoices { get; set; }

        public DbSet<ResListRestaurant> ResListRestaurants { get; set; }

        public DbSet<ResCategory> ResCategories { get; set; }

        public DbSet<SubcategoryEntity> SubCategorires { get; set; }

        public DbSet<ResSubcategoryEntity> ResSubCategories { get; set; }

        public DbSet<BadgeEntity> Badge { get; set; }

        private readonly string _connectionString;

        public EnterpriseAppContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public EnterpriseAppContext(DbContextOptions<EnterpriseAppContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseMySQL(_connectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id);
                entity.HasIndex(u => u.UEmail).IsUnique();
                entity.Property(e => e.UPassword).IsRequired(false);
                entity.Property(e => e.UName);
                entity.Property(e => e.UFirstName);
                entity.Property(e => e.USex);
                entity.Property(e => e.DOB);
                entity.Property(e => e.CountryId).IsRequired();
                entity.HasOne(u => u.Country).WithMany()
                 .HasForeignKey(u => u.CountryId)
                 .IsRequired(false);

                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<DeletedUser>(entity =>
            {
                entity.ToTable("DeletedUser");
                entity.HasKey(e => e.Id);
                entity.HasIndex(u => u.UEmail).IsUnique();
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<PortalUser>(entity =>
            {
                entity.ToTable("PortalUser");
                entity.HasKey(e => e.Id);
                entity.HasIndex(u => u.UserName).IsUnique();
                entity.Property(e => e.Password).IsRequired(false);
                entity.Property(e => e.FirstName);
                entity.Property(e => e.LastName);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("Restaurant");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RName).IsRequired();
                entity.Property(e => e.RStreet).IsRequired(false);
                entity.Property(e => e.RCity).IsRequired();
                entity.Property(e => e.RDescription).IsRequired(false);
                entity.Property(e => e.RLongitude).IsRequired();
                entity.Property(e => e.RLatitude).IsRequired();
                entity.Property(e => e.RPostalCode).IsRequired(false);
                entity.Property(e => e.RContact).IsRequired();
                entity.Property(e => e.CountryId).IsRequired();
                entity.Property(e => e.CategoryId).IsRequired();
                entity.Property(e => e.Session1).HasConversion<int>();
                entity.Property(e => e.Session2).HasConversion<int>();

                entity.HasOne(u => u.Country).WithMany()
                .HasForeignKey(u => u.CountryId)
                .IsRequired(true);
                entity.Property(e => e.CategoryId).IsRequired();
                entity.HasOne(u => u.ResCategory).WithMany()
                .HasForeignKey(u => u.CategoryId)
                .IsRequired(true);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
                //entity.Property(e=>e.UserFavouriteRestaurant).
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired();
                entity.Property(e => e.SortOrder).HasDefaultValue(-1);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResComment>(entity =>
            {
                entity.ToTable("ResComment");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RCDate).HasDefaultValue(DateTimeOffset.Now);
                entity.Property(e => e.RComment).IsRequired();
                entity.Property(e => e.RestaurantId).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.HasOne(d => d.Restaurant)
                  .WithMany(p => p.Comments)
                  .HasForeignKey(x => x.RestaurantId)
                  .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.User)
                 .WithMany(p => p.CommentsList)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResListRestaurant>(entity =>
            {
                entity.ToTable("ResListRestaurant");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ListId).IsRequired();
                entity.Property(e => e.RestaurantId).IsRequired();
                entity.HasOne(d => d.List)
                 .WithMany(p => p.ResListRestaurants)
                 .HasForeignKey(x => x.ListId)
                 .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Restaurant)
                .WithMany(p => p.ResListItems)
                .HasForeignKey(x => x.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResFileUpload>(entity =>
            {
                entity.ToTable("ResFileUpload");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.HasOne(d => d.Restaurant)
                 .WithMany(p => p.Images)
                 .HasForeignKey(x => x.RestaurantId)
                 .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResFilterIcon>(entity =>
            {
                entity.ToTable("ResFilterIcon");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RIconID).IsRequired();
                entity.Property(e => e.FilterName).IsRequired(false);
                entity.Property(e => e.RestaurantId).IsRequired();
                entity.HasOne(d => d.Restaurant)
                 .WithMany(p => p.Icons)
                 .HasForeignKey(x => x.RestaurantId)
                 .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResCategory>(entity =>
            {
                entity.ToTable("ResCategory");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResOpeningHour>(entity =>
            {
                entity.ToTable("ResOpeningHour");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Session).IsRequired();
                entity.Property(e => e.Day).IsRequired();
                entity.Property(e => e.Opens).IsRequired();
                entity.Property(e => e.Closes).IsRequired();

                entity.HasOne(d => d.Restaurant)
                 .WithMany(p => p.OpeningHours)
                 .HasForeignKey(x => x.RestaurantId)
                 .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<UserFileUpload>(entity =>
            {
                entity.ToTable("UserFileUpload");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.HasOne(d => d.User)
                 .WithMany(p => p.Images)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResKeySeq>(entity =>
            {
                entity.ToTable("ResKeySeq");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ResKey).IsRequired();
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<UserRestaurantRequest>(entity =>
            {
                entity.ToTable("UserRestaurantRequest");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RName).IsRequired();
                entity.Property(e => e.RStreet).IsRequired();
                entity.Property(e => e.RCity).IsRequired();
                entity.HasOne(u => u.Country).WithMany()
                .HasForeignKey(u => u.CountryId)
                .IsRequired(true);
                entity.HasOne(d => d.User)
                .WithMany(p => p.ResRequestList)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<UserResList>(entity =>
            {
                entity.ToTable("UserResList");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ListName).IsRequired();
                entity.Property(e => e.IconId);
                entity.Property(e => e.LColour);
                entity.HasOne(d => d.User)
                .WithMany(p => p.ResList)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<UserVisitedRestaurant>(entity =>
            {
                entity.ToTable("UserVisitedRestaurant");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.RestaurantId).IsRequired();

                entity.Property(e => e.RestaurantId).IsRequired();
                entity.HasOne(d => d.User)
                .WithMany(p => p.UserVisitedResturants)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<UserVisitedCountries>(entity =>
            {
                entity.ToTable("UserVisitedCountries");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CountryId).IsRequired();
                entity.Property(e => e.UserId).IsRequired();

                entity.Property(e => e.CountryId).IsRequired();
                entity.HasOne(d => d.User)
                .WithMany(p => p.UserVisitedCountries)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IconId).IsRequired(false);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<UserUType>(entity =>
            {
                entity.ToTable("UserUType");
                entity.HasKey(bc => new { bc.UserId, bc.UTypeId });
                entity.HasOne(bc => bc.User)
                .WithMany(b => b.UserTypes)
                .HasForeignKey(bc => bc.UserId);
                entity.HasOne(bc => bc.UType)
                .WithMany(c => c.UserTypes)
                .HasForeignKey(bc => bc.UTypeId);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<UserFavouriteRestaurant>(entity =>
            {
                entity.ToTable("UserFavouriteRestaurant");
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Restaurant)
                  .WithMany(p => p.UserFavouriteRestaurantList)
                  .HasForeignKey(x => x.RestaurantId);
                entity.HasOne(d => d.User)
                 .WithMany(p => p.UserFavouriteRestaurantList)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<UserRestaurantChoice>(entity =>
            {
                entity.ToTable("UserRestaurantChoice");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MyChoice).IsRequired();
                entity.HasOne(d => d.Restaurant)
                  .WithMany(p => p.ChoiceRestaurantList);
                entity.HasOne(d => d.User)
                 .WithMany(p => p.ChoiceRestaurantList)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<UserReward>(entity =>
            {
                entity.ToTable("UserReward");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IconId).HasDefaultValue(0);
                entity.Property(e => e.UserId).IsRequired();
                entity.HasOne(d => d.User)
                 .WithMany(p => p.Rewards)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<SubcategoryEntity>(entity =>
            {
                entity.ToTable("Subcategory");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResSubcategoryEntity>(entity =>
            {
                entity.ToTable("ResSubcategory");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResSubcategoryEntity>()
                .HasOne(sc => sc.Restaurant)
                .WithMany(s => s.ResSubcategoryEntityList)
                .HasForeignKey(sc => sc.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResSubcategoryEntity>()
                .HasOne(sc => sc.Subcategory)
                .WithMany(s => s.ResSubcategoryEntityList)
                .HasForeignKey(sc => sc.SubcategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BadgeEntity>(entity =>
            {
                entity.ToTable("Badge");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IconId).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResBadgeEntity>(entity =>
            {
                entity.ToTable("ResBadge");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedOn).HasDefaultValue(DateTimeOffset.Now);
            });

            modelBuilder.Entity<ResBadgeEntity>()
              .HasOne(sc => sc.Restaurant)
              .WithMany(s => s.ResBadgeEntity)
              .HasForeignKey(sc => sc.RestaurantId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResBadgeEntity>()
                .HasOne(sc => sc.Badge)
                .WithMany(s => s.ResBadgeEntity)
                .HasForeignKey(sc => sc.BadgeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}