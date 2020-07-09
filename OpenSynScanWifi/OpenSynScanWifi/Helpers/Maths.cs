using System;

namespace OpenSynScanWifi.Helpers
{
	public static class Maths
	{
		public const double RAD1 = Math.PI / 180;

		public static double AngleDistance(double angle1, double angle2)
		{
			angle1 = UniformAngle(angle1);

			angle2 = UniformAngle(angle2);

			double delta = angle2 - angle1;

			return UniformAngle(delta);
		}

		public static double UniformAngle(double source)
		{
			source = source % (Math.PI * 2);

			if (source > Math.PI)
			{
				return source - 2 * Math.PI;
			}

			if (source < -Math.PI)
			{
				return source + 2 * Math.PI;
			}

			return source;
		}

		public static double DegToRad(double degree) => (degree / 180 * Math.PI);

		public static double RadToDeg(double rad) => (rad / Math.PI * 180.0);

		public static double RadToMin(double rad) => (rad / Math.PI * 180.0 * 60.0);

		public static double RadToSec(double rad) => (rad / Math.PI * 180.0 * 60.0 * 60.0);
	}
}