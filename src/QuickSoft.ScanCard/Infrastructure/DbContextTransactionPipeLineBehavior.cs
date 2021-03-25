using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace QuickSoft.ScanCard.Infrastructure
{
    /// <summary>
    /// Adds transaction to the processing pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class DbContextTransactionPipeLineBehavior <TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ApplicationDbContext _dbContext;

        public DbContextTransactionPipeLineBehavior(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;

            try
            {
                _dbContext.BeginTransaction();
                response = await next();
                _dbContext.CommitTransaction();
            }
            catch
            {
                _dbContext.RollbackTransaction();
                throw;
            }

            return response;
        }
    }
}