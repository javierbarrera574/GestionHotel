using Microsoft.EntityFrameworkCore;
using Reservas.BData.Data.Entity;
using System.Reflection.Emit;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Reservas.BData
{
	public class Context : DbContext
	{
		public Context() { }
		public Context(DbContextOptions<Context> options) : base(options) { }
		public DbSet<Huesped> Huespedes => Set<Huesped>();
		public DbSet<Habitacion> Habitaciones => Set<Habitacion>();
		public DbSet<Reserva> Reservas => Set<Reserva>();
       
        public DbSet<Persona> Personas => Set<Persona>();
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{ }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Habitacion>(o =>
			{
				o.HasKey(b => b.Id);
				o.Property(b => b.Nhab);
				o.Property(b => b.Camas);
				o.Property(b => b.Estado);
			
			});

			modelBuilder.Entity<Persona>(o =>
			{
				o.HasKey(b => b.Id);
				o.Property(b => b.Dni);
				o.Property(b => b.Nombres);
				o.Property(b => b.Apellidos);
				o.Property(b => b.Correo);
				o.Property(b => b.Telefono);
				o.Property(b => b.NumTarjeta);
				o.Property(b => b.NumHab);
			});
			modelBuilder.Entity<Huesped>(o =>
			{
				o.HasKey(b => b.Id);
				o.Property(b => b.Dni);
				o.Property(b => b.Nombres);
				o.Property(b => b.Apellidos);
				o.Property(b => b.Checkin);
				o.Property(b => b.DniPersona);
			});
            modelBuilder.Entity<Reserva>(o =>
            {
                o.HasKey(b => b.Id);
                o.Property(b => b.NroReserva);
                o.Property(b => b.Fecha_inicio);
                o.Property(b => b.Fecha_fin);
                o.Property(b => b.Dni);
				o.Property(b => b.nhabs);
                o.Property(b => b.DniHuesped)
				.HasConversion(
				v => string.Join(",", v),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList())
				.IsRequired()
				.Metadata.SetValueComparer(new ValueComparer<List<int>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList()));
            });
        }
    }
}
