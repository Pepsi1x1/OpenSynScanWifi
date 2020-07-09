using System;
using System.Runtime.Serialization;

namespace OpenSynScanWifi.Exceptions
{
	[Serializable]
	public class WifiException : Exception
	{
		public WifiException()
		{
		}

		public WifiException(string message) : base(message)
		{
		}

		public WifiException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected WifiException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}