using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Commands;
using OpenSynScanWifi.Constants;
using OpenSynScanWifi.Helpers;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Services
{
	public class MountControl : MountControllerBase, IMountControl
	{
		[NotNull] private IMountInfo _mountInfo;

		[NotNull] private readonly IMountControlCommandBuilder _commandBuilder;

		[NotNull] private readonly IMountControlCommandParser _commandParser;

		public MountControl([NotNull] IObjectPool<UdpClient> udpClientPool,
			[NotNull] IMountControlCommandBuilder commandBuilder,
			[NotNull] IMountControlCommandParser commandParser,
			[NotNull] IMountCommonCommandBuilder commonCommandBuilder,
			[NotNull] IMountCommonCommandParser commonCommandParser,
			[NotNull] IMountInfo mountInfo) : base(commonCommandBuilder, commonCommandParser, udpClientPool)
		{
			this._commandBuilder = commandBuilder;

			this._commandParser = commandParser;

			this._mountInfo = mountInfo;
		}

		public void SetMountInfo([NotNull] IMountInfo mountInfo)
		{
			if (mountInfo is null)
			{
				throw new ArgumentNullException(nameof(mountInfo), "Value cannot be null");
			}

			this._mountInfo = mountInfo;
		}

		public async Task QueryStatus(MountAxis axis)
		{
			byte[] command = this._commonCommandBuilder.BuildGetStatusCommand(axis);

			byte[] response = await this.SendReceiveCommandAsync(this._mountInfo.WifiMount, command).ConfigureAwait(false);

			if (!base.ValidateResponse(response))
			{
				return;
			}

			if (response.Length != 5)
			{
				Debug.WriteLine($"Got malformed response to status query {response.Length} bytes");

				this._mountInfo.MountState.AxesStatus[(int) axis] ??= new AxisStatus();

				return;
			}

			AxisStatus status = this._commonCommandParser.ParseStatusResponse(response);

			this._mountInfo.MountState.AxesStatus[(int) axis] = status;
		}

		public async Task SetAxisStop(MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildSetAxisStopCommand(axis);

			await this.SendCommandAsync(this._mountInfo.WifiMount, command).ConfigureAwait(false);

			this._mountInfo.MountState.AxesStatus[(int) axis].FullStop = true;
		}

		public async Task SetAxisInstantStop(MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildSetAxisInstantStopCommand(axis);

			await this.SendCommandAsync(this._mountInfo.WifiMount, command).ConfigureAwait(false);

			this._mountInfo.MountState.AxesStatus[(int) axis].FullStop = true;
		}

		public Task SetMotionMode(MountAxis axis, MotionModeLoBitFlags func, MotionModeHiBitFlags direction)
		{
			string szCmd = "" + (int) func + (int) direction;

			byte[] command = this._commandBuilder.BuildSetMotionModeCommand(axis, szCmd);

			return this.SendCommandAsync(this._mountInfo.WifiMount, command);
		}

		public Task SetStepPeriod(MountAxis axis, double stepsCount)
		{
			string szCmd = stepsCount.ToBinaryCodedDecimal();

			byte[] command = this._commandBuilder.BuildSetStepPeriodCommand(axis, szCmd);

			return this.SendCommandAsync(this._mountInfo.WifiMount, command);
		}

		public Task StartMotion(MountAxis axis)
		{
			byte[] command = this._commandBuilder.BuildSetStartMotionCommand(axis);

			return this.SendCommandAsync(this._mountInfo.WifiMount, command);
		}

		public Task SetGotoTargetIncrement(MountAxis axis, double stepsCount)
		{
			string szCmd = stepsCount.ToBinaryCodedDecimal();

			byte[] command = this._commandBuilder.BuildSetGotoTargetIncrementCommand(axis, szCmd);

			return this.SendCommandAsync(this._mountInfo.WifiMount, command);
		}

		public Task SetBreakPointIncrement(MountAxis axis, double stepsCount)
		{
			string szCmd = stepsCount.ToBinaryCodedDecimal();

			byte[] command = this._commandBuilder.BuildSetBreakPointIncrementCommand(axis, szCmd);

			return this.SendCommandAsync(this._mountInfo.WifiMount, command);
		}

		public Task SetBreakSteps(MountAxis axis, double breakSteps)
		{
			string szCmd = breakSteps.ToBinaryCodedDecimal();

			byte[] command = this._commandBuilder.BuildSetBreakStepsCommand(axis, szCmd);

			return this.SendCommandAsync(this._mountInfo.WifiMount, command);
		}
	}
}