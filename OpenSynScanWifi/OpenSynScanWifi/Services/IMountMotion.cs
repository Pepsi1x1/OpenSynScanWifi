using System.Threading.Tasks;
using OpenSynScanWifi.Constants;

namespace OpenSynScanWifi.Services
{
	public interface IMountMotion
	{
		Task SlewAxisAsync(MountAxis axis, double speed);

		Task SlewAxisToAsync(MountAxis axis, double targetPosition);

		Task Stop(MountAxis axis);
	}
}