namespace QuickSoft.ScanCard.Features.Customers.Queries
{
    using MediatR;
    using System.Linq;    
    using Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    public static class List
    {
        public record Query : IRequest<List<Customer>>;
        
        public class Handler : IRequestHandler<Query, List<Customer>>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<Customer>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await (from cus1 in _context.Customers
                        join card1 in _context.Cards on cus1.Id equals card1.CustomerId into leftJ
                        from cc in leftJ.DefaultIfEmpty()
                        where cc.IsActive.Equals(true)
                        select new Customer
                        {
                            Id = cus1.Id,
                            AuditId = cus1.AuditId,
                            CreatedDate = cus1.CreatedDate,
                            Name = cus1.Name,
                            Phone = cus1.Phone,
                            IsCardActive = cc.IsActive
                        }
                    ).ToListAsync(cancellationToken);
            }
        }
    }
}