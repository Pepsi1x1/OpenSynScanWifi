using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Services
{
	public class MountControl : IMountControl
	{
		[NotNull] private readonly UdpClient _udpClient;

		[NotNull] private IMountOptions _mountOptions;

		public MountControl([NotNull] UdpClient udpClient, [NotNull] IMountOptions mountOptions)
		{
			this._udpClient = udpClient;

			this._mountOptions = mountOptions;
		}

		public Task ListenAsync(CancellationToken cancellationToken)
		{
			Task.Run(async () =>
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					UdpReceiveResult receiveResult = await this._udpClient.ReceiveAsync().ConfigureAwait(false);

					if (cancellationToken.IsCancellationRequested)
					{
						break;
					}

					IPEndPoint remoteEndPoint = receiveResult.RemoteEndPoint;

					Debug.WriteLine(remoteEndPoint.Address);

					var receivedMessage = BitConverter.ToString(receiveResult.Buffer);

					Debug.WriteLine(receivedMessage);
				}
			}, cancellationToken);

			return Task.CompletedTask;
		}

		public Task EchoAsync()
		{
			// hello world
			var command = new byte[] {0x3a, 75, 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100};

			return this.SendCommandAsync(command);
		}

		public async Task SendCommandAsync([NotNull] byte[] command)
		{
			this.ValidateSendCommand(command);

			Contract.Requires(command.Length > 0);

			Debug.WriteLine($"Sending UDP datagram {BitConverter.ToString(command)} to {this._mountOptions.WifiMount.Address}:{this._mountOptions.WifiMount.Port} ");

			var res = await this._udpClient.SendAsync(command, command.Length, new IPEndPoint(IPAddress.Parse(this._mountOptions.WifiMount.Address), this._mountOptions.WifiMount.Port)).ConfigureAwait(false);

			Debug.WriteLine(res);
		}

		private void ValidateSendCommand(byte[] command)
		{
			if (command is null)
			{
				throw new System.ArgumentNullException(nameof(command));
			}
		}
	}
}