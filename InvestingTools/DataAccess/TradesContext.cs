using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess
{
    public partial class TradesContext : DbContext
    {
        public TradesContext()
        {
        }

        public TradesContext(DbContextOptions<TradesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ExtendedHoursBiggestMovers> ExtendedHoursBiggestMovers { get; set; }
        public virtual DbSet<Recommendation> Recommendation { get; set; }
        public virtual DbSet<RecommendationActionType> RecommendationActionType { get; set; }
        public virtual DbSet<Source> Source { get; set; }
        public virtual DbSet<Symbol> Symbol { get; set; }
        public virtual DbSet<SymbolNews> SymbolNews { get; set; }
        public virtual DbSet<SymbolSource> SymbolSource { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExtendedHoursBiggestMovers>(entity =>
            {
                entity.Property(e => e.CreatedDateTimeUtc).HasColumnType("datetime");

                entity.Property(e => e.MarketDate).HasColumnType("date");

                entity.Property(e => e.PriceAfterHours).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PriceClose).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PriceOpen).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Symbol)
                    .WithMany(p => p.ExtendedHoursBiggestMovers)
                    .HasForeignKey(d => d.SymbolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExtendedHoursBiggestMovers_Symbol");
            });

            modelBuilder.Entity<Recommendation>(entity =>
            {
                entity.Property(e => e.CurrentPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EventDateTime).HasColumnType("datetime");

                entity.Property(e => e.Ticker)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecommendationActionType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Source>(entity =>
            {
                entity.Property(e => e.ApiKey)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Symbol>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol1)
                    .IsRequired()
                    .HasColumnName("Symbol")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SymbolNews>(entity =>
            {
                entity.Property(e => e.Author)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.PublishedDateTime).HasColumnType("datetime");

                entity.Property(e => e.PublisherName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PublisherUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RelevantSymbolsCsv)
                    .HasColumnName("RelevantSymbolsCSV")
                    .IsUnicode(false);

                entity.Property(e => e.Summary).IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Symbol)
                    .WithMany(p => p.InverseSymbol)
                    .HasForeignKey(d => d.SymbolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SymbolNews_SymbolNews");
            });

            modelBuilder.Entity<SymbolSource>(entity =>
            {
                entity.ToTable("Symbol_Source");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.SymbolSource)
                    .HasForeignKey(d => d.SourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Symbol_Source_Source");

                entity.HasOne(d => d.Symbol)
                    .WithMany(p => p.SymbolSource)
                    .HasForeignKey(d => d.SymbolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Symbol_Source_Symbol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
