namespace PETNET.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PetnetDB : DbContext
    {
        public PetnetDB()
            : base("name=PetnetDB")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
      
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .Property(e => e.Date)
                .IsFixedLength();

            modelBuilder.Entity<Blog>()
                .Property(e => e.ReadingCount)
                .IsFixedLength();

            modelBuilder.Entity<Blog>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Blogs)
                .Map(m => m.ToTable("BlogsTags").MapLeftKey("BlogID").MapRightKey("TagID"));

            modelBuilder.Entity<Category>()
                .Property(e => e.CategorieName)
                .IsFixedLength();

            modelBuilder.Entity<Comment>()
                .Property(e => e.Date)
                .IsFixedLength();

            modelBuilder.Entity<Role>()
                .Property(e => e.Role1)
                .IsFixedLength();

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tag>()
                .Property(e => e.TagName)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Animals)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
