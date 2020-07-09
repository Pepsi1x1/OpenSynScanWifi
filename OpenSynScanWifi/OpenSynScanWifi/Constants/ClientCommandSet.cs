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

		public const string COMMAND_FINALISE_INITIALISATION = "F";

		public const string COMMAND_GET_STATUS = "f";

		public const string COMMAND_SET_MOTION_MODE = "G";

		public const string COMMAND_SET_GOTO_TARGET_INCREMENT = "H";

		public const string COMMAND_SET_BREAK_POINT_INCREMENT = "M";

		public const string COMMAND_SET_BREAK_STEPS = "U";

		public const string COMMAND_SET_STEP_PERIOD = "I";

		public const string COMMAND_SET_START_MOTION = "J";
	}
}