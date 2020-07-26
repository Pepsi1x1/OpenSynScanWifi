using System;
using OpenSynScanWifi.Constants;

namespace OpenSynScanWifi.Models
{
	public readonly struct StepCoefficients
	{
		public readonly double FactorRadToStep;

		public readonly double FactorStepToRad;

		public readonly double LowSpeedGotoMargin;

		public StepCoefficients(double gearRatio)
		{
			this.FactorRadToStep = gearRatio / (2 * Math.PI);

			this.FactorStepToRad = 2 * Math.PI / gearRatio;

			// LowSpeedGotoMargin is calculated from slewing for 5 seconds in 128x sidereal rate
			this.LowSpeedGotoMargin = 640 * MountControlConstants.SIDEREAL_RATE * this.FactorRadToStep;
		}

		public StepCoefficients(double factorRadToStep, double factorStepToRad)
		{
			this.FactorRadToStep = factorRadToStep;

			this.FactorStepToRad = factorStepToRad;

			this.LowSpeedGotoMargin = 640 * MountControlConstants.SIDEREAL_RATE * this.FactorRadToStep;
		}

		public override bool Equals(object obj)
		{
			return obj is StepCoefficients other &&
			       Equals(other);
		}

		public bool Equals(StepCoefficients other)
		{
			return this.FactorRadToStep == other.FactorRadToStep &&
			       this.FactorStepToRad == other.FactorStepToRad;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(this.FactorRadToStep, this.FactorStepToRad);
		}

		public void Deconstruct(out double factorRadToStep, out double factorStepToRad)
		{
			factorRadToStep = this.FactorRadToStep;

			factorStepToRad = this.FactorStepToRad;
		}

		public static implicit operator (double factorRadToStep, double factorStepToRad)(StepCoefficients value)
		{
			return (value.FactorRadToStep, value.FactorStepToRad);
		}

		public static implicit operator StepCoefficients((double factorRadToStep, double factorStepToRad) value)
		{
			return new StepCoefficients(value.factorRadToStep, value.factorStepToRad);
		}
	}
}