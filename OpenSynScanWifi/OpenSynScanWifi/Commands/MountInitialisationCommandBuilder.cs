using System.Text;
using OpenSynScanWifi.Constants;

namespace OpenSynScanWifi.Commands
{
	public sealed class MountControlCommandBuilder : MountCommandBuilder, IMountControlCommandBuilder
	{
		//public byte[] BuildSlewToCommand(MountAxis axis)
		//{
		//	string command = base.Build(axis, ClientCommandSet.COMMAND_GET_MOTOR_BOARD_VERSION);

		//	byte[] rawCommand = Encoding.ASCII.GetBytes(command);

		//	return rawCommand;
		//}
	}

	public interface IMountControlCommandBuilder
	{
	}

	public sealed class MountInitialisationCommandBuilder : MountCommandBuilder, IMountInitialisationCommandBuilder
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

		public byte[] BuildGetCountsPerRevolutionCommand(MountAxis axis)
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
		/// <param name="axis"></param>
		public byte[] BuildGetHighSpeedRatioCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_GET_HIGH_SPEED_RATIO);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="axis"></param>
		public byte[] BuildFinaliseInitialisationCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_FINALISE_INITIALISATION);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}
	}
}