using System.Text;
using OpenSynScanWifi.Constants;

namespace OpenSynScanWifi.Commands
{
	public sealed class MountCommonCommandBuilder : MountCommandBuilder, IMountCommonCommandBuilder
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="axis"></param>
		public byte[] BuildGetAxisPositionCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_GET_AXIS_POSITION);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="axis"></param>
		public byte[] BuildGetStatusCommand(MountAxis axis)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_GET_STATUS);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="axis"></param>
		public byte[] BuildGetStatusExCommand(MountAxis axis, string parameters)
		{
			string command = base.Build(axis, ClientCommandSet.COMMAND_GET_STATUS_EX);

			byte[] rawCommand = Encoding.ASCII.GetBytes(command);

			return rawCommand;
		}
	}
}