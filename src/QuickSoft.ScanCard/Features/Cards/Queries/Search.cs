using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Infrastructure;
using QuickSoft.ScanCard.Infrastructure.Errors;

namespace QuickSoft.ScanCard.Features.Cards.Queries
{
    public static class Search
    {
        public class Query : IRequest<Domain.CardEnvelope>
        {
            public string CardNumber { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
        }

        public class Handler : IRequestHandler<Query, Domain.CardEnvelope>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            
            public async Task<Domain.CardEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _context.Cards
                    .GroupJoin(_context.Customers, card => card.CustomerId, customer => customer.Id, (card, customer) => new {card, customer})
                    .SelectMany(t => t.customer.DefaultIfEmpty(), (card, customer) => new {card, customer})
                    .Where(t => t.card.card.CardNumber == request.CardNumber || t.customer.Name == request.Username)
                    .AsNoTracking()
                    .Select(t => t.card.card)
                    .FirstOrDefaultAsync(cancellationToken);
                
                if (result == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Card = Constants.NOT_FOUND });
                }

                return new Domain.CardEnvelope(result);
            }
        }
    }
}