using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using QuickSoft.ScanCard.Domain;
using QuickSoft.ScanCard.Infrastructure.EntityConfigurations;
using QuickSoft.ScanCard.Infrastructure.Security;

namespace QuickSoft.ScanCard.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IPasswordHasher _passwordHasher;
        private IDbContextTransaction _contextTransaction;

        public ApplicationDbContext(DbContextOptions options, IPasswordHasher passwordHasher) 
            : base(options)
        {
            _passwordHasher = passwordHasher;
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuditConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            
            var person = new Person()
            {
                Id = 1,
                Username = "me",
                ProfileUrl = "",
                Phone = "078391398",
                UserType = 1,
                Password = _passwordHasher.Hash("123"),
                CreatedDate = DateTime.Now
            };
            modelBuilder.Entity<Person>().HasData(person);
            
        
            base.OnModelCreating(modelBuilder);
        }

        #region Transaction Handler
        public void BeginTransaction()
        {
            if (_contextTransaction != null)
            {
                return;
            }

            // if (!Database.IsInMemory())
            // {
                _contextTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
            // }
        }
        public void CommitTransaction()
        {
            try
            {
                _contextTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_contextTransaction != null)
                {
                    _contextTransaction.Dispose();
                    _contextTransaction = null;
                }
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                _contextTransaction?.Rollback();
            }
            finally
            {
                if (_contextTransaction != null)
                {
                    _contextTransaction.Dispose();
                    _contextTransaction = null;
                }
            }
        }
        #endregion
    }
}