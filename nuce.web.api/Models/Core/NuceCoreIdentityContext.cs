using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Models.Core
{
    public partial class NuceCoreIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public NuceCoreIdentityContext(DbContextOptions<NuceCoreIdentityContext> options) : base(options)
        {
        }

        public virtual DbSet<ActiviyLogs> ActiviyLogs { get; set; }
        public virtual DbSet<ManagerBackup> ManagerBackup { get; set; }
        public virtual DbSet<NewsCatItem> NewsCatItem { get; set; }
        public virtual DbSet<NewsCats> NewsCats { get; set; }
        public virtual DbSet<NewsItems> NewsItems { get; set; }
        public virtual DbSet<FileUpload> FileUpload { get; set; }
        public virtual DbSet<ClientParameters> ClientParameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ActiviyLogs>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.RoleId).HasMaxLength(400);

                entity.Property(e => e.UserCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ManagerBackup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DatabaseName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<NewsCatItem>(entity =>
            {
                entity.ToTable("News_Cat_Item");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.CatName).HasMaxLength(200);

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.ItemTitle).HasMaxLength(500);
            });

            modelBuilder.Entity<NewsCats>(entity =>
            {
                entity.ToTable("News_Cats");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.MenuHref)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Role).HasMaxLength(200);
            });

            modelBuilder.Entity<NewsItems>(entity =>
            {
                entity.ToTable("News_Items");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Avatar).HasMaxLength(200);

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.EntryDatetime).HasColumnType("datetime");

                entity.Property(e => e.EntryUsername)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.File).HasMaxLength(200);

                entity.Property(e => e.MetaDesciption).HasMaxLength(500);

                entity.Property(e => e.MetaKeyword).HasMaxLength(500);

                entity.Property(e => e.NewSource).HasMaxLength(200);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdateDatetime).HasColumnType("datetime");

                entity.Property(e => e.UpdateUsername)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ClientParameters>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EntryDatetime).HasColumnType("datetime");

                entity.Property(e => e.EntryUsername).HasMaxLength(250);

                entity.Property(e => e.Role)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDatetime).HasColumnType("datetime");

                entity.Property(e => e.UpdateUsername).HasMaxLength(250);
            });

            modelBuilder.Entity<FileUpload>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EntryDatetime).HasColumnType("datetime");

                entity.Property(e => e.EntryUsername).HasMaxLength(250);

                entity.Property(e => e.FileType)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDatetime).HasColumnType("datetime");

                entity.Property(e => e.UpdateUsername).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
