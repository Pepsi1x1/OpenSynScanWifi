using OpenSynScanWifi.Annotations;

namespace OpenSynScanWifi.Commands
{
	public class MountCommandBuilder
	{
		public string Build(MountAxis axis, string command, [NotNull] string parameters = "")
		{
			string commandStr = $"{ClientCommandSet.COMMAND_START}{command}{(int) axis}{parameters}{ClientCommandSet.COMMAND_TERMINATOR}";

			return commandStr;
		}
	}
}