using System;

namespace OpenSynScanWifi.Commands
{
	public class MountCommandParserBase
	{
		private void ValidateResponse(string response)
		{
			if (string.IsNullOrWhiteSpace(response))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(response));
			}
		}

		public void ValidateResponse(byte[] response)
		{
			if (response == null)
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(response));
			}

			if (response.Length == 0)
			{
				throw new ArgumentOutOfRangeException(nameof(response), "Value cannot be empty.");
			}
		}
	}
}