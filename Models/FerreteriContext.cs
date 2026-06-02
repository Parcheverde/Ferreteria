using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Ferreteri.Models;

public partial class FerreteriContext : DbContext
{
    public FerreteriContext()
    {
    }

    public FerreteriContext(DbContextOptions<FerreteriContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categorias");

            entity.Property(e => e.IdCategoria)
                .HasColumnType("int(11)")
                .HasColumnName("idCategoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.HasKey(e => e.IdMov).HasName("PRIMARY");

            entity.ToTable("movimientos");

            entity.HasIndex(e => e.FkIdProd, "FK_Productos_Movimientos");

            entity.Property(e => e.IdMov)
                .HasColumnType("int(11)")
                .HasColumnName("idMov");
            entity.Property(e => e.Cantidad)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.FkIdProd)
                .HasColumnType("int(11)")
                .HasColumnName("FK_idProd");
            entity.Property(e => e.TipoMov)
                .HasMaxLength(100)
                .HasColumnName("tipoMov");

            entity.HasOne(d => d.FkIdProdNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.FkIdProd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Movimientos");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProd).HasName("PRIMARY");

            entity.ToTable("productos");

            entity.HasIndex(e => e.FkIdCategoria, "FK_Productos_Categorias");

            entity.Property(e => e.IdProd)
                .HasColumnType("int(11)")
                .HasColumnName("idProd");
            entity.Property(e => e.FkIdCategoria)
                .HasColumnType("int(11)")
                .HasColumnName("FK_idCategoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(75)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
            entity.Property(e => e.Stock)
                .HasColumnType("int(11)")
                .HasColumnName("stock");

            entity.HasOne(d => d.FkIdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.FkIdCategoria)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Productos_Categorias");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
