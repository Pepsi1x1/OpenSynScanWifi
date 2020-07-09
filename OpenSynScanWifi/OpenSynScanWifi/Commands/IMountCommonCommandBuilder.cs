using OpenSynScanWifi.Constants;

namespace OpenSynScanWifi.Commands
{
	public interface IMountCommonCommandBuilder
	{
		byte[] BuildGetAxisPositionCommand(MountAxis axis);

		byte[] BuildGetStatusCommand(MountAxis axis);
	}
}