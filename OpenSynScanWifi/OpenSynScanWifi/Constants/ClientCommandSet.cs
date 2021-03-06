﻿using System.Text;

namespace OpenSynScanWifi.Constants
{
	public static class ClientCommandSet
	{
		public const string COMMAND_START = ":";

		public const string COMMAND_TERMINATOR = "\r";

		public const string COMMAND_GET_MOTOR_BOARD_VERSION = "e";

		public const string COMMAND_GET_GRID_PER_REVOLUTION = "a";

		public const string COMMAND_GET_TIMER_INTERRUPT_FREQ = "b";

		public const string COMMAND_GET_HIGH_SPEED_RATIO = "g";

		public const string COMMAND_GET_AXIS_POSITION = "j";

		public const string COMMAND_GET_PEC_PERIOD = "s";

		public const string COMMAND_GET_STATUS_EX = "q";

		public const string COMMAND_FINALISE_INITIALISATION = "F";

		public const string COMMAND_GET_STATUS = "f";

		public const string COMMAND_SET_MOTION_MODE = "G";

		public const string COMMAND_SET_GOTO_TARGET_INCREMENT = "H";

		public const string COMMAND_SET_BREAK_POINT_INCREMENT = "M";

		public const string COMMAND_SET_BREAK_STEPS = "U";

		public const string COMMAND_SET_STEP_PERIOD = "I";

		public const string COMMAND_SET_START_MOTION = "J";

		public const string COMMAND_SET_AXIS_STOP = "K";

		public const string COMMAND_SET_AXIS_INSTANT_STOP = "L";

		public const string COMMAND_SET_AUTOGUIDE_SPEED = "P";
	}

	public static class ServerCommandSet
	{
		public const string COMMAND_START = "=";

		public static readonly byte COMMAND_START_BYTE = Encoding.ASCII.GetBytes(COMMAND_START)[0];

		public const string COMMAND_ERROR_START = "!";

		public static readonly byte COMMAND_ERROR_START_BYTE = Encoding.ASCII.GetBytes(COMMAND_ERROR_START)[0];

		public const string COMMAND_TERMINATOR = "\r";

		public static readonly byte COMMAND_TERMINATOR_BYTE = Encoding.ASCII.GetBytes(COMMAND_TERMINATOR)[0];
	}
}