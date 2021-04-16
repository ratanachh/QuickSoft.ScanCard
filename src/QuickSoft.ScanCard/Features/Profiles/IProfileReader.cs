using System.Threading;
using System.Threading.Tasks;

namespace QuickSoft.ScanCard.Features.Profiles
{
    public interface IProfileReader
    {
        Task<ProfileEnvelope> ReadProfile(string username, CancellationToken cancellationToken);
    }
}