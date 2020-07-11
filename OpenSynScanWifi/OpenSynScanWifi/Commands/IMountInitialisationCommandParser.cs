using OpenSynScanWifi.Annotations;

namespace OpenSynScanWifi.Commands
{
	public interface IMountInitialisationCommandParser
	{
		double ParseCountsPerRevolutionRepsonse([NotNull] byte[] response, long mcVersion);

		double ParseHighSpeedRatioResponse([NotNull] byte[] response);

		long ParseMotorBoardResponse([CanBeNull] byte[] response);

		double ParseTimerInterruptFreqResponse([NotNull] byte[] response);
	}
}