using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace QuickSoft.ScanCard.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        private IDbContextTransaction _contextTransaction;

        public ApplicationDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #region Transaction Handler
        public void BeginTransaction()
        {
            if (_contextTransaction != null)
            {
                return;
            }

            if (!Database.IsInMemory())
            {
                _contextTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
            }
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