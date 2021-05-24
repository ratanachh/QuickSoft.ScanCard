using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuickSoft.ScanCard.Features.Customers.Queries
{
    public interface ICustomerReader
    {
        public Task<Customer> SearchByCardNumber(string cardNumber, CancellationToken cancellationToken);
        public Task<List<Customer>> SelectAll(CancellationToken cancellationToken);
    }
}