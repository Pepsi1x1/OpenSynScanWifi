﻿namespace OpenSynScanWifi.Models
{
	internal class MountInfo : IMountInfo
	{
		public WifiMount WifiMount { get; set; }

		public StepCoefficients[] StepCoefficients { get; set; } = new StepCoefficients[3];

		private long _mountControllerVersion;

		public long MountControllerVersion
		{
			get => this._mountControllerVersion;
			set
			{
				this._mountControllerVersion = value;
				this.MountCode = value & 0xFF;
			}
		}

		public long MountCode { get; private set; }

		public double[] TimerInterruptFrequencies { get; set; } = new double[3];

		public double[] MotorInterval { get; set; } = new double[3];

		public double[] AxisPositions { get; set; } = new double[3];

		public double[] HighSpeedRatio { get; set; } = new double[3];

		public AxisStatus[] AxesStatus { get; set; } = new AxisStatus[3];

		public long[] BreakSteps { get; set; } = new long[3];
	}
}