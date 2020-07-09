using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Services
{
	public interface IMountDiscovery
	{
		ObservableCollection<IMountInfo> ConnectedMounts { get; }

		Task DiscoverAsync(CancellationToken cancellationToken);

		Task FindAsync();
	}
}