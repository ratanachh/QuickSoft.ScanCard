using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Infrastructure;

namespace QuickSoft.ScanCard.Features.Customers.Queries
{
    public class CustomerReader : ICustomerReader
    {
        private readonly ApplicationDbContext _context;

        public CustomerReader(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> SearchByCardNumber(string cardNumber, CancellationToken cancellationToken)
        {
            var customer = await 
                _context.Customers
                .Join(_context.Cards, cus => cus.Id, card => card.CustomerId, (cus, card) => new {cus, card})
                .Where(t => t.card.CardNumber == cardNumber)
                .AsNoTracking()
                .Select(t => new Customer
                {
                    Id = t.cus.Id,
                    AuditId = t.cus.AuditId,
                    CardNumber = t.card.CardNumber,
                    CreatedDate = t.cus.CreatedDate,
                    IsCardActive = t.card.IsActive,
                    Name = t.cus.Name,
                    Phone = t.cus.Phone
                })
                .FirstOrDefaultAsync(cancellationToken);
            return customer;
        }

        public async Task<List<Customer>> SelectAll(CancellationToken cancellationToken)
        {
            var customers = await 
                    _context.Customers
                    .GroupJoin(_context.Cards, cus => cus.Id, card => card.CustomerId, (cus, card) => new {cus, card})
                    .SelectMany(t => t.card.DefaultIfEmpty(), (customer, card) => new {customer, card})
                    .AsNoTracking()
                    .Select(t => new Customer
                    {
                        Id = t.customer.cus.Id,
                        AuditId = t.customer.cus.AuditId,
                        CardNumber = t.card.CardNumber,
                        CreatedDate = t.customer.cus.CreatedDate,
                        IsCardActive = t.card.IsActive,
                        Name = t.customer.cus.Name,
                        Phone = t.customer.cus.Phone
                    })
                    .ToListAsync(cancellationToken);
            return customers; 
        }
    }
}