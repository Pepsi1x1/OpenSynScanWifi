using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Commands
{
	public interface IMountInitialisationCommandParser
	{
		long ParseCountsPerRevolutionRepsonse([NotNull] byte[] response, long mcVersion);

		long ParseHighSpeedRatioResponse([NotNull] byte[] response);

		long ParseMotorBoardResponse([CanBeNull] byte[] response);

		long ParseTimerInterruptFreqResponse([NotNull] byte[] response);
	}
}