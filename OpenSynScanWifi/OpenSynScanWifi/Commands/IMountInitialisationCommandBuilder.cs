using OpenSynScanWifi.Constants;

namespace OpenSynScanWifi.Commands
{
	public interface IMountInitialisationCommandBuilder
	{
		byte[] BuildFinaliseInitialisationCommand(MountAxis axis);

		byte[] BuildGetCountsPerRevolutionCommand(MountAxis axis);

		byte[] BuildGetHighSpeedRatioCommand(MountAxis axis);

		byte[] BuildGetMotorBoardVersionCommand(MountAxis axis);

		byte[] BuildGetTimerInterruptFreqCommand(MountAxis axis);

		byte[] BuildResetRxBufferCommand();
	}
}