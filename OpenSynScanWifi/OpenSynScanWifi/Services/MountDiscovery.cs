using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Constants;
using Xamarin.Forms;

namespace OpenSynScanWifi.Services
{
	public sealed class MountDiscovery : IMountDiscovery, INotifyPropertyChanged
	{
		public ObservableCollection<WifiMount> DeviceIpEndPoints { get; private set; } = new ObservableCollection<WifiMount>();

		[NotNull] private readonly UdpClient _udpClient;

		public MountDiscovery([NotNull] UdpClient udpClient)
		{
			this._udpClient = udpClient;
		}

		public Task DiscoverAsync(CancellationToken cancellationToken)
		{
			DeviceIpEndPoints.Clear();

			Task.Run(async () =>
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					UdpReceiveResult receiveResult = await this._udpClient.ReceiveAsync().ConfigureAwait(false);

					IPEndPoint remoteEndPoint = receiveResult.RemoteEndPoint;

					Debug.WriteLine(remoteEndPoint.Address);

					var receivedMessage = BitConverter.ToString(receiveResult.Buffer);

					var buffer = receiveResult.Buffer.ToList();

					Debug.WriteLine("GOT MESSAGE");

					Debug.WriteLine(receivedMessage);

					if (buffer[0] == 13)
					{
						buffer.RemoveAt(0);
					}

					WifiMount wifiMount = WifiMount.ToWifiMount(remoteEndPoint);

					if (this.DeviceIpEndPoints.FirstOrDefault(d => d.Address == wifiMount.Address) == null)
					{
						Device.BeginInvokeOnMainThread(() => this.DeviceIpEndPoints.Add(wifiMount));

						Task.Run(async () => { await this.Handshake(wifiMount).ConfigureAwait(false); });
					}

					if (buffer.SequenceEqual(WifiConstants.SMSG_DISCOVERY_DATAGRAM))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_DISCOVERY_DATAGRAM2))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_DISCOVERY_DATAGRAM3))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_DISCOVERY_DATAGRAM5))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_DISCOVERY_DATAGRAM4))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_UNK_A1))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_UNK_A2))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_UNK_B1))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_UNK_G1))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_UNK_G2))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_UNK_F1))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_UNK_F2))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_UNK_f1))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_UNK_f2))
					{
						//
					}
					else if (buffer.SequenceEqual(WifiConstants.SMSG_MOUNT_MODE_QUERY))
					{
						//
					}
					else
					{
						Debug.WriteLine("!!!NEW PACKET!!!");
					}

					Array.ForEach(buffer.ToArray(), b => Debug.Write($"{b}, "));

					Debug.WriteLine("");
				}
			}, cancellationToken);

			return Task.CompletedTask;
		}

		public async Task FindAsync()
		{
			this._udpClient.EnableBroadcast = true;

			await this._udpClient.SendAsync(WifiConstants.CMSG_DISCOVERY_DATAGRAM, WifiConstants.CMSG_DISCOVERY_DATAGRAM.Length, new IPEndPoint(IPAddress.Parse("192.168.4.255"), 11880)).ConfigureAwait(false);

			Debug.WriteLine("Sent DISCOVERY1");
		}

		public async Task Handshake(WifiMount mount)
		{
			await this.AckDiscovery2(mount);

			await Task.Delay(2000);

			await this.AckDiscovery3(mount);

			await Task.Delay(2000);

			await this.AckDiscovery4(mount);

			await Task.Delay(2000);

			this.AckDiscoveryEnd(mount);

			await Task.Delay(5000);

			await this.AckDiscovery5(mount);

			await Task.Delay(2000);

			await this.SendCommand(mount, WifiConstants.CMSG_UNK_A1);
			Debug.WriteLine("Sent CMSG_UNK_A1");

			await this.SendCommand(mount, WifiConstants.CMSG_UNK_A2);
			Debug.WriteLine("Sent CMSG_UNK_A2");

			await this.SendCommand(mount, WifiConstants.CMSG_UNK_B1);
			Debug.WriteLine("Sent CMSG_UNK_B1");

			await this.SendCommand(mount, WifiConstants.CMSG_UNK_G1);
			Debug.WriteLine("Sent CMSG_UNK_G1");

			await this.SendCommand(mount, WifiConstants.CMSG_UNK_G2);
			Debug.WriteLine("Sent CMSG_UNK_G2");

			await this.SendCommand(mount, WifiConstants.CMSG_UNK_F1);
			Debug.WriteLine("Sent CMSG_UNK_F1");

			await this.SendCommand(mount, WifiConstants.CMSG_UNK_F2);
			Debug.WriteLine("Sent CMSG_UNK_F2");

			await this.SendCommand(mount, WifiConstants.CMSG_UNK_f1);
			Debug.WriteLine("Sent CMSG_UNK_f1");

			await this.SendCommand(mount, WifiConstants.CMSG_UNK_f2);
			Debug.WriteLine("Sent CMSG_UNK_f2");

			await this.SendCommand(mount, WifiConstants.CMSG_MOUNT_MODE_QUERY);
			Debug.WriteLine("Sent CMSG_MOUNT_MODE_QUERY");
		}

		public async Task SendCommand(WifiMount mount, byte[] command)
		{
			await this._udpClient.SendAsync(command, command.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);

			await Task.Delay(2000);
		}

		public async Task AckDiscovery2(WifiMount mount)
		{
			await this._udpClient.SendAsync(WifiConstants.CMSG_DISCOVERY_DATAGRAM2, WifiConstants.CMSG_DISCOVERY_DATAGRAM2.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);
			Debug.WriteLine("Sent CMSG_DISCOVERY_DATAGRAM2");
		}

		public async Task AckDiscovery3(WifiMount mount)
		{
			await this._udpClient.SendAsync(WifiConstants.CMSG_DISCOVERY_DATAGRAM3, WifiConstants.CMSG_DISCOVERY_DATAGRAM3.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);
			Debug.WriteLine("Sent CMSG_DISCOVERY_DATAGRAM3");
		}

		public async Task AckDiscovery4(WifiMount mount)
		{
			await this._udpClient.SendAsync(WifiConstants.CMSG_DISCOVERY_DATAGRAM4, WifiConstants.CMSG_DISCOVERY_DATAGRAM4.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);
			Debug.WriteLine("Sent CMSG_DISCOVERY_DATAGRAM4");
		}

		public async Task AckDiscoveryEnd(WifiMount mount)
		{
			await this._udpClient.SendAsync(WifiConstants.CMSG_START_DATAGRAM, WifiConstants.CMSG_START_DATAGRAM.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);
			Debug.WriteLine("Sent CMSG_START_DATAGRAM");
		}

		public async Task AckDiscovery5(WifiMount mount)
		{
			await this._udpClient.SendAsync(WifiConstants.CMSG_DISCOVERY_DATAGRAM5, WifiConstants.CMSG_START_DATAGRAM.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);
			Debug.WriteLine("Sent CMSG_DISCOVERY_DATAGRAM5");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this._udpClient != null);
		}
	}
}