namespace OpenSynScanWifi.Models
{
	public sealed class AxisStatus
	{
		public bool FullStop { get; set; }

		public bool Tracking { get; set; }

		public bool Goto { get; set; }

		public bool SlewingClockWise { get; set; }

		public bool HighSpeed { get; set; }

		public bool NotInitialized { get; set; }

		public bool LevelSwitchOn { get; set; }

		public bool Blocked { get; set; }
	}

	public sealed class AxisStatusEx
	{
	}
}