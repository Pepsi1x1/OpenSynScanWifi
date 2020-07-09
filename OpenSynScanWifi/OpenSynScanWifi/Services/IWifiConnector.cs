using System.Collections.Generic;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Services
{
	public interface IWifiConnector
	{
		void Connect(WifiDescriptor descriptor, string passPhrase);
		IEnumerable<WifiDescriptor> List();
	}
}