using System;

namespace OpenSynScanWifi.Constants
{
	public static class MountControlConstants
	{
		public const double SIDEREAL_RATE = 2 * Math.PI / 86164.09065;

		public const double INTERNAL_SIDEREAL_RATE = SIDEREAL_RATE / 1000;

		public const double LOW_SPEED_MARGIN = (128.0 * SIDEREAL_RATE);

		public const double MAX_SPEED = 500;
	}
}