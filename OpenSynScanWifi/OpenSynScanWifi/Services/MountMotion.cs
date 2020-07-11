using System;
using System.Threading;
using System.Threading.Tasks;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Commands;
using OpenSynScanWifi.Constants;
using OpenSynScanWifi.Helpers;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Services
{
	public class MountMotion : IMountMotion
	{
		[NotNull] private readonly IMountControl _mountControl;

		[NotNull] private readonly IMountInfo _mountInfo;

		public MountMotion([NotNull] IMountControl mountControl, [NotNull] IMountInfo mountInfo)
		{
			this._mountControl = mountControl;

			this._mountInfo = mountInfo;
		}

		[NotNull]
		public Task Stop(MountAxis axis)
		{
			return this._mountControl.SetAxisStop(MountAxis.UpDown);
		}

		private async Task PrepareForSlewingAsync(MountAxis axis, double speed, double absoluteSpeed)
		{
			await this._mountControl.QueryStatus(axis).ConfigureAwait(false);

			await this.StopAxisIfRequiredAsync(axis, speed, absoluteSpeed).ConfigureAwait(false);

			MotionModeHiBitFlags directionOnClock = MotionModeHiBitFlags.None;
			if (speed < 0.0)
			{
				directionOnClock = MotionModeHiBitFlags.DirectionOnClock;
			}
			else
			{
				//
			}

			if (absoluteSpeed > MountControlConstants.LOW_SPEED_MARGIN)
			{
				// Set HIGH speed slewing mode.
				await this._mountControl.SetMotionMode(axis, MotionModeLoBitFlags.TrackingModeFast, directionOnClock).ConfigureAwait(false);
			}
			else
			{
				// Set LOW speed slewing mode.
				await this._mountControl.SetMotionMode(axis, MotionModeLoBitFlags.TrackingModeSlow, directionOnClock).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="speed"></param>
		/// <returns>If stopping was required</returns>
		private async Task<bool> StopAxisIfRequiredAsync(MountAxis axis, double speed, double absoluteSpeed)
		{
			AxisStatus mountInfoAxisStatus = this._mountInfo.MountState.AxesStatus[(int) axis];

			if (!mountInfoAxisStatus.FullStop)
			{
				if (!mountInfoAxisStatus.Tracking)
				{
					await this._mountControl.SetAxisStop(axis).ConfigureAwait(false);
				}
				else if (mountInfoAxisStatus.HighSpeed)
				{
					await this._mountControl.SetAxisStop(axis).ConfigureAwait(false);
				}
				// Moving to ludicrous speed
				else if (absoluteSpeed >= MountControlConstants.LOW_SPEED_MARGIN)
				{
					await this._mountControl.SetAxisStop(axis).ConfigureAwait(false);
				}
				// Changing direction
				else if (mountInfoAxisStatus.SlewingClockWise && speed < 0)
				{
					await this._mountControl.SetAxisStop(axis).ConfigureAwait(false);
				}
				// Changing direction
				else if (mountInfoAxisStatus.SlewingClockWise && speed > 0)
				{
					await this._mountControl.SetAxisStop(axis).ConfigureAwait(false);
				}
				else
				{
					// All checks passed, safe to set motion mode
					return false;
				}

				// Wait until the axis stops
				while (true)
				{
					await this._mountControl.QueryStatus(axis).ConfigureAwait(false);

					// Return if the axis has stopped.
					if (mountInfoAxisStatus.FullStop)
					{
						break;
					}

					Thread.Sleep(100);
				}
			}

			return true;
		}

		public async Task SlewAxisAsync(MountAxis axis, double speed)
		{
			// Limit maximum speed
			if (speed > MountControlConstants.MAX_SPEED) // 3.4 degrees/sec, 800X sidereal rate, is the highest speed.
			{
				speed = MountControlConstants.MAX_SPEED;
			}
			else if (speed < -MountControlConstants.MAX_SPEED)
			{
				speed = -MountControlConstants.MAX_SPEED;
			}

			double absoluteSpeed = Math.Abs(speed);

			if (absoluteSpeed <= MountControlConstants.INTERNAL_SIDEREAL_RATE)
			{
				await this._mountControl.SetAxisStop(axis).ConfigureAwait(false);

				return;
			}

			// The motor has to be stopped to set the motion mode
			await this.PrepareForSlewingAsync(axis, speed, absoluteSpeed).ConfigureAwait(false);

			if (speed > 0.0)
			{
				this._mountInfo.MountState.AxesStatus[(int) axis].SlewingClockWise = true;
			}
			else
			{
				this._mountInfo.MountState.AxesStatus[(int) axis].SlewingClockWise = false;
			}

			double internalSpeed = absoluteSpeed;

			if (internalSpeed > MountControlConstants.LOW_SPEED_MARGIN)
			{
				// High speed adjustment
				internalSpeed = internalSpeed / this._mountInfo.MountState.HighSpeedRatio[(int) axis];

				this._mountInfo.MountState.AxesStatus[(int) axis].HighSpeed = true;
			}

			double speedInSecondsPerRadian = 1 / internalSpeed;

			double motorInterval = Conversion.CalculateMotorInterval(this._mountInfo.MountState.StepCoefficients[(int) axis].FactorRadToStep, speedInSecondsPerRadian);

			// For special MC version.
			if ((this._mountInfo.MountState.MountControllerVersion == 0x010600)
			    || (this._mountInfo.MountState.MountControllerVersion == 0x010601))
			{
				motorInterval -= 3;
			}

			motorInterval = Math.Max(motorInterval, 1);

			await this._mountControl.SetStepPeriod(axis, motorInterval).ConfigureAwait(false);

			// Let's goooo
			await this._mountControl.StartMotion(axis).ConfigureAwait(false);
		}

		public async Task SlewAxisToAsync(MountAxis axis, double targetPosition)
		{
			// Update our stored axes
			await this._mountControl.QueryStatus(axis).ConfigureAwait(false);

			var deltaAngle = targetPosition - this._mountInfo.MountState.AxisPositions[(int) axis];

			var stepsToMove = Conversion.AngleToStep(this._mountInfo.MountState.StepCoefficients[(int) axis].FactorRadToStep, deltaAngle);

			// Nothing to do
			if (stepsToMove == 0)
			{
				return;
			}

			MotionModeHiBitFlags directionOnClock;
			if (stepsToMove > 0)
			{
				directionOnClock = MotionModeHiBitFlags.None;
				this._mountInfo.MountState.AxesStatus[(int) axis].SlewingClockWise = true;
			}
			else
			{
				directionOnClock = MotionModeHiBitFlags.DirectionOnClock;
				this._mountInfo.MountState.AxesStatus[(int) axis].SlewingClockWise = false;
			}

			stepsToMove = Math.Abs(stepsToMove);

			// Check if it's far enough that we should go at ludicrous speed
			if (stepsToMove > this._mountInfo.MountState.StepCoefficients[(int) axis].LowSpeedGotoMargin)
			{
				await this._mountControl.SetMotionMode(axis, MotionModeLoBitFlags.GotoModeFast, directionOnClock).ConfigureAwait(false);
				this._mountInfo.MountState.AxesStatus[(int) axis].HighSpeed = true;
			}
			else
			{
				await this._mountControl.SetMotionMode(axis, MotionModeLoBitFlags.GotoModeSlow, directionOnClock).ConfigureAwait(false);
				this._mountInfo.MountState.AxesStatus[(int) axis].HighSpeed = false;
			}

			await this._mountControl.SetGotoTargetIncrement(axis, stepsToMove).ConfigureAwait(false);

			await this._mountControl.SetBreakPointIncrement(axis, this._mountInfo.MountState.BreakSteps[(int) axis]).ConfigureAwait(false);

			await this._mountControl.StartMotion(axis).ConfigureAwait(false);
		}
	}
}