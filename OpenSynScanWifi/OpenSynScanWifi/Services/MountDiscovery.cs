using System;
using System.Collections.Generic;
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
using ImTools;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Commands;
using OpenSynScanWifi.Constants;
using OpenSynScanWifi.Helpers;
using OpenSynScanWifi.Models;
using Xamarin.Forms;

namespace OpenSynScanWifi.Services
{
	public sealed class MountDiscovery : IMountDiscovery, INotifyPropertyChanged
	{
		public ObservableCollection<IMountInfo> ConnectedMounts { get; private set; } = new ObservableCollection<IMountInfo>();

		private List<string> _discoveredMounts = new List<string>();

		[NotNull] private readonly UdpClient _udpClient;

		[NotNull] private readonly IMountInitialisationCommandBuilder _commandBuilder;

		[NotNull] private readonly IMountInitialisationCommandParser _commandParser;

		[NotNull] private readonly IMountCommonCommandBuilder _commonCommandBuilder;

		[NotNull] private readonly IMountCommonCommandParser _commonCommandParser;

		[NotNull] private readonly Random _rand = new Random(50101);

		public MountDiscovery([NotNull] UdpClient udpClient, 
			[NotNull] IMountInitialisationCommandBuilder commandBuilder, 
			[NotNull] IMountInitialisationCommandParser commandParser,
			[NotNull] IMountCommonCommandBuilder commonCommandBuilder, 
			[NotNull] IMountCommonCommandParser commonCommandParser)
		{
			this._udpClient = udpClient;

			this._commandBuilder = commandBuilder;

			this._commandParser = commandParser;

			this._commonCommandBuilder = commonCommandBuilder;

			this._commonCommandParser = commonCommandParser;
		}

		public Task DiscoverAsync(CancellationToken cancellationToken)
		{
			this.ConnectedMounts.Clear();

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

					var buffer = receiveResult.Buffer;

					Debug.WriteLine("GOT MESSAGE");

					Debug.WriteLine(receivedMessage);

					if (buffer[0] == 13)
					{
						buffer = buffer.RemoveAt(0);
					}

					WifiMount wifiMount = WifiMount.ToWifiMount(remoteEndPoint);

					if (!this._discoveredMounts.Contains(wifiMount.Address))
					{
						this._discoveredMounts.Add(wifiMount.Address);

						MountInfo mountOptions = new MountInfo() {WifiMount = wifiMount};

						Task.Run(async () => { await this.Handshake(mountOptions).ConfigureAwait(false); });
					}
					
#if DEBUG
					Array.ForEach(buffer.ToArray(), b => Debug.Write($"{b}, "));
#endif

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

		public async Task Handshake(IMountInfo mountOptions)
		{
			await this.ResetRxBuffer(mountOptions, MountAxis.LeftRight).ConfigureAwait(false);

			await this.ResetRxBuffer(mountOptions, MountAxis.UpDown).ConfigureAwait(false);


			await this.GetMotorBoardVersion(mountOptions).ConfigureAwait(false);


			await this.QueryCountsPerRevolution(mountOptions, MountAxis.LeftRight).ConfigureAwait(false);

			await this.QueryCountsPerRevolution(mountOptions, MountAxis.UpDown).ConfigureAwait(false);


			await this.QueryTimerInteruptFrequency(mountOptions, MountAxis.LeftRight).ConfigureAwait(false);

			await this.QueryTimerInteruptFrequency(mountOptions, MountAxis.UpDown).ConfigureAwait(false);


			await this.QueryHighSpeedRatio(mountOptions, MountAxis.LeftRight).ConfigureAwait(false);

			await this.QueryHighSpeedRatio(mountOptions, MountAxis.UpDown).ConfigureAwait(false);


			await this.QueryAxisPosition(mountOptions, MountAxis.LeftRight).ConfigureAwait(false);

			await this.QueryAxisPosition(mountOptions, MountAxis.UpDown).ConfigureAwait(false);


			await this.FinaliseInitialisation(mountOptions, MountAxis.LeftRight).ConfigureAwait(false);

			await this.FinaliseInitialisation(mountOptions, MountAxis.UpDown).ConfigureAwait(false);


			await this.QueryStatus(mountOptions, MountAxis.LeftRight).ConfigureAwait(false);

			await this.QueryStatus(mountOptions, MountAxis.UpDown).ConfigureAwait(false);

			// Default break steps
			mountOptions.BreakSteps[(int) MountAxis.LeftRight] = 3500;

			mountOptions.BreakSteps[(int) MountAxis.UpDown] = 3500;

			Device.BeginInvokeOnMainThread(() => this.ConnectedMounts.Add(mountOptions));
		}

		private async Task QueryStatus(IMountInfo mountOptions, MountAxis axis)
		{
			byte[] command = this._commonCommandBuilder.BuildGetStatusCommand(axis);

			byte[] response = await this.SendRecieveCommand(mountOptions.WifiMount, command).ConfigureAwait(false);

			AxisStatus status = this._commonCommandParser.ParseStatusResponse(response);

			mountOptions.AxesStatus[(int) axis] = status;
		}

		private async Task FinaliseInitialisation(IMountInfo mountOptions, MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildFinaliseInitialisationCommand(axis);

			await this.SendCommand(mountOptions.WifiMount, command).ConfigureAwait(false);
		}

		private async Task QueryAxisPosition(IMountInfo mountOptions, MountAxis axis)
		{
			byte[] command = this._commonCommandBuilder.BuildGetAxisPositionCommand(axis);

			byte[] response = await this.SendRecieveCommand(mountOptions.WifiMount, command).ConfigureAwait(false);

			long steps = this._commonCommandParser.ParseAxisPositionResponse(response);

			mountOptions.AxisPositions[(int) axis] = Conversion.StepToAngle(mountOptions.StepCoefficients[(int) axis].FactorStepToRad, steps);
		}

		private async Task QueryHighSpeedRatio(IMountInfo mountOptions, MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildGetHighSpeedRatioCommand(axis);

			byte[] response = await this.SendRecieveCommand(mountOptions.WifiMount, command).ConfigureAwait(false);

			long ratio = this._commandParser.ParseHighSpeedRatioResponse(response);

			mountOptions.HighSpeedRatio[(int) axis] = ratio;
		}

		private async Task QueryTimerInteruptFrequency(IMountInfo mountOptions, MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildGetTimerInterruptFreqCommand(axis);

			byte[] response = await this.SendRecieveCommand(mountOptions.WifiMount, command).ConfigureAwait(false);

			long timerFreq = this._commandParser.ParseTimerInterruptFreqResponse(response);

			mountOptions.TimerInterruptFrequencies[(int) axis] = timerFreq;

			mountOptions.MotorInterval[(int) axis] = Conversion.CalculateMotorInterval(mountOptions.StepCoefficients[(int) axis].FactorRadToStep, timerFreq);
		}

		private async Task QueryCountsPerRevolution(IMountInfo mountOptions, MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildGetCountsPerRevolutionCommand(axis);

			byte[] response = await this.SendRecieveCommand(mountOptions.WifiMount, command).ConfigureAwait(false);

			long gearRatio = this._commandParser.ParseCountsPerRevolutionRepsonse(response, mountOptions.MountControllerVersion);

			mountOptions.StepCoefficients[(int) axis] = new StepCoefficients(gearRatio);
		}

		private async Task GetMotorBoardVersion(IMountInfo mountOptions)
		{
			byte[] mcVersionCommand = this._commandBuilder.BuildGetMotorBoardVersionCommand(MountAxis.LeftRight);

			byte[] mcVersionResponse = await this.SendRecieveCommand(mountOptions.WifiMount, mcVersionCommand).ConfigureAwait(false);

			mountOptions.MountControllerVersion = this._commandParser.ParseMotorBoardResponse(mcVersionResponse);
		}

		[NotNull]
		private Task ResetRxBuffer(IMountInfo mountOptions, MountAxis axis)
		{
			byte[] rxBuffer = this._commandBuilder.BuildResetRxBufferCommand(axis);

			return this.SendCommand(mountOptions.WifiMount, rxBuffer);
		}

		public async Task SendCommand(WifiMount mount, byte[] command)
		{
			await this._udpClient.SendAsync(command, command.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);

			await Task.Delay(2000);
		}

		public async Task<byte[]> SendRecieveCommand(WifiMount mount, byte[] command)
		{
			int port = this._rand.Next(50101, 65535);

			Debug.WriteLine($"UDP on port {port}");

			using (var udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port)))
			{
				await udpClient.SendAsync(command, command.Length, new IPEndPoint(IPAddress.Parse(mount.Address), mount.Port)).ConfigureAwait(false);

				UdpReceiveResult receiveResult = await udpClient.ReceiveAsync().ConfigureAwait(false);

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