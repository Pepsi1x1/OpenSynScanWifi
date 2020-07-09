using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Commands
{
	public interface IMountCommonCommandParser
	{
		AxisStatus ParseStatusResponse([NotNull] byte[] response);

		long ParseAxisPositionResponse([NotNull] byte[] response);
	}
}