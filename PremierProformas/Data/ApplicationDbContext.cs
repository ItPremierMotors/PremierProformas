using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PremierProformas.Models;

namespace PremierProformas.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Marca> Marcas => Set<Marca>();
    public DbSet<Sucursal> Sucursales => Set<Sucursal>();
    public DbSet<Vendedor> Vendedores => Set<Vendedor>();
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Proforma> Proformas => Set<Proforma>();
    public DbSet<CorrelativoProforma> CorrelativosProforma => Set<CorrelativoProforma>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>().ToTable("asp_net_roles");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("asp_net_role_claims");
        builder.Entity<IdentityUser>().ToTable("asp_net_users");
        builder.Entity<IdentityUserClaim<string>>().ToTable("asp_net_user_claims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("asp_net_user_logins");
        builder.Entity<IdentityUserRole<string>>().ToTable("asp_net_user_roles");
        builder.Entity<IdentityUserToken<string>>().ToTable("asp_net_user_tokens");

        // ---------- Marca ----------
        builder.Entity<Marca>(e =>
        {
            e.Property(x => x.Id).HasIdentityOptions(startValue: 6);
            e.Property(x => x.Nombre).HasMaxLength(100);
            e.Property(x => x.Prefijo).HasMaxLength(10);
            e.HasIndex(x => x.Nombre).IsUnique();
        });

        var fechaSeed = new DateTime(2026, 10, 14, 0, 0, 0, DateTimeKind.Utc);
        builder.Entity<Marca>().HasData(
            new { Id = 1, Nombre = "CHANGAN", Prefijo = "CH", LogoArchivo = "changan.png", Activo = true, CreadoEn = fechaSeed },
            new { Id = 2, Nombre = "GREAT WALL", Prefijo = "GR", LogoArchivo = "greatwall.png", Activo = true, CreadoEn = fechaSeed },
            new { Id = 3, Nombre = "HAVAL", Prefijo = "HA", LogoArchivo = "haval.png", Activo = true, CreadoEn = fechaSeed },
            new { Id = 4, Nombre = "ZXAUTO", Prefijo = "ZX", LogoArchivo = "zxauto.png", Activo = true, CreadoEn = fechaSeed },
            new { Id = 5, Nombre = "DEEPAL", Prefijo = "DE", LogoArchivo = "deepal.png", Activo = true, CreadoEn = fechaSeed });

        // ---------- Sucursal ----------
        builder.Entity<Sucursal>(e =>
        {
            e.Property(x => x.Id).HasIdentityOptions(startValue: 3);
            e.Property(x => x.Codigo).HasMaxLength(10);
            e.Property(x => x.Nombre).HasMaxLength(100);
            e.HasIndex(x => x.Codigo).IsUnique();

            // Sucursal 1 -> N Vendedores (usa Vendedor.SucursalId)
            e.HasMany(x => x.Vendedores)
             .WithOne(v => v.Sucursal)
             .HasForeignKey(v => v.SucursalId)
             .OnDelete(DeleteBehavior.Restrict);

            // Sucursal -> Jefe de ventas (relación aparte, opcional, sin navegación inversa)
            e.HasOne(x => x.JefeVentas)
             .WithMany()
             .HasForeignKey(x => x.JefeVentasId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<Sucursal>().HasData(
            new { Id = 1, Codigo = "SPS", Nombre = "San Pedro Sula", Direccion = "Col. Las Mesetas, 21 calle, 17 ave. Dos cuadras arriba de CEMESA", Telefono = "2556-5019", Activo = true, CreadoEn = fechaSeed },
            new { Id = 2, Codigo = "TGU", Nombre = "Tegucigalpa", Direccion = "", Telefono = "", Activo = true, CreadoEn = fechaSeed });

        // ---------- Vendedor ----------
        builder.Entity<Vendedor>(e =>
        {
            e.Property(x => x.NombreCompleto).HasMaxLength(150);
            e.HasIndex(x => x.UsuarioId).IsUnique();
            e.HasOne<IdentityUser>()
             .WithMany()
             .HasForeignKey(x => x.UsuarioId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ---------- Vehiculo ----------
        builder.Entity<Vehiculo>(e =>
        {
            e.Property(x => x.CodigoInventario).HasMaxLength(50);
            e.Property(x => x.Vin).HasMaxLength(50);
            e.Property(x => x.Condicion).HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.Estatus).HasConversion<string>().HasMaxLength(20);
            e.HasIndex(x => x.Vin).IsUnique();
            e.HasIndex(x => x.CodigoInventario).IsUnique();

            e.HasOne(x => x.Marca)
             .WithMany(m => m.Vehiculos)
             .HasForeignKey(x => x.MarcaId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Cliente ----------
        builder.Entity<Cliente>(e =>
        {
            e.Property(x => x.NombreCompleto).HasMaxLength(150);
            e.Property(x => x.Dni).HasMaxLength(20);
            e.Property(x => x.Rtn).HasMaxLength(20);
            e.HasIndex(x => x.Dni).IsUnique();
        });

        // ---------- Proforma ----------
        builder.Entity<Proforma>(e =>
        {
            e.Property(x => x.Numero).HasMaxLength(30);
            e.Property(x => x.Estado).HasConversion<string>().HasMaxLength(20);
            e.HasIndex(x => x.Numero).IsUnique();

            foreach (var prop in new[] {
                nameof(Proforma.PrecioUsd),
                nameof(Proforma.PrecioConIsv), nameof(Proforma.PrecioSinIsv),
                nameof(Proforma.Isv), nameof(Proforma.PrecioVenta),
                nameof(Proforma.Prima), nameof(Proforma.SaldoFinanciar) })
            {
                e.Property(prop).HasPrecision(14, 2);
            }
            e.Property(x => x.TasaCambio).HasPrecision(10, 4);

            e.HasOne(x => x.Sucursal).WithMany().HasForeignKey(x => x.SucursalId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Cliente).WithMany().HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Vehiculo).WithMany().HasForeignKey(x => x.VehiculoId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Vendedor).WithMany().HasForeignKey(x => x.VendedorId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- CorrelativoProforma ----------
        builder.Entity<CorrelativoProforma>(e =>
        {
            e.HasKey(x => new { x.SucursalId, x.Anio });
            e.HasOne<Sucursal>()
             .WithMany()
             .HasForeignKey(x => x.SucursalId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
