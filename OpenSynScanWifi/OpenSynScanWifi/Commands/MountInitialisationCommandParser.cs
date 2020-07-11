using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Helpers;

namespace OpenSynScanWifi.Commands
{
	public sealed class MountInitialisationCommandParser : MountCommandParserBase, IMountInitialisationCommandParser
	{
		public long ParseMotorBoardResponse([CanBeNull] byte[] response)
		{
			base.ValidateResponse(response);

			response = base.StripGrammar(response);

			long tmpMCVersion = (long) response.BinaryCodedDecimalToDouble();

			return ((tmpMCVersion & 0xFF) << 16) | (tmpMCVersion & 0xFF00) | ((tmpMCVersion & 0xFF0000) >> 16);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		/// <param name="mcVersion">Version number from motor board</param>
		/// <returns>FactorRadToStep coefficient, FactorStepToRad coefficient</returns>
		public double ParseCountsPerRevolutionRepsonse([NotNull] byte[] response, long mcVersion)
		{
			base.ValidateResponse(response);

			response = base.StripGrammar(response);

			double gearRatio;

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
				gearRatio = response.BinaryCodedDecimalToDouble();
			}

			return gearRatio;
		}

		/// <summary>
		/// Inquire Timer Interrupt Freq ":b1".
		/// </summary>
		/// <param name="response"></param>
		/// <param name="factorRadToStep"></param>
		/// <returns>stepTimerFreq</returns>
		public double ParseTimerInterruptFreqResponse([NotNull] byte[] response)
		{
			base.ValidateResponse(response);

			response = base.StripGrammar(response);

			double timeFreq = response.BinaryCodedDecimalToDouble();

			return timeFreq;
		}

		public double ParseHighSpeedRatioResponse([NotNull] byte[] response)
		{
			base.ValidateResponse(response);

			response = base.StripGrammar(response);

			double highSpeedRatio = response.BinaryCodedDecimalToDouble();

			return highSpeedRatio;
		}
	}
}