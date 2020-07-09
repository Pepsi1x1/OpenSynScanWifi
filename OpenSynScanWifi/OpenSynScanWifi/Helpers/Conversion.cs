using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using OpenSynScanWifi.Annotations;

namespace OpenSynScanWifi.Helpers
{
	public static class Conversion
	{
		[NotNull]
		public static byte[] HexToByteArray([NotNull] string hexString)
		{
			if (string.IsNullOrWhiteSpace(hexString))
			{
				return new byte[1];
			}

			return BigInteger.Parse(hexString, NumberStyles.HexNumber).ToByteArray().Reverse().ToArray();
		}

		/// <summary>
		/// i.e. 020782 => 8521474
		/// </summary>
		/// <param name="str">A Binary coded decimal - hexadecimal string representation</param>
		/// <returns>The decoded long</returns>
		public static long BinaryCodedDecimalToLong(this string str)
		{
			try
			{
				byte[] bi = Conversion.HexToByteArray(str);

				return bi.BinaryCodedDecimalToLong();
			}
			catch (FormatException)
			{
				throw new InvalidOperationException("Parsing BCD Failed");
			}
		}

		/// <summary>
		/// i.e. 020782 => 8521474
		/// </summary>
		/// <param name="bi">A Binary coded decimal - byte array representation</param>
		/// <returns>The decoded long</returns>
		public static long BinaryCodedDecimalToLong(this byte[] bi)
		{
			long value = 0;
			for (int i = 0; i < bi.Length; i++)
			{
				long nibble = bi[i];

				Debug.WriteLine("n" + nibble);

				double pow = Math.Pow(16, i * 2);

				Debug.WriteLine("^" + pow);

				long part = (long) (nibble * pow);

				Debug.WriteLine("p" + part);

				value += part;
			}

			return value;
		}

		public static double StepToAngle(double stepToRadCoefficent, long Steps)
		{
			return Steps * stepToRadCoefficent;
		}
	}
}