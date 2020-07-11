using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ImTools;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Commands;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Services
{
	public abstract class MountControllerBase : INotifyPropertyChanged
	{
		[NotNull] protected readonly UdpClient _udpClient;

		[NotNull] protected readonly Random _rand = new Random(50101);

		[NotNull] protected readonly IMountCommonCommandBuilder _commonCommandBuilder;

		[NotNull] protected readonly IMountCommonCommandParser _commonCommandParser;

		protected MountControllerBase([NotNull] UdpClient udpClient,
			[NotNull] IMountCommonCommandBuilder commonCommandBuilder,
			[NotNull] IMountCommonCommandParser commonCommandParser)
		{
			this._udpClient = udpClient;

			this._commonCommandBuilder = commonCommandBuilder;

			this._commonCommandParser = commonCommandParser;
		}

		public bool ValidateResponse(byte[] response)
		{
			if (response.Length == 0)
			{
				return false;
			}

			return true;
		}

		public async Task SendCommand(WifiMount mount, byte[] command)
		{
			await this._udpClient.SendAsync(command, command.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);

			await Task.Delay(2000).ConfigureAwait(false);
		}

		public async Task<byte[]> SendRecieveCommand(WifiMount mount, byte[] command)
		{
			int port = this._rand.Next(50101, 50199);

			Debug.WriteLine($"UDP on port {port}");

			// TODO: Implement connection pooling
			UdpClient udpClient;
			try
			{
				udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port));
			}
			catch (SocketException)
			{
				port = this._rand.Next(50101, 50199);
				udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port));
			}

			await udpClient.SendAsync(command, command.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);

			UdpReceiveResult receiveResult = await udpClient.ReceiveAsync().ConfigureAwait(false);

			udpClient.Close();

			IPEndPoint remoteEndPoint = receiveResult.RemoteEndPoint;

			Debug.WriteLine(remoteEndPoint.Address);

			string receivedMessage = BitConverter.ToString(receiveResult.Buffer);

			byte[] buffer = receiveResult.Buffer;

			Debug.WriteLine("GOT MESSAGE");

			Debug.WriteLine(receivedMessage);

			if (buffer[0] == 13)
			{
				buffer = buffer.RemoveAt(0);
			}

			return buffer;
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