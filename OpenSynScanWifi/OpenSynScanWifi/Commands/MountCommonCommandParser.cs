using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Helpers;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Commands
{
	public sealed class MountCommonCommandParser : MountCommandParserBase, IMountCommonCommandParser
	{
		public double ParseAxisPositionResponse([NotNull] byte[] response)
		{
			base.ValidateResponse(response);

			response = base.StripGrammar(response);

			double steps = response.BinaryCodedDecimalToDouble();

			// Special Notes for data related to position:
			// All the position data is offset by 0x800000.
			// For example, axis position (in Counts) 0x000012
			// should be converted to 0x800012 when preparing the command.
			// while the true position of data 0x801234
			// reported by the motor controller is 0x001234. 
			steps -= 0x800000;

			return steps;
		}

		public AxisStatus ParseStatusResponse(byte[] response)
		{
			response = base.StripGrammar(response);

			var axisStatus = new AxisStatus();

			if ((response[1] & (int) StatusMidBitFlags.Running) == 0)
			{
				axisStatus.FullStop = true;
				axisStatus.Tracking = false;
				axisStatus.Goto = false;
			}
			else
			{
				// Axis is running
				axisStatus.FullStop = false;

				if ((response[0] & (int) StatusLoBitFlags.Tracking) == 0)
				{
					// Axis in Goto mode.
					axisStatus.Goto = true;
					axisStatus.Tracking = false;
				}
				else
				{
					// Axis in slewing(AstroMisc speed) mode.
					axisStatus.Tracking = true;
					axisStatus.Goto = false;
				}
			}

			if ((response[1] & (int) StatusMidBitFlags.Blocked) == 0)
			{
				axisStatus.Blocked = false;
			}
			else
			{
				axisStatus.Blocked = true;
			}

			if ((response[0] & (int) StatusLoBitFlags.Direction) == 0)
			{
				axisStatus.SlewingClockWise = true;
			}
			else
			{
				axisStatus.SlewingClockWise = false;
			}

			if ((response[0] & (int) StatusLoBitFlags.Speed) == 0)
			{
				axisStatus.HighSpeed = false;
			}
			else
			{
				axisStatus.HighSpeed = true;
			}

			if ((response[2] & (int) StatusHiBitFlags.InitDone) == 0)
			{
				axisStatus.NotInitialized = true;
			}
			else
			{
				axisStatus.NotInitialized = false;
			}

			if ((response[2] & (int) StatusHiBitFlags.LevelSwitch) == 1)
			{
				axisStatus.LevelSwitchOn = true;
			}
			else
			{
				axisStatus.NotInitialized = false;
			}

			return axisStatus;
		}

		public AxisStatusEx ParseExtendedStatusResponse([NotNull] byte[] response)
		{
			response = base.StripGrammar(response);

			return new AxisStatusEx();
		}
	}
}