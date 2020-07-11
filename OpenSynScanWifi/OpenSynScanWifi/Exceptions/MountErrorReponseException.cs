using System;
using System.Runtime.Serialization;

namespace OpenSynScanWifi.Exceptions
{
	[Serializable]
	public class MountErrorReponseException : Exception
	{
		public MountErrorReponseException()
		{
		}

		public MountErrorReponseException(string message) : base(message)
		{
		}

		public MountErrorReponseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected MountErrorReponseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}