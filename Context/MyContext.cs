using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
namespace Context
{
    public  class MyContext : DbContext
    {

        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(r => r.Ingredients).HasColumnType("nvarchar(max)");
                entity.Property(r => r.Instructions).HasColumnType("nvarchar(max)");

                entity.HasOne(u => u.User)
                      .WithMany()
                      .HasForeignKey(u => u.IdUser);

                entity.HasOne(u => u.Category)
                      .WithMany()
                      .HasForeignKey(u => u.IdCategory);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(u => u.User)
                      .WithMany()
                      .HasForeignKey(u => u.IdUser);

                entity.HasOne(u => u.Recipe)
                      .WithMany()
                      .HasForeignKey(u => u.IdRecipe);
            });
               modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(u => u.User)
                      .WithMany()
                      .HasForeignKey(u => u.IdUser);

                entity.HasOne(u => u.Recipe)
                      .WithMany()
                      .HasForeignKey(u => u.IdRecipe);
            });
        }
    }
}
