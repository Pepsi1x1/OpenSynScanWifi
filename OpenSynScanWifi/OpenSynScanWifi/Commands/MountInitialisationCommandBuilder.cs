using System.Text;
using OpenSynScanWifi.Constants;

namespace OpenSynScanWifi.Commands
{
	public sealed class MountControlCommandBuilder : MountCommandBuilder, IMountControlCommandBuilder
	{
		public byte[] BuildSetStepPeriodCommand(MountAxis axis, string parameters)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_SET_STEP_PERIOD, parameters);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		public byte[] BuildSetAxisStopCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_SET_AXIS_STOP);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		public byte[] BuildSetAxisInstantStopCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_SET_AXIS_INSTANT_STOP);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		public byte[] BuildSetMotionModeCommand(MountAxis axis, string parameters)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_SET_MOTION_MODE, parameters);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		public byte[] BuildSetStartMotionCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_SET_START_MOTION);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		public byte[] BuildSetGotoTargetIncrementCommand(MountAxis axis, string parameters)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_SET_GOTO_TARGET_INCREMENT, parameters);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		public byte[] BuildSetBreakPointIncrementCommand(MountAxis axis, string parameters)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_SET_BREAK_POINT_INCREMENT, parameters);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		public byte[] BuildSetBreakStepsCommand(MountAxis axis, string parameters)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_SET_BREAK_STEPS, parameters);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}
	}

	public interface IMountControlCommandBuilder
	{
		byte[] BuildSetAxisInstantStopCommand(MountAxis axis);
		byte[] BuildSetAxisStopCommand(MountAxis axis);
		byte[] BuildSetBreakPointIncrementCommand(MountAxis axis, string parameters);
		byte[] BuildSetBreakStepsCommand(MountAxis axis, string parameters);
		byte[] BuildSetGotoTargetIncrementCommand(MountAxis axis, string parameters);
		byte[] BuildSetMotionModeCommand(MountAxis axis, string parameters);
		byte[] BuildSetStartMotionCommand(MountAxis axis);
		byte[] BuildSetStepPeriodCommand(MountAxis axis, string parameters);
	}

	public sealed class MountInitialisationCommandBuilder : MountCommandBuilder, IMountInitialisationCommandBuilder
	{
		public byte[] BuildResetRxBufferCommand()
		{
			string command = $"{ClientCommandSet.COMMAND_START}{ClientCommandSet.COMMAND_TERMINATOR}";

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