using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Foundation;
using NetworkExtension;
using OpenSynScanWifi.Exceptions;
using OpenSynScanWifi.Services;

namespace OpenSynScanWifi.iOS.Services
{
	public class WifiConnector : IWifiConnector
	{
		public event EventHandler OnConnected;

		readonly NEHotspotConfigurationManager _wifiManager;

		public WifiConnector()
		{
			this._wifiManager = new NEHotspotConfigurationManager();
		}

		public IEnumerable<Models.WifiDescriptor> List()
		{
			return Enumerable.Empty<Models.WifiDescriptor>();
		}

		public void Connect(Models.WifiDescriptor descriptor, string passPhrase)
		{
			NEHotspotConfiguration wifiConfig = new NEHotspotConfiguration(descriptor.Ssid, passPhrase, false);

			wifiConfig.JoinOnce = true;

			try
			{
				if (this._wifiManager == null)
				{
					throw new WifiException("WifiConnector can not access the device WifiManager");
				}

				this._wifiManager.RemoveConfiguration(descriptor.Ssid);

				this._wifiManager.ApplyConfiguration(wifiConfig, error => this.CompletionHandler(error, descriptor.Ssid));
			}
			catch (Exception ex)
			{
				throw new WifiException("WifiConnector can not add the new wifi network configuration", ex);
			}
		}

		private void CompletionHandler(NSError error, string ssid)
		{
			if (error != null)
			{
				Debug.WriteLine(error.ToString());

				if (error.ToString().Contains("internal"))
				{
					const string message = "Please check and manually add your Entitlements.plist instead of using automatic provisioning profile.";

					Debug.WriteLine(message);

					throw new WifiException(message);
				}
				else
				{
					string message = $"Error while connecting to WiFi network {ssid}: {error}";

					Debug.WriteLine(message);

					throw new WifiException(message);
				}
			}
			else
			{
				this.OnConnected?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}