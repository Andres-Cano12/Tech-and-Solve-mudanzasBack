using AccessData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessData.Context
{
    public class DBContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public DbSet<Move> Moves { get; set; }
        public DbSet<MoveDetail> MoveDetails { get; set; }

        public DBContext(DbContextOptions<DBContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Move>(entity =>
            //{
            //    // Table
            //    entity.ToTable("Move");

            //    // Columns
            //    entity.Property(u => u.IdMove).HasColumnName("IdMove");
            //    entity.Property(u => u.IdentificationCard).HasColumnName("IdentificationCard");
            //    entity.Property(u => u.DateMove).HasColumnName("DateMove");
         

            //});


            //modelBuilder.Entity<MoveDetail>(entity =>
            //{
            //    // Table
            //    entity.ToTable("MoveDetail");

            //    // Columns
            //    entity.Property(u => u.IdMove).HasColumnName("IdMove");
            //    entity.Property(u => u.IdMoveDetail).HasColumnName("IdMoveDetail");
            //    entity.Property(u => u.Value).HasColumnName("Value");
            
            //});

            modelBuilder.Entity<Move>()
            .HasMany(b => b.MoveDetail)
            .WithOne();

            modelBuilder.Entity<MoveDetail>()
            .HasOne(p => p.Move)
            .WithMany(b => b.MoveDetail)
            .HasForeignKey(p => p.IdMove);
        }
    }
}
