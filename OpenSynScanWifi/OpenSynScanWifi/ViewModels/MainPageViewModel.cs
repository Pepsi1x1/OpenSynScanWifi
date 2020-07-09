using System.Threading;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Constants;
using OpenSynScanWifi.Services;
using Prism.Commands;
using Prism.Navigation;

namespace OpenSynScanWifi.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
		[CanBeNull] public WifiMount SelectedMount { get; set; }

		[NotNull] public IAsyncCommand RestartDiscoveryAsync { get; }

		[NotNull] public IAsyncCommand FindMountsAsync { get; }

		[NotNull] public IMountDiscovery MountDiscovery { get; private set; }

		[NotNull] private CancellationTokenSource _mountDiscoveryCancellationTokenSource;

		[NotNull] private readonly IMountOptions _mountOptions;

		public DelegateCommand SelectionChangedCommand { get; }

		public MainPageViewModel(
			[NotNull] INavigationService navigationService,
			[NotNull] IMountDiscovery mountDiscovery,
			[NotNull] IMountOptions mountOptions)
			: base(navigationService)
		{
			Title = "Main Page";


			this.MountDiscovery = mountDiscovery;

			this._mountOptions = mountOptions;

			FindMountsAsync = new AsyncCommand(ExecuteFindMountsAsync);

			RestartDiscoveryAsync = new AsyncCommand(ExecuteRestartDiscoveryAsync);

			SelectionChangedCommand = new DelegateCommand(this.OnSelectionChangedCommandExecuted);
		}

		/// <summary>Called when the implementer has been navigated to.</summary>
		/// <param name="parameters">The navigation parameters.</param>
		public override async void OnNavigatedTo(INavigationParameters parameters)
		{
			this._mountDiscoveryCancellationTokenSource = new CancellationTokenSource();

			await this.MountDiscovery.DiscoverAsync(this._mountDiscoveryCancellationTokenSource.Token).ConfigureAwait(false);

			base.OnNavigatedTo(parameters);
		}

		/// <summary>
		/// Called when the implementer has been navigated away from.
		/// </summary>
		/// <param name="parameters">The navigation parameters.</param>
		public override void OnNavigatedFrom(INavigationParameters parameters)
		{
			this._mountDiscoveryCancellationTokenSource.Cancel();

			base.OnNavigatedFrom(parameters);
		}

		private void OnSelectionChangedCommandExecuted()
		{
			this._mountOptions.WifiMount = SelectedMount;

			if (SelectedMount != null)
			{
				base.NavigationService.NavigateAsync(NavigationConstants.CONTROL_PAGE);
			}
		}

		private Task ExecuteFindMountsAsync()
		{
			return this.MountDiscovery.FindAsync();
		}

		private Task ExecuteRestartDiscoveryAsync()
		{
			this.MountDiscovery.DeviceIpEndPoints.Clear();

			return Task.CompletedTask;
		}
	}
}