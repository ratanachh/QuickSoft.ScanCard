using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using QuickSoft.ScanCard.Infrastructure.Errors;

namespace QuickSoft.ScanCard.Features.Customers.Queries
{
    public static class Search
    {
        public class Query : IRequest<CustomersEnvelope>
        {
            public string CardNumber { get; set; }
        }
        
        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.CardNumber).NotEmpty().NotNull();
            }
        }
        
        public class Handler : IRequestHandler<Query,CustomersEnvelope>
        {
            private readonly ICustomerReader _customerReader;

            public Handler(ICustomerReader customerReader)
            {
                _customerReader = customerReader;
            }
            public async Task<CustomersEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var customers = new List<Customer>();
                if (request.CardNumber.Equals("*"))
                {
                    customers = await _customerReader.SelectAll(cancellationToken);
                }
                else
                {
                    customers.Add(await _customerReader.SearchByCardNumber(request.CardNumber, cancellationToken));
                }

                if (customers.Count == 0)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Customers = Constants.NOT_FOUND});
                }

                return new CustomersEnvelope(customers);
            }
        }
    }
}