using System.Threading;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Constants;
using OpenSynScanWifi.Models;
using OpenSynScanWifi.Services;
using Prism.Navigation;

namespace OpenSynScanWifi.ViewModels
{
	public class ControlPageViewModel : ViewModelBase
	{
		public IAsyncCommand EchoCommand { get; }

		[NotNull] private readonly IMountControl _mountControl;

		[NotNull] private CancellationTokenSource _mountControlCancellationTokenSource;

		[NotNull] private IMountInfo _mountInfo;

		public ControlPageViewModel(
			[NotNull] INavigationService navigationService,
			[NotNull] IMountControl mountControl)
			: base(navigationService)
		{
			Title = "Control Page";

			this._mountControl = mountControl;

			this.EchoCommand = new AsyncCommand(this.OnEchoCommandExecutedAsync);
		}

		/// <summary>Called when the implementer has been navigated to.</summary>
		/// <param name="parameters">The navigation parameters.</param>
		public override async void OnNavigatedTo(INavigationParameters parameters)
		{
			if (parameters.ContainsKey(NavigationConstants.MOUNT_INFO_NAV_PARAM))
			{
				this._mountInfo = (IMountInfo)parameters[NavigationConstants.MOUNT_INFO_NAV_PARAM];
			}
			this._mountControlCancellationTokenSource = new CancellationTokenSource();

			await this._mountControl.ListenAsync(this._mountControlCancellationTokenSource.Token).ConfigureAwait(false);

			base.OnNavigatedTo(parameters);
		}

		/// <summary>
		/// Called when the implementer has been navigated away from.
		/// </summary>
		/// <param name="parameters">The navigation parameters.</param>
		public override void OnNavigatedFrom(INavigationParameters parameters)
		{
			this._mountControlCancellationTokenSource.Cancel();

			base.OnNavigatedFrom(parameters);
		}

		private Task OnEchoCommandExecutedAsync()
		{
			return this._mountControl.EchoAsync();
		}
	}
}