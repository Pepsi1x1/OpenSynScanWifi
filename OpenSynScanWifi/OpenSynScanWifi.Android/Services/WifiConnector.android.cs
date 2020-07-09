using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using OpenSynScanWifi.Exceptions;
using OpenSynScanWifi.Services;

namespace OpenSynScanWifi.Droid.Services
{
#pragma warning disable 612, 618

	public class WifiConnector : IWifiConnector
	{
		readonly WifiManager _wifiManager;

		public WifiConnector()
		{
			this._wifiManager = (WifiManager) Application.Context.GetSystemService(Context.WifiService);
		}

		public event EventHandler OnConnected;

		public IEnumerable<Models.WifiDescriptor> List()
		{
			bool scanned = this._wifiManager.StartScan();

			if (scanned)
			{
				return this._wifiManager.ScanResults.Select(cn => new Models.WifiDescriptor {Name = cn.OperatorFriendlyName.ToString(), Ssid = cn.Ssid});
			}
			else
			{
				return Enumerable.Empty<Models.WifiDescriptor>();
			}
		}

		public void Connect(Models.WifiDescriptor descriptor, string passPhrase)
		{
			WifiConfiguration wifiConfig = new WifiConfiguration
			{
				Ssid = $"\"{descriptor.Ssid}\"",
				PreSharedKey = $"\"{passPhrase}\""
			};

			try
			{
				this._wifiManager.AddNetwork(wifiConfig);
			}
			catch (Exception ex)
			{
				throw new WifiException("WifiConnector can not add the new wifi network configuration", ex);
			}

			WifiConfiguration network = null;
			try
			{
				network = this._wifiManager.ConfiguredNetworks
					.FirstOrDefault(n => n.Ssid == wifiConfig.Ssid);

				if (network == null)
				{
					throw new WifiException("WifiConnector can not connect to the specified wifi network");
				}
			}
			catch (Exception ex)
			{
				throw new WifiException("WifiConnector can not get the list of configured wifi networks", ex);
			}

			try
			{
				this._wifiManager.Disconnect();

				bool networkEnabled = _wifiManager.EnableNetwork(network.NetworkId, true);

				if (this.VerifyConnectivity(wifiConfig.Ssid, networkEnabled))
				{
					OnConnected?.Invoke(this, EventArgs.Empty);
				}
				else
				{
					throw new WifiException("The specified wifi network does not exist");
				}
			}
			catch (Exception ex)
			{
				throw new WifiException("Activating the connection to the configured wifi network failed", ex);
			}
		}

		private bool VerifyConnectivity(string formattedSsid, bool networkEnabled)
		{
			if (!networkEnabled)
			{
				return false;
			}

			if (this._wifiManager.ConnectionInfo == null)
			{
				return false;
			}


			return _wifiManager.ConnectionInfo.SSID.Equals(formattedSsid);
		}
	}
#pragma warning restore 612, 618
}