using System.Text;

namespace OpenSynScanWifi.Commands
{
	public sealed class MountInitialisationCommandBuilder : MountCommandBuilder
	{
		public byte[] BuildResetRxBufferCommand(MountAxis axis)
		{
			string command = base.Build(axis, "");

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		public byte[] BuildGetMotorBoardVersionCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_GET_MOTOR_BOARD_VERSION);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		public byte[] BuildGetGridPerRevolutionCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_GET_GRID_PER_REVOLUTION);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		public byte[] BuildGetTimerInterruptFreqCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_GET_TIMER_INTERRUPT_FREQ);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		/// <summary>
		/// Inquire high speed ratio ":g(*2)", where *2: '1'= CH1, '2' = CH2.
		/// </summary>
		/// <param name="Axis"></param>
		public byte[] BuildGetHighSpeedRatioCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_GET_HIGH_SPEED_RATIO);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Axis"></param>
		public byte[] BuildGetAxisPositionCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_GET_AXIS_POSITION);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Axis"></param>
		public byte[] BuildFinaliseInitialisationCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_FINALISE_INITIALISATION);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Axis"></param>
		public byte[] BuildGetStatusCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_GET_STATUS);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}
	}
}