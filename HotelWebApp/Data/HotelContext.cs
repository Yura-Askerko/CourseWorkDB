using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HotelWebApp.Models;

namespace HotelWebApp.Data
{
    public partial class HotelContext : DbContext
    {
        public HotelContext()
        {
        }

        public HotelContext(DbContextOptions<HotelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Info> Info { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<RoomRate> RoomRates { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Servicetype> Servicetypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-9QN6I91\\SQLEXPRESS;Database=Hotel; Trusted_Connection =True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.List)
                    .HasColumnName("list")
                    .HasMaxLength(255);

                entity.Property(e => e.Middlename)
                    .HasColumnName("middlename")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.NameOfRoom)
                    .HasColumnName("name_of_room")
                    .HasMaxLength(255);

                entity.Property(e => e.PassportData)
                    .HasColumnName("passport_data")
                    .HasMaxLength(255);

                entity.Property(e => e.Surname)
                    .HasColumnName("surname")
                    .HasMaxLength(255);

                entity.Property(e => e.TotalCost)
                    .HasColumnName("total_cost")
                    .HasColumnType("money");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employees");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Middlename)
                    .HasColumnName("middlename")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Position)
                    .HasColumnName("position")
                    .HasMaxLength(255);

                entity.Property(e => e.Surname)
                    .HasColumnName("surname")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Info>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Info");

                entity.Property(e => e.CheckInDate)
                    .HasColumnName("check_in_date")
                    .HasColumnType("date");

                entity.Property(e => e.CheckOut)
                    .HasColumnName("check_out")
                    .HasColumnType("date");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.List)
                    .HasColumnName("list")
                    .HasMaxLength(255);

                entity.Property(e => e.Middlename)
                    .HasColumnName("middlename")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.NameOfRoom)
                    .HasColumnName("name_of_room")
                    .HasMaxLength(255);

                entity.Property(e => e.PassportData)
                    .HasColumnName("passport_data")
                    .HasMaxLength(255);

                entity.Property(e => e.Surname)
                    .HasColumnName("surname")
                    .HasMaxLength(255);

                entity.Property(e => e.TotalCost)
                    .HasColumnName("total_cost")
                    .HasColumnType("money");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CheckInDate)
                    .HasColumnName("check_in_date")
                    .HasColumnType("date");

                entity.Property(e => e.CheckOut)
                    .HasColumnName("check_out")
                    .HasColumnType("date");

                entity.Property(e => e.ClientId).HasColumnName("clientId");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__orders__clientId__46E78A0C");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__orders__employee__45F365D3");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__orders__roomId__47DBAE45");
            });

            modelBuilder.Entity<RoomRate>(entity =>
            {
                entity.ToTable("room_rates");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomRates)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__room_rate__roomI__4316F928");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("rooms");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.Specification)
                    .HasColumnName("specification")
                    .HasMaxLength(255);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("services");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.ServicetypeId).HasColumnName("servicetypeId");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__services__employ__44FF419A");

                entity.HasOne(d => d.Servicetype)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ServicetypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__services__servic__440B1D61");
            });

            modelBuilder.Entity<Servicetype>(entity =>
            {
                entity.ToTable("servicetypes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Specificaion)
                    .HasColumnName("specificaion")
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
