namespace OpenSynScanWifi.Models
{
	public interface IMountInfo
	{
		WifiMount WifiMount { get; set; }

		StepCoefficients[] StepCoefficients { get; set; }

		long MountControllerVersion { get; set; }

		long MountCode { get; }

		double[] TimerInterruptFrequencies { get; set; }

		double[] MotorInterval { get; set; }

		double[] HighSpeedRatio { get; set; }

		double[] AxisPositions { get; set; }

		AxisStatus[] AxesStatus { get; set; }

		long[] BreakSteps { get; set; }
	}
}