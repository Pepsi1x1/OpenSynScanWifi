using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ImTools;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Commands;
using OpenSynScanWifi.Helpers;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Services
{
	public abstract class MountControllerBase : INotifyPropertyChanged
	{
		[NotNull] protected readonly IMountCommonCommandBuilder _commonCommandBuilder;

		[NotNull] protected readonly IMountCommonCommandParser _commonCommandParser;

		[NotNull] private readonly IObjectPool<UdpClient> _udpClientPool;

		protected MountControllerBase(
			[NotNull] IMountCommonCommandBuilder commonCommandBuilder,
			[NotNull] IMountCommonCommandParser commonCommandParser,
			[NotNull] IObjectPool<UdpClient> udpClientPool)
		{
			this._commonCommandBuilder = commonCommandBuilder;

			this._commonCommandParser = commonCommandParser;

			this._udpClientPool = udpClientPool;
		}

		public bool ValidateResponse(byte[] response)
		{
			if (response.Length == 0)
			{
				return false;
			}

			return true;
		}

		public async Task SendCommandAsync(WifiMount mount, byte[] command)
		{
			UdpClient udpClient = this._udpClientPool.Get();

			await udpClient.SendAsync(command, command.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);

			CancellationTokenSource source = new CancellationTokenSource();
			source.CancelAfter(5000);
			CancellationToken token = source.Token;
			token.ThrowIfCancellationRequested();

			// We don't need the response, but awaiting this ensures the mount will be ready for our next command
			// Worst case scenario is that we have to wait 5 seconds for a response, if we don't get it, we will
			// sack off the task and go about our business
			try
			{
				_ = await udpClient.ReceiveAsync(token).ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
				Debug.WriteLine("SendCommandAsync: Cancelled receive");
				// Sacked off, close it down and grab a new client, the consumer can carry on blissfully unaware
				udpClient.Close();
				udpClient = this._udpClientPool.Get();
			}

			_ = Task.Run(async () =>
			{
				await Task.Delay(100).ConfigureAwait(false);
				this._udpClientPool.Return(udpClient);
			});
		}

		public async Task<byte[]> SendReceiveCommandAsync(WifiMount mount, byte[] command)
		{

			UdpClient udpClient = this._udpClientPool.Get();

			CancellationTokenSource source = new CancellationTokenSource();
			source.CancelAfter(5000);
			CancellationToken token = source.Token;

			await udpClient.SendAsync(command, command.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);

			UdpReceiveResult receiveResult;
			try
			{
				receiveResult = await udpClient.ReceiveAsync(token).ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
					Debug.WriteLine("SendReceiveCommandAsync: Cancelled receive, retrying");
					udpClient.Close();
					udpClient = this._udpClientPool.Get();

					CancellationTokenSource source2 = new CancellationTokenSource();
					source.CancelAfter(5000);
					CancellationToken token2 = source2.Token;
					token2.ThrowIfCancellationRequested();

					try
					{
						await udpClient.SendAsync(command, command.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);
						receiveResult = await udpClient.ReceiveAsync(token2).ConfigureAwait(false);
					}
					catch (OperationCanceledException)
					{
						udpClient.Close();
						return new byte[1];
					}
			}

			_ = Task.Run(async () =>
			{
				await Task.Delay(100).ConfigureAwait(false);
				this._udpClientPool.Return(udpClient);
			});

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
	}
}