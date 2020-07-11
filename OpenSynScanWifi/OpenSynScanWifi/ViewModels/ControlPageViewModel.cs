using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Models;
using Prism.Navigation;

namespace OpenSynScanWifi.ViewModels
{
	public class ControlPageViewModel : ViewModelBase
	{
		[NotNull] private readonly IMountInfo _mountInfo;

		public ControlPageViewModel(
			[NotNull] INavigationService navigationService,
			[NotNull] IMountInfo mountInfo)
			: base(navigationService)
		{
			this._mountInfo = mountInfo;
			Title = "Control Page";
		}
	}
}