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
        public class Query : IRequest<Domain.Card>
        {
            public string CardNumber { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
        }

        public class Handler : IRequestHandler<Query, Domain.Card>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            
            public async Task<Domain.Card> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await (
                        from card in _context.Cards
                        join customer in _context.Customers on card.CustomerId equals customer.Id into cc
                        from cus in cc.DefaultIfEmpty()
                        where card.CardNumber.Equals(request.CardNumber) || cus.Name.Equals(request.Username)
                        select card
                    )
                    .FirstOrDefaultAsync(cancellationToken);
                if (result == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new {Card = Constants.NOT_FOUND});
                }

                return result;
            }
        }
    }
}