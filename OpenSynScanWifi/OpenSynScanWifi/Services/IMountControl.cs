using System.Threading.Tasks;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Commands;
using OpenSynScanWifi.Constants;
using OpenSynScanWifi.Models;

namespace OpenSynScanWifi.Services
{
	public interface IMountControl
	{
		Task QueryStatus(MountAxis axis);
		Task SetAxisInstantStop(MountAxis axis);
		Task SetAxisStop(MountAxis axis);
		Task SetBreakPointIncrement(MountAxis axis, double stepsCount);
		Task SetBreakSteps(MountAxis axis, double breakSteps);
		Task SetGotoTargetIncrement(MountAxis axis, double stepsCount);
		Task SetMotionMode(MountAxis axis, MotionModeLoBitFlags func, MotionModeHiBitFlags direction);
		void SetMountInfo([NotNull] IMountInfo mountInfo);
		Task SetStepPeriod(MountAxis axis, double stepsCount);
		Task StartMotion(MountAxis axis);
	}
}