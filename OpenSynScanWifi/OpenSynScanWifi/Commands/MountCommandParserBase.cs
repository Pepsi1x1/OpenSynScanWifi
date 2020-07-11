using System;
using ImTools;
using OpenSynScanWifi.Constants;
using OpenSynScanWifi.Exceptions;

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

			if (response[0] == ServerCommandSet.COMMAND_ERROR_START_BYTE)
			{
				throw new MountErrorReponseException();
			}
		}

		public byte[] StripGrammar(byte[] response)
		{
			//Don't need to check length as RemoveAt will return the source array
			//if operations are out of bounds or the array is null/empty

			byte[] bytes = response;
			if (response[0] == ServerCommandSet.COMMAND_START_BYTE)
			{
				bytes = response.RemoveAt(0);
			}

			if (bytes[bytes.Length - 1] == ServerCommandSet.COMMAND_TERMINATOR_BYTE)
			{
				bytes = bytes.RemoveAt(bytes.Length - 1);
			}

			return bytes;
		}
	}
}