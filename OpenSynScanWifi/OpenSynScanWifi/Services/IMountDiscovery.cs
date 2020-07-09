using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSynScanWifi.Services
{
	public interface IMountDiscovery
	{
		ObservableCollection<WifiMount> DeviceIpEndPoints { get; }

		Task DiscoverAsync(CancellationToken cancellationToken);

		Task FindAsync();
	}
}