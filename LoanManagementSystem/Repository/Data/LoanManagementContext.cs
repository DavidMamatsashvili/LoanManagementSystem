using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository.Data
{
    public class LoanManagementContext : DbContext
    {
        public LoanManagementContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LoanSchedule> LoanSchedules { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("customer");

                entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(255); ;
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();

                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.PersonalNumber).HasColumnName("personal_number");
                entity.Property(e => e.BirthDate).HasColumnName("birth_date");
                entity.Property(e => e.CreditScore).HasColumnName("credit_score");
                entity.Property(e => e.Role).HasColumnName("role").HasConversion<string>();

                entity.Property(e => e.PersonalNumber)
                    .IsRequired()
      .HasMaxLength(11);

                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.HasIndex(e => e.PersonalNumber).IsUnique();
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("admin");

                entity.Property(e => e.Email).HasColumnName("email").IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.Role).HasColumnName("role").HasConversion<string>().IsRequired();

                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("loan");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");
                entity.Property(e => e.Amount).HasColumnName("amount").HasPrecision(18, 2);
                entity.Property(e => e.InterestRate).HasColumnName("interest_rate").HasPrecision(5, 2);
                entity.Property(e => e.TermMonths).HasColumnName("term_months");
                entity.Property(e => e.MonthlyPayment).HasColumnName("monthly_payment").HasPrecision(18, 2);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");


                entity.HasOne(e => e.Customer).WithMany(e => e.Loans).HasForeignKey(e => e.CustomerId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("payment");

                entity.Property(e => e.LoanId).HasColumnName("loan_id");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.PaymentDate).HasColumnName("payment_date");

                entity.HasOne(e => e.Loan).WithMany(e => e.Payments).HasForeignKey(e => e.LoanId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoanSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("loan_schedule");

                entity.Property(e => e.LoanId)
                      .HasColumnName("loan_id");

                entity.Property(e => e.PMT)
                      .HasColumnName("pmt")
                      .HasPrecision(18, 2);

                entity.Property(e => e.Date)
                      .HasColumnName("date");

                entity.HasOne(e => e.Loan)
                      .WithMany(e => e.LoanSchedules)
                      .HasForeignKey(e => e.LoanId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
