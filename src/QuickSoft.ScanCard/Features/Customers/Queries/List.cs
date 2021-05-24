using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Infrastructure;

namespace QuickSoft.ScanCard.Features.Customers.Queries
{
    public static class List
    {
        public record Query : IRequest<CustomersEnvelope>;
        
        public class Handler : IRequestHandler<Query, CustomersEnvelope>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<CustomersEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var customers = await 
                    _context.Customers
                    .GroupJoin(_context.Cards, cus1 => cus1.Id, card1 => card1.CustomerId,
                        (cus1, leftJ) => new {cus1, leftJ})
                    .SelectMany(t => t.leftJ.DefaultIfEmpty(), (t, cc) => new {t, cc})
                    .Where(t => t.cc.IsActive.Equals(true))
                    .AsNoTracking()
                    .Select(t => new Customer
                    {
                        Id = t.t.cus1.Id,
                        AuditId = t.t.cus1.AuditId,
                        CreatedDate = t.t.cus1.CreatedDate,
                        Name = t.t.cus1.Name,
                        Phone = t.t.cus1.Phone,
                        IsCardActive = t.cc.IsActive
                    })
                    .ToListAsync(cancellationToken);

                return new CustomersEnvelope(customers);
            }
        }
    }
}