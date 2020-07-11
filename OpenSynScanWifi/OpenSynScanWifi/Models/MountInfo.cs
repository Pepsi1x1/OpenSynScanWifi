namespace OpenSynScanWifi.Models
{
	public class MountInfo : IMountInfo
	{
		public WifiMount WifiMount { get; set; }

		public MountState MountState { get; set; }
	}
}