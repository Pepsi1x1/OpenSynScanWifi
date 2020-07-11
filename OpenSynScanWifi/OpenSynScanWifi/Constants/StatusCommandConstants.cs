using System;

namespace OpenSynScanWifi.Commands
{
	[Flags]
	public enum StatusLoBitFlags
	{
		Tracking = 0x01,
		Direction = 0x02,
		Speed = 0x04
	}

	[Flags]
	public enum StatusMidBitFlags
	{
		Running = 0x01,
		Blocked = 0x02
	}

	[Flags]
	public enum StatusHiBitFlags
	{
		InitDone = 0x01,
		LevelSwitch = 0x02
	}

	[Flags]
	public enum MotionModeLoBitFlags
	{
		None = 0,

		/// <summary>
		/// 0=Goto, 1=Tracking 
		/// </summary>
		GotoOrTracking = 0x01,

		/// <summary>
		/// B1: 0=Slow, 1=Fast  (TrackingMode)
		///	    0=Fast, 1=Slow  (GotoMode) 
		/// </summary>
		SlowOrFast = 0x02,

		/// <summary>
		/// B2: 0=S/F, 1=Medium
		/// </summary>
		SFOrMedium = 0x04,

		/// <summary>
		/// B3: 1x Slow Goto
		/// </summary>
		SlowGoto = 0x08,


		GotoModeSlow = SlowOrFast,

		GotoModeFast = None,

		TrackingModeSlow = GotoOrTracking,

		TrackingModeFast = GotoOrTracking | SlowOrFast
	}

	[Flags]
	public enum MotionModeHiBitFlags
	{
		None = 0,

		/// <summary>
		/// B0: 0=CW, 1=CCW
		/// </summary>
		DirectionOnClock = 0x01,

		/// <summary>
		/// B1: 0=North, 1=South 
		/// </summary>
		DirectionPoles = 0x02,

		/// <summary>
		/// 0=Normal Goto, 1=Coarse Goto
		/// </summary>
		NormalOrCoarseGoto = 0x04,
	}
}