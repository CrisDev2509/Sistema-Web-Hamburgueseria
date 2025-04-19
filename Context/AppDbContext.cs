using Bigtoria.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Bigtoria.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<LoginEmployee> LoginEmpleyoee { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EmployeeType> EmployeesType { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SalesDetail { get; set; }
        public DbSet<Delivery> Delivery { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relaciones
            modelBuilder.Entity<LoginEmployee>()
                .HasOne(le => le.Employee)
                .WithOne(e => e.LoginEmployee)
                .HasForeignKey<LoginEmployee>(le => le.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.EmployeeType)
                .WithMany(et => et.empleyoees)
                .HasForeignKey(e => e.EmployeeTypeId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<SaleDetail>()
                .HasOne(sd => sd.Product)
                .WithMany(p => p.SaleDetail)
                .HasForeignKey(sd => sd.ProductId);

            modelBuilder.Entity<SaleDetail>()
                .HasOne(sd => sd.Sale)
                .WithOne(p => p.SaleDetail)
                .HasForeignKey<SaleDetail>(sd => sd.SaleId);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Sales)
                .HasForeignKey(s => s.EmployeeId);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Client)
                .WithMany(c => c.Sales)
                .HasForeignKey(s =>s.ClientId);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Sale)
                .WithOne(s  => s.Delivery)
                .HasForeignKey<Delivery>(d => d.SaleId);

            //Atributos
            modelBuilder.Entity<LoginEmployee>(t =>
            {
                t.HasKey(c => c.Id);

                t.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                t.Property(c => c.Email).IsRequired().HasMaxLength(64);
                t.Property(c => c.Password).IsRequired().HasMaxLength(64);
                t.Property(c => c.EmployeeId).IsRequired();

                t.ToTable("LoginEmployee");
            });

            modelBuilder.Entity<Client>(t =>
            {
                t.HasKey(c => c.Id);

                t.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                t.Property(c => c.Name).IsRequired().HasMaxLength(35);
                t.Property(c => c.Lastname).IsRequired().HasMaxLength(35);
                t.Property(c => c.Email).HasMaxLength(64);
                t.Property(c => c.Phone).HasMaxLength(12);

                t.ToTable("Client");
            });

            modelBuilder.Entity<Employee>(t =>
            {
                t.HasKey(c => c.Id);

                t.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                t.Property(c => c.Name).IsRequired().HasMaxLength(35);
                t.Property(c => c.Lastname).IsRequired().HasMaxLength(35);
                t.Property(c => c.Email).IsRequired();
                t.Property(c => c.Phone).IsRequired();
                t.Property(c => c.ContractDate).IsRequired();
                t.Property(c => c.Birthdate).IsRequired();
                t.Property(c => c.Salary).IsRequired();
                t.Property(c => c.Status).IsRequired();
                t.Property(c => c.EmployeeTypeId).IsRequired();
                t.Property(c => c.ImagePath);

                t.ToTable("Employee");
            });

            modelBuilder.Entity<EmployeeType>(t =>
            {
                t.HasKey(c => c.Id);

                t.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                t.Property(c => c.Name).IsRequired().HasMaxLength(30);
                t.Property(c => c.Status).IsRequired().HasMaxLength(8);

                t.ToTable("EmployeeType");
            });

            modelBuilder.Entity<Product>(t =>
            {
                t.HasKey(c => c.Id);

                t.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                t.Property(c => c.Name).IsRequired().HasMaxLength(30);
                t.Property(c => c.Description).IsRequired();
                t.Property(c => c.ShowStore).IsRequired();
                t.Property(c => c.Status).IsRequired();
                t.Property(c => c.Price).IsRequired().HasPrecision(6, 2);
                t.Property(c => c.Stock).IsRequired().HasPrecision(6, 2);
                t.Property(c => c.Discount).IsRequired().HasPrecision(6, 2).HasDefaultValue(0);
                t.Property(c => c.CategoryId).IsRequired();
                t.Property(c => c.ImagePath);

                t.ToTable("Product");
            });

            modelBuilder.Entity<Category>(t =>
            {
                t.HasKey(c => c.Id);

                t.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                t.Property(c => c.Name).IsRequired().HasMaxLength(30);
                t.Property(c => c.Status).IsRequired().HasMaxLength(9);
                t.Property(c => c.ShowFilter).IsRequired();

                t.ToTable("Category");
            });

            modelBuilder.Entity<Sale>(t =>
            {
                t.HasKey(c => c.Id);

                t.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                t.Property(c => c.Date).IsRequired();
                t.Property(c => c.SaleType).IsRequired();
                t.Property(c => c.Payment).HasMaxLength(9).IsRequired();
                t.Property(c => c.PercentageIGV).IsRequired().HasPrecision(6,2);
                t.Property(c => c.IGV).IsRequired().HasPrecision(6,2);
                t.Property(c => c.SubTotal).IsRequired().HasPrecision(6, 2);
                t.Property(c => c.Total).IsRequired().HasPrecision(6, 2);
                t.Property(c => c.ClientId).IsRequired(false);
                t.Property(c => c.State).IsRequired();
                t.Property(c => c.EmployeeId).IsRequired();

                t.ToTable("Sales");
            });

            modelBuilder.Entity<SaleDetail>(t =>
            {
                t.HasKey(c => c.Id);

                t.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                t.Property(c => c.Quantity).IsRequired().HasPrecision(6, 2);
                t.Property(c => c.UnitPrice).IsRequired().HasPrecision(6,2);
                t.Property(c => c.ProductId).IsRequired();
                t.Property(c => c.SaleId).IsRequired();

                t.ToTable("SaleDetail");
            });

            modelBuilder.Entity<Delivery>(t =>
            {
                t.HasKey(c => c.Id);

                t.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                t.Property(c => c.Address).IsRequired().HasMaxLength(60);
                t.Property(c => c.Reference).IsRequired();
                t.Property(c => c.State).IsRequired();
                t.Property(c => c.SaleId).IsRequired();

                t.ToTable("Delivery");
            });
        }
    }
}

