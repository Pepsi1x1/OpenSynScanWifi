using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Helpers;

namespace OpenSynScanWifi.Commands
{
	public sealed class MountControlCommandParser : MountCommandParserBase, IMountControlCommandParser
	{

	}

	public interface IMountControlCommandParser
	{
	}

	public sealed class MountInitialisationCommandParser : MountCommandParserBase, IMountInitialisationCommandParser
	{
		public long ParseMotorBoardResponse([CanBeNull] byte[] response)
		{
			base.ValidateResponse(response);

			long tmpMCVersion = response.BinaryCodedDecimalToLong();

			return ((tmpMCVersion & 0xFF) << 16) | (tmpMCVersion & 0xFF00) | ((tmpMCVersion & 0xFF0000) >> 16);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		/// <param name="mcVersion">Version number from motor board</param>
		/// <returns>FactorRadToStep coefficient, FactorStepToRad coefficient</returns>
		public long ParseCountsPerRevolutionRepsonse([NotNull] byte[] response, long mcVersion)
		{
			base.ValidateResponse(response);

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

			return gearRatio;
		}

		/// <summary>
		/// Inquire Timer Interrupt Freq ":b1".
		/// </summary>
		/// <param name="response"></param>
		/// <param name="factorRadToStep"></param>
		/// <returns>stepTimerFreq</returns>
		public long ParseTimerInterruptFreqResponse([NotNull] byte[] response)
		{
			base.ValidateResponse(response);

			long timeFreq = response.BinaryCodedDecimalToLong();

			return timeFreq;
		}

		public long ParseHighSpeedRatioResponse([NotNull] byte[] response)
		{
			base.ValidateResponse(response);

			long highSpeedRatio = response.BinaryCodedDecimalToLong();

			return highSpeedRatio;
		}
	}
}