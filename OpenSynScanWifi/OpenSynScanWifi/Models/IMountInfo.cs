namespace OpenSynScanWifi.Models
{
	public interface IMountInfo
	{
		WifiMount WifiMount { get; set; }
		
		MountState MountState { get; set; }
	}
}