using UIKit;

namespace OpenSynScanWifi.iOS
{
#pragma warning disable S1118 // Utility classes should not have public constructors - False positive
	public class Application
#pragma warning restore S1118 // Utility classes should not have public constructors
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}