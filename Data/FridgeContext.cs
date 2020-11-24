using Fridge_BackEnd.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fridge_BackEnd.Data
{
    public class FridgeContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public FridgeContext(DbContextOptions<FridgeContext> options) : base(options)
        { }

        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<FridgeUser> FridgeUsers { get; set; }
        public DbSet<FridgeIngredient> FridgeIngredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientCategory> IngredientCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .ToTable("AppUsers");

            modelBuilder.Entity<AppUser>()
                .HasMany(a => a.Fridges)
                .WithOne(f => f.AppUser)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<AppUser>()
                .HasMany(a => a.Recipes)
                .WithOne(a => a.AppUser)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<IdentityRole<int>>(i =>
            {
                i.ToTable("Roles");
                i.HasKey(x => x.Id);
            });

            modelBuilder.Entity<IdentityUserRole<int>>(i =>
            {
                i.ToTable("UserRole");
                i.HasKey(x => new { x.RoleId, x.UserId });
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(i =>
            {
                i.ToTable("UserLogin");
                i.HasKey(x => new { x.ProviderKey, x.LoginProvider });
            });

            modelBuilder.Entity<IdentityRoleClaim<int>>(i =>
            {
                i.ToTable("RoleClaims");
                i.HasKey(x => x.Id);
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(i =>
            {
                i.ToTable("UserClaims");
                i.HasKey(x => x.Id);
            });

            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

        }
    }
}
