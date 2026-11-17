using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PrijavaIspita.Models;

public partial class PrijavaIspitaContext : DbContext
{
    public PrijavaIspitaContext()
    {
    }

    public PrijavaIspitaContext(DbContextOptions<PrijavaIspitaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Budzet> Budzets { get; set; }
    public virtual DbSet<Predmet> Predmet { get; set; }
    public virtual DbSet<Prijava> Prijavas { get; set; }
    public virtual DbSet<Smer> Smer { get; set; }
    public DbSet<Korisnik> Korisnici { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<Transakcija> Transakcijas { get; set; }
    public DbSet<Obavestenje> Obavestenja { get; set; }
    public DbSet<ZahtevBudzet> ZahtevBudzet { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, move it out of source code.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-R46SRUE\\SQLEXPRESS;Database=PrijavaIspita;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Budzet>(entity =>
        {
            entity.HasKey(e => e.BudzetId);
            entity.ToTable("Budzet");
            entity.Property(e => e.BudzetId).HasColumnName("BudzetID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Na cekanju");
            entity.Property(e => e.KorisnikID).HasColumnName("KorisnikID");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Budzets)
                .HasForeignKey(d => d.KorisnikID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Budzet__KorisnikID");
        });

        modelBuilder.Entity<Prijava>(entity =>
        {
            entity.HasKey(e => new { e.PredmetId, e.KorisnikID });
            entity.ToTable("Prijava");
            entity.Property(e => e.PredmetId).HasColumnName("PredmetID");
            entity.Property(e => e.KorisnikID).HasColumnName("KorisnikID");

            entity.HasOne(d => d.Predmet).WithMany(p => p.Prijavas)
                .HasForeignKey(d => d.PredmetId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Prijavas)
                .HasForeignKey(d => d.KorisnikID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Transakcija>(entity =>
        {
            entity.HasKey(e => e.TransakcijaId);
            entity.ToTable("Transakcija");
            entity.Property(e => e.TransakcijaId).HasColumnName("TransakcijaID");
            entity.Property(e => e.Datum)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");
            entity.Property(e => e.KorisnikId).HasColumnName("KorisnikID");
            entity.Property(e => e.Tip).HasMaxLength(50);

            entity.HasOne(d => d.Korisnik)
                  .WithMany(p => p.Transakcijas)
                  .HasForeignKey(d => d.KorisnikId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Obavestenje>(entity =>
        {
            entity.HasKey(e => e.ObavestenjeId);
            entity.ToTable("Obavestenja");
            entity.Property(e => e.ObavestenjeId).HasColumnName("Id");
            entity.Property(e => e.Tekst).HasMaxLength(500);
            entity.Property(e => e.Datum)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");
            entity.Property(e => e.KorisnikID).HasColumnName("KorisnikID");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Obavestenja)
                .HasForeignKey(d => d.KorisnikID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        // Role 

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");
            entity.HasKey(r => r.RoleID);
            entity.Property(r => r.RoleName).HasMaxLength(50);
        });


        // UserRole 
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleID);
            entity.ToTable("UserRole");
            entity.Property(e => e.UserRoleID).HasColumnName("UserRoleID");
            entity.Property(e => e.KorisnikID).HasColumnName("KorisnikID");
            entity.Property(e => e.RoleID).HasColumnName("RoleID");

            entity.HasOne(d => d.Korisnik)
                  .WithMany(p => p.UserRoles)
                  .HasForeignKey(d => d.KorisnikID)
                  .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Role)
                  .WithMany(p => p.UserRoles)
                  .HasForeignKey(d => d.RoleID)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
