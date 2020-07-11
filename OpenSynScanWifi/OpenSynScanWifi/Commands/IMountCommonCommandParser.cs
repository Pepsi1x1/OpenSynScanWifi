using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Commands
{
	public interface IMountCommonCommandParser
	{
		AxisStatus ParseStatusResponse([NotNull] byte[] response);

		AxisStatusEx ParseExtendedStatusResponse([NotNull] byte[] response);

		double ParseAxisPositionResponse([NotNull] byte[] response);
	}
}