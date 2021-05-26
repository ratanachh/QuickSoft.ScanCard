using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using QuickSoft.ScanCard.Infrastructure;

namespace QuickSoft.ScanCard.Features.Customers.Commands
{
    public static class Create
    {
        public class Command : IRequest<CustomerEnvelope>
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string CardNumber { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().NotNull();
                RuleFor(x => x.Phone).NotEmpty().NotNull();
                RuleFor(x => x.CardNumber).NotEmpty().NotNull();
            }
        }
        
        public class Handler : IRequestHandler<Command, CustomerEnvelope>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CustomerEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                
            }
        }
    }
}