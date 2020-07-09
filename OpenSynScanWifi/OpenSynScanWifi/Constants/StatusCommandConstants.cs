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
}