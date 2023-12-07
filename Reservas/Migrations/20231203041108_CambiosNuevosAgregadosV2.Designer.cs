﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reservas.BData;

#nullable disable

namespace Reservas.BData.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20231203041108_CambiosNuevosAgregadosV2")]
    partial class CambiosNuevosAgregadosV2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Reservas.BData.Data.Entity.Habitacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Camas")
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Nhab")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Habitaciones");
                });

            modelBuilder.Entity("Reservas.BData.Data.Entity.Huesped", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("Checkin")
                        .HasColumnType("bit");

                    b.Property<int>("Dni")
                        .HasColumnType("int");

                    b.Property<int>("DniPersona")
                        .HasColumnType("int");

                    b.Property<int?>("HabitacionId")
                        .HasColumnType("int");

                    b.Property<int>("HabitacionNumero")
                        .HasColumnType("int");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("PersonaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HabitacionId")
                        .IsUnique()
                        .HasFilter("[HabitacionId] IS NOT NULL");

                    b.HasIndex("PersonaId")
                        .IsUnique()
                        .HasFilter("[PersonaId] IS NOT NULL");

                    b.ToTable("Huespedes");
                });

            modelBuilder.Entity("Reservas.BData.Data.Entity.Persona", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("nvarchar(55)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Dni")
                        .HasColumnType("int");

                    b.Property<bool>("EsHuespedyReservante")
                        .HasColumnType("bit");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("nvarchar(55)");

                    b.Property<int>("NumHab")
                        .HasColumnType("int");

                    b.Property<string>("NumTarjeta")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Personas");
                });

            modelBuilder.Entity("Reservas.BData.Data.Entity.Reserva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Dni")
                        .HasColumnType("int");

                    b.Property<string>("DniHuesped")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fecha_fin")
                        .IsRequired()
                        .HasColumnType("char(10)");

                    b.Property<string>("Fecha_inicio")
                        .IsRequired()
                        .HasColumnType("char(10)");

                    b.Property<int>("NroReserva")
                        .HasColumnType("int");

                    b.Property<int>("nhabs")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Reservas");
                });

            modelBuilder.Entity("Reservas.BData.Data.Entity.Huesped", b =>
                {
                    b.HasOne("Reservas.BData.Data.Entity.Habitacion", "Habitacion")
                        .WithOne("Huesped")
                        .HasForeignKey("Reservas.BData.Data.Entity.Huesped", "HabitacionId");

                    b.HasOne("Reservas.BData.Data.Entity.Persona", "Persona")
                        .WithOne("Huesped")
                        .HasForeignKey("Reservas.BData.Data.Entity.Huesped", "PersonaId");

                    b.Navigation("Habitacion");

                    b.Navigation("Persona");
                });

            modelBuilder.Entity("Reservas.BData.Data.Entity.Habitacion", b =>
                {
                    b.Navigation("Huesped");
                });

            modelBuilder.Entity("Reservas.BData.Data.Entity.Persona", b =>
                {
                    b.Navigation("Huesped");
                });
#pragma warning restore 612, 618
        }
    }
}
