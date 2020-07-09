using System.Threading;
using System.Threading.Tasks;

namespace OpenSynScanWifi.Services
{
	public interface IMountControl
	{
		Task EchoAsync();
		Task ListenAsync(CancellationToken cancellationToken);
	}
}