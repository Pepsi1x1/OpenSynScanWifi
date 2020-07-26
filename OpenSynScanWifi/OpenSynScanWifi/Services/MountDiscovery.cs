using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
	public sealed class MountDiscovery : MountControllerBase, IMountDiscovery
	{
		public ObservableCollection<IMountInfo> ConnectedMounts { get; private set; } = new ObservableCollection<IMountInfo>();

		private readonly List<string> _discoveredMounts = new List<string>();

		[NotNull] private readonly IMountInitialisationCommandBuilder _commandBuilder;

		[NotNull] private readonly IMountInitialisationCommandParser _commandParser;

		[NotNull] private readonly UdpClient _discoveryClient;

		public MountDiscovery([NotNull] IObjectPool<UdpClient> udpClientPool,
			[NotNull] IMountInitialisationCommandBuilder commandBuilder,
			[NotNull] IMountInitialisationCommandParser commandParser,
			[NotNull] IMountCommonCommandBuilder commonCommandBuilder,
			[NotNull] IMountCommonCommandParser commonCommandParser) : base(commonCommandBuilder, commonCommandParser, udpClientPool)
		{
			this._commandBuilder = commandBuilder;

			this._commandParser = commandParser;

			this._discoveryClient = udpClientPool.Get();
		}

		public Task DiscoverAsync(CancellationToken cancellationToken)
		{
			this.ConnectedMounts.Clear();

			Task.Run(async () =>
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					UdpReceiveResult receiveResult = await this._discoveryClient.ReceiveAsync().ConfigureAwait(false);

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

						MountInfo mountInfo = new MountInfo() {WifiMount = wifiMount};

						Task.Run(async () => { await this.HandshakeAsync(mountInfo).ConfigureAwait(false); });
					}

#if DEBUG
					Array.ForEach(buffer.ToArray(), b => Debug.Write($"{b}, "));
#endif

					Debug.WriteLine("");
				}
			}, cancellationToken);

			return Task.CompletedTask;
		}

		public void ClearClients()
		{
			this.ConnectedMounts.Clear();
			this._discoveredMounts.Clear();
		}

		public async Task FindAsync()
		{
			this._discoveryClient.EnableBroadcast = true;

			await this._discoveryClient.SendAsync(WifiConstants.CMSG_DISCOVERY_DATAGRAM, WifiConstants.CMSG_DISCOVERY_DATAGRAM.Length, new IPEndPoint(IPAddress.Parse("192.168.4.255"), 11880)).ConfigureAwait(false);

			Debug.WriteLine("Sent DISCOVERY1");
		}

		public async Task HandshakeAsync(IMountInfo mountInfo)
		{
			mountInfo.MountState = new MountState();

			await this.QueryExtendedStatus(mountInfo, MountAxis.LeftRight).ConfigureAwait(false);


			await this.ResetRxBuffer(mountInfo).ConfigureAwait(false);


			await this.GetMotorBoardVersion(mountInfo).ConfigureAwait(false);


			await this.QueryCountsPerRevolution(mountInfo, MountAxis.LeftRight).ConfigureAwait(false);

			await this.QueryCountsPerRevolution(mountInfo, MountAxis.UpDown).ConfigureAwait(false);


			await this.QueryTimerInteruptFrequency(mountInfo, MountAxis.LeftRight).ConfigureAwait(false);

			await this.QueryTimerInteruptFrequency(mountInfo, MountAxis.UpDown).ConfigureAwait(false);


			await this.QueryHighSpeedRatio(mountInfo, MountAxis.LeftRight).ConfigureAwait(false);

			await this.QueryHighSpeedRatio(mountInfo, MountAxis.UpDown).ConfigureAwait(false);


			await this.QueryAxisPosition(mountInfo, MountAxis.LeftRight).ConfigureAwait(false);

			await this.QueryAxisPosition(mountInfo, MountAxis.UpDown).ConfigureAwait(false);


			await this.FinaliseInitialisation(mountInfo, MountAxis.LeftRight).ConfigureAwait(false);

			await this.FinaliseInitialisation(mountInfo, MountAxis.UpDown).ConfigureAwait(false);


			await this.QueryStatus(mountInfo, MountAxis.LeftRight).ConfigureAwait(false);

			await this.QueryStatus(mountInfo, MountAxis.UpDown).ConfigureAwait(false);

			// Default break steps
			mountInfo.MountState.BreakSteps[(int) MountAxis.LeftRight] = 3500;

			mountInfo.MountState.BreakSteps[(int) MountAxis.UpDown] = 3500;

			Device.BeginInvokeOnMainThread(() => this.ConnectedMounts.Add(mountInfo));
		}

		private async Task QueryExtendedStatus(IMountInfo mountInfo, MountAxis axis)
		{
			byte[] command = this._commonCommandBuilder.BuildGetStatusExCommand(axis, "010000");

			byte[] response = await this.SendReceiveCommandAsync(mountInfo.WifiMount, command).ConfigureAwait(false);

			if (!base.ValidateResponse(response))
			{
				return;
			}

			AxisStatusEx statusEx = this._commonCommandParser.ParseExtendedStatusResponse(response);

			mountInfo.MountState.AxesStatusExtended[(int) axis] = statusEx;
		}

		private async Task QueryStatus(IMountInfo mountInfo, MountAxis axis)
		{
			byte[] command = this._commonCommandBuilder.BuildGetStatusCommand(axis);

			byte[] response = await this.SendReceiveCommandAsync(mountInfo.WifiMount, command).ConfigureAwait(false);

			if (!base.ValidateResponse(response))
			{
				return;
			}

			if (response.Length != 5)
			{
				Debug.WriteLine($"Got malformed response to status query {response.Length} bytes");
				return;
			}

			AxisStatus status = this._commonCommandParser.ParseStatusResponse(response);

			mountInfo.MountState.AxesStatus[(int) axis] = status;
		}

		private async Task FinaliseInitialisation(IMountInfo mountInfo, MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildFinaliseInitialisationCommand(axis);

			await this.SendCommandAsync(mountInfo.WifiMount, command).ConfigureAwait(false);
		}

		private async Task QueryAxisPosition(IMountInfo mountInfo, MountAxis axis)
		{
			byte[] command = this._commonCommandBuilder.BuildGetAxisPositionCommand(axis);

			byte[] response = await this.SendReceiveCommandAsync(mountInfo.WifiMount, command).ConfigureAwait(false);

			if (!base.ValidateResponse(response))
			{
				return;
			}

			double steps = this._commonCommandParser.ParseAxisPositionResponse(response);

			mountInfo.MountState.AxisPositions[(int) axis] = Conversion.StepToAngle(mountInfo.MountState.StepCoefficients[(int) axis].FactorStepToRad, steps);

			mountInfo.MountState.AxisSexagesimalAngles[(int) axis] = SexagesimalAngle.FromDouble(Maths.RadToDeg(mountInfo.MountState.AxisPositions[(int) axis]));
		}

		private async Task QueryHighSpeedRatio(IMountInfo mountInfo, MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildGetHighSpeedRatioCommand(axis);

			byte[] response = await this.SendReceiveCommandAsync(mountInfo.WifiMount, command).ConfigureAwait(false);

			if (!base.ValidateResponse(response))
			{
				return;
			}

			double ratio = this._commandParser.ParseHighSpeedRatioResponse(response);

			mountInfo.MountState.HighSpeedRatio[(int) axis] = ratio;
		}

		private async Task QueryTimerInteruptFrequency(IMountInfo mountInfo, MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildGetTimerInterruptFreqCommand(axis);

			byte[] response = await this.SendReceiveCommandAsync(mountInfo.WifiMount, command).ConfigureAwait(false);

			if (!base.ValidateResponse(response))
			{
				return;
			}

			double timerFreq = this._commandParser.ParseTimerInterruptFreqResponse(response);

			mountInfo.MountState.TimerInterruptFrequencies[(int) axis] = timerFreq;

			mountInfo.MountState.MotorInterval[(int) axis] = Conversion.CalculateMotorInterval(mountInfo.MountState.StepCoefficients[(int) axis].FactorRadToStep, timerFreq);
		}

		private async Task QueryCountsPerRevolution(IMountInfo mountInfo, MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildGetCountsPerRevolutionCommand(axis);

			byte[] response = await this.SendReceiveCommandAsync(mountInfo.WifiMount, command).ConfigureAwait(false);

			if (!base.ValidateResponse(response))
			{
				return;
			}

			double gearRatio = this._commandParser.ParseCountsPerRevolutionRepsonse(response, mountInfo.MountState.MountControllerVersion);

			mountInfo.MountState.StepCoefficients[(int) axis] = new StepCoefficients(gearRatio);
		}

		private async Task GetMotorBoardVersion(IMountInfo mountInfo)
		{
			byte[] mcVersionCommand = this._commandBuilder.BuildGetMotorBoardVersionCommand(MountAxis.LeftRight);

			byte[] mcVersionResponse = await this.SendReceiveCommandAsync(mountInfo.WifiMount, mcVersionCommand).ConfigureAwait(false);

			if (!base.ValidateResponse(mcVersionResponse))
			{
				await this.GetMotorBoardVersion(mountInfo).ConfigureAwait(false);

				return;
			}

			mountInfo.MountState.MountControllerVersion = this._commandParser.ParseMotorBoardResponse(mcVersionResponse);
		}

		[NotNull]
		private Task ResetRxBuffer(IMountInfo mountInfo)
		{
			byte[] rxBuffer = this._commandBuilder.BuildResetRxBufferCommand();

			return this.SendCommandAsync(mountInfo.WifiMount, rxBuffer);
		}
	}
}