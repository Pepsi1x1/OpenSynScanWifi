using System;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Helpers;

namespace OpenSynScanWifi.Commands
{
	public sealed class MountInitialisationCommandParser
	{
		public long ParseMotorBoardResponse([CanBeNull] string response)
		{
			this.ValidateResponse(response);

			long tmpMCVersion = response.BinaryCodedDecimalToLong();

			return ((tmpMCVersion & 0xFF) << 16) | (tmpMCVersion & 0xFF00) | ((tmpMCVersion & 0xFF0000) >> 16);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		/// <param name="mcVersion">Version number from motor board</param>
		/// <returns>FactorRadToStep coefficient, FactorStepToRad coefficient</returns>
		public (double, double) ParseGridPerRevolutionRepsonse([NotNull] string response, long mcVersion)
		{
			this.ValidateResponse(response);

			long gearRatio;

			// There is a bug in the earlier firmware versions < 2.00 of motor controller MC001.
			// Overwrite the GearRatio reported by the MC for 80GT mount and 114GT mount.
			long modelCode = (mcVersion & 0x0000FF);

			// for 80GT mount
			if (modelCode == 0x80)
			{
				gearRatio = 0x162B97;
			}
			// for 114GT mount
			else if (modelCode == 0x82)
			{
				gearRatio = 0x205318;
			}
			else
			{
				gearRatio = response.BinaryCodedDecimalToLong();
			}

			//TODO: Return the gear ratio and do this utility calculation elsewhere
			double factorRadToStep = gearRatio / (2 * Math.PI);

			double factorStepToRad = 2 * Math.PI / gearRatio;

			return (factorRadToStep, factorStepToRad);
		}

		/// <summary>
		/// Inquire Timer Interrupt Freq ":b1".
		/// </summary>
		/// <param name="response"></param>
		/// <param name="factorRadToStep"></param>
		/// <returns>stepTimerFreq, FactorRadRateToInt</returns>
		public (long, double) ParseTimerInterruptFreqResponse([NotNull] string response, double factorRadToStep)
		{
			this.ValidateResponse(response);

			long timeFreq = response.BinaryCodedDecimalToLong();

			//TODO: Return the timeFreq and do this utility calculation elsewhere
			var factorRadRateToInt = (double) (timeFreq) / factorRadToStep;

			return (timeFreq, factorRadRateToInt);
		}

		public long ParseHighSpeedRatioResponse([NotNull] string response)
		{
			this.ValidateResponse(response);

			long highSpeedRatio = response.BinaryCodedDecimalToLong();

			return highSpeedRatio;
		}

		public double ParseAxisPositionResponse([NotNull] string response, double stepToRadCoefficent)
		{
			this.ValidateResponse(response);

			long steps = response.BinaryCodedDecimalToLong();

			// Special Notes for data related to position:
			// All the position data is offset by 0x800000.
			// For example, axis position (in Counts) 0x000012
			// should be converted to 0x800012 when preparing the command.
			// while the true position of data 0x801234
			// reported by the motor controller is 0x001234. 
			steps -= 0x800000;

			var axisPosition = Conversion.StepToAngle(stepToRadCoefficent, steps);

			return axisPosition;
		}

		private void ValidateResponse(string response)
		{
			if (string.IsNullOrWhiteSpace(response))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(response));
			}
		}

		private void ValidateResponse(byte[] response)
		{
			if (response == null)
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(response));
			}

			if (response.Length == 0)
			{
				throw new ArgumentOutOfRangeException(nameof(response), "Value cannot be empty.");
			}
		}
	}
}