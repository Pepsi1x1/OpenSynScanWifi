using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using OpenSynScanWifi.Annotations;

namespace OpenSynScanWifi.Helpers
{
	public static class Conversion
	{
		[NotNull]
		public static byte[] HexToByteArray([NotNull] string hexString)
		{
			if (String.IsNullOrWhiteSpace(hexString))
			{
				return new byte[1];
			}

			return BigInteger.Parse(hexString, NumberStyles.HexNumber).ToByteArray().Reverse().ToArray();
		}

		public static double RevolutionsToDegrees(long revolutions)
		{
			return revolutions * 360;
		}

		public static double RevolutionsToDegrees(double revolutions)
		{
			return revolutions * 360;
		}

		public static double DegreesToRevolutions(double degrees)
		{
			return degrees / 360 * Math.Pow(2, 24);
		}

		/// <summary>
		/// i.e. 020782 => 8521474
		/// </summary>
		/// <param name="bi">A Binary coded decimal - byte array representation</param>
		/// <returns>The decoded long</returns>
		public static double BinaryCodedDecimalToDouble(this byte[] bi)
		{
			var hexStr = Encoding.ASCII.GetString(bi);

			Debug.WriteLine(hexStr);

			double retVal = 0;

			for (int i = 0; i + 1 < hexStr.Length; i += 2)
			{
				string complementHex = hexStr.Substring(i, 2);

				int complement = int.Parse(complementHex, System.Globalization.NumberStyles.AllowHexSpecifier);

				Debug.WriteLine("c" + complement);

				double pow = Math.Pow(16, i);

				Debug.WriteLine("^" + pow);

				double part = complement * pow;

				Debug.WriteLine("p" + part);

				retVal += part;
			}

			Console.WriteLine(retVal);

			return retVal;
		}

		public static string ToBinaryCodedDecimal(this double value)
		{
			int a = (int) value & 0xFF;

			int b = (int) ((long) value & 0xFF00) / 256;

			int c = (int) ((long) value & 0xFF0000) / 65536;

			return a.ToString("X2") + b.ToString("X2") + c.ToString("X2");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stepToRadCoefficient"></param>
		/// <param name="steps"></param>
		/// <returns>Angle in radians</returns>
		public static double StepToAngle(double stepToRadCoefficient, double steps)
		{
			return steps * stepToRadCoefficient;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="radToStepCoefficient"></param>
		/// <param name="angle">Angle in radians</param>
		/// <returns></returns>
		public static long AngleToStep(double radToStepCoefficient, double angle)
		{
			return (long) (angle * radToStepCoefficient);
		}

		public static double CalculateMotorInterval(double factorRadToStep, double timerFrequency)
		{
			return timerFrequency / factorRadToStep;
		}
	}
}