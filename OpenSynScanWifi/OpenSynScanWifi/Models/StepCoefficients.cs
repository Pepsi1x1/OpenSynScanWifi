﻿using System;
using OpenSynScanWifi.Constants;

namespace OpenSynScanWifi.Models
{
	public struct StepCoefficients
	{
		public double FactorRadToStep;

		public double FactorStepToRad;

		public double LowSpeedGotoMargin;

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
			       this.FactorRadToStep == other.FactorRadToStep &&
			       this.FactorStepToRad == other.FactorStepToRad;
		}

		public override int GetHashCode()
		{
			int hashCode = 1322887786;

			hashCode = hashCode * -1521134295 + this.FactorRadToStep.GetHashCode();

			hashCode = hashCode * -1521134295 + this.FactorStepToRad.GetHashCode();

			return hashCode;
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