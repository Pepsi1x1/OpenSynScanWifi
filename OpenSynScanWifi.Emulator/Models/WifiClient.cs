using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.CompilerServices;
using OpenSynScanWifi.Annotations;

namespace OpenSynScanWifi.Emulator.Models
{
	public class WifiClient : INotifyPropertyChanged
	{
		[NotNull] private string _address = string.Empty;

		[NotNull]
		public string Address
		{
			get => this._address;
			set
			{
				Contract.Requires(value != null);

				if (value == this._address)
				{
					return;
				}

				this._address = value;

				this.OnPropertyChanged();
			}
		}

		private int _port;

		public int Port
		{
			get => this._port;
			set
			{
				if (value == this._port)
				{
					return;
				}

				this._port = value;

				this.OnPropertyChanged();
			}
		}

		[NotNull]
		public static WifiClient ToWifiClient(IPEndPoint i)
		{
			Contract.Ensures(Contract.Result<WifiClient>() != null);

			if (i == null)
			{
				return null;
			}

			WifiClient wifiMount = new WifiClient();

			wifiMount.Address = i.Address.ToString();

			wifiMount.Port = i.Port;

			return wifiMount;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this._address != null);
		}
	}
}