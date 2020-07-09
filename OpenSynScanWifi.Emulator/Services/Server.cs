using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Constants;
using OpenSynScanWifi.Emulator.Models;

namespace OpenSynScanWifi.Emulator.Services
{
	public class Server
	{
		public ObservableCollection<WifiClient> DeviceIpEndPoints { get; private set; } = new ObservableCollection<WifiClient>();

		[NotNull] private readonly UdpClient _udpClient;

		public Server(UdpClient udpClient)
		{
			this._udpClient = udpClient;
		}

		public void Listen(CancellationToken cancellationToken)
		{
			Task.Run(async () =>
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					UdpReceiveResult receiveResult = await this._udpClient.ReceiveAsync().ConfigureAwait(false);

					IPEndPoint remoteEndPoint = receiveResult.RemoteEndPoint;

					var buffer = receiveResult.Buffer.ToList();

					if (buffer[0] == 13)
					{
						buffer.RemoveAt(0);
					}

					if (buffer.SequenceEqual(WifiConstants.CMSG_DISCOVERY_DATAGRAM))
					{
						Debug.WriteLine("<<<CMSG_DISCOVERY_DATAGRAM---");

						Debug.WriteLine("Response");

						var responseMessage = BitConverter.ToString(WifiConstants.SMSG_DISCOVERY_DATAGRAM);

						Debug.WriteLine(responseMessage);

						await this._udpClient.SendAsync(WifiConstants.SMSG_DISCOVERY_DATAGRAM, WifiConstants.SMSG_DISCOVERY_DATAGRAM.Length, receiveResult.RemoteEndPoint).ConfigureAwait(false);
						Debug.WriteLine(">>>SMSG_DISCOVERY_DATAGRAM---");
					}
					else if (buffer.SequenceEqual(WifiConstants.CMSG_DISCOVERY_DATAGRAM2))
					{
						Debug.WriteLine("<<<CMSG_DISCOVERY_DATAGRAM2---");

						Debug.WriteLine("Response");

						var responseMessage = BitConverter.ToString(WifiConstants.SMSG_DISCOVERY_DATAGRAM2);

						Debug.WriteLine(responseMessage);

						await this._udpClient.SendAsync(WifiConstants.SMSG_DISCOVERY_DATAGRAM2, WifiConstants.SMSG_DISCOVERY_DATAGRAM2.Length, receiveResult.RemoteEndPoint).ConfigureAwait(false);
						Debug.WriteLine(">>>SMSG_DISCOVERY_DATAGRAM2---");
					}
					else if (buffer.SequenceEqual(WifiConstants.CMSG_DISCOVERY_DATAGRAM3))
					{
						Debug.WriteLine("<<<CMSG_DISCOVERY_DATAGRAM3---");

						Debug.WriteLine("Response");

						var responseMessage = BitConverter.ToString(WifiConstants.SMSG_DISCOVERY_DATAGRAM3);

						Debug.WriteLine(responseMessage);

						await this._udpClient.SendAsync(WifiConstants.SMSG_DISCOVERY_DATAGRAM3, WifiConstants.SMSG_DISCOVERY_DATAGRAM3.Length, receiveResult.RemoteEndPoint).ConfigureAwait(false);
						Debug.WriteLine(">>>SMSG_DISCOVERY_DATAGRAM3---");
					}
					else if (buffer.SequenceEqual(WifiConstants.CMSG_DISCOVERY_DATAGRAM4))
					{
						Debug.WriteLine("<<<CMSG_DISCOVERY_DATAGRAM4---");

						Debug.WriteLine("Response");

						var responseMessage = BitConverter.ToString(WifiConstants.SMSG_DISCOVERY_DATAGRAM4);

						Debug.WriteLine(responseMessage);

						await this._udpClient.SendAsync(WifiConstants.SMSG_DISCOVERY_DATAGRAM4, WifiConstants.SMSG_DISCOVERY_DATAGRAM4.Length, receiveResult.RemoteEndPoint).ConfigureAwait(false);
						Debug.WriteLine(">>>SMSG_DISCOVERY_DATAGRAM4---");
					}
					else if (buffer.SequenceEqual(WifiConstants.CMSG_START_DATAGRAM))
					{
						Debug.WriteLine("<<<CMSG_START_DATAGRAM---");

						//Debug.WriteLine("Response");

						//var responseMessage = BitConverter.ToString(WifiConstants.SMSG_FINSHED_DATAGRAM);

						//Debug.WriteLine(responseMessage);

						//await this._udpClient.SendAsync(WifiConstants.SMSG_FINSHED_DATAGRAM, WifiConstants.SMSG_FINSHED_DATAGRAM.Length, receiveResult.RemoteEndPoint).ConfigureAwait(false);
						//Debug.WriteLine(">>>SMSG_FINSHED_DATAGRAM---");
					}
					else if (buffer.SequenceEqual(WifiConstants.CMSG_DISCOVERY_DATAGRAM5))
					{
						Debug.WriteLine("<<<CMSG_DISCOVERY_DATAGRAM5---");

						//Debug.WriteLine("Response");

						//var responseMessage = BitConverter.ToString(WifiConstants.SMSG_FINSHED_DATAGRAM);

						//Debug.WriteLine(responseMessage);

						//await this._udpClient.SendAsync(WifiConstants.SMSG_FINSHED_DATAGRAM, WifiConstants.SMSG_FINSHED_DATAGRAM.Length, receiveResult.RemoteEndPoint).ConfigureAwait(false);
					}
					else
					{
						Debug.WriteLine("!!!NEW PACKET!!!");
					}

					Debug.WriteLine(remoteEndPoint);

					var receivedMessage = BitConverter.ToString(receiveResult.Buffer);

					Debug.WriteLine(receivedMessage);

					Array.ForEach(buffer.ToArray(), b => Debug.Write($"{b}, "));

					Debug.WriteLine("");

					WifiClient wifiClient = WifiClient.ToWifiClient(remoteEndPoint);

					if (this.DeviceIpEndPoints.FirstOrDefault(d => d.Address == wifiClient.Address) == null)
					{
						this.DeviceIpEndPoints.Add(wifiClient);
					}
				}
			}, cancellationToken);
		}
	}
}