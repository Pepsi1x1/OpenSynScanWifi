using System;
using OpenSynScanWifi.Annotations;
using OpenSynScanWifi.Constants;
using OpenSynScanWifi.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OpenSynScanWifi.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ControlPage : ContentPage
	{
		[CanBeNull] private readonly IMountMotion _mountMotion = DependencyService.Resolve<IMountMotion>();

		public ControlPage()
		{
			InitializeComponent();
		}

		public async void OnStopButtonPressed(object sender, EventArgs e)
		{
			await this._mountMotion.Stop(MountAxis.UpDown).ConfigureAwait(false);
			await this._mountMotion.Stop(MountAxis.LeftRight).ConfigureAwait(false);
		}

		private async void OnUpButtonPressed(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Up", true);
			await this._mountMotion.SlewAxisAsync(MountAxis.UpDown, 10).ConfigureAwait(false);
		}

		public async void OnUpButtonReleased(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Up", false);
			await this._mountMotion.Stop(MountAxis.UpDown).ConfigureAwait(false);
		}

		private async void OnDownButtonPressed(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Down", true);
			await this._mountMotion.SlewAxisAsync(MountAxis.UpDown, -10).ConfigureAwait(false);
		}

		public async void OnDownButtonReleased(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Down", false);
			await this._mountMotion.Stop(MountAxis.UpDown).ConfigureAwait(false);
		}

		private async void OnLeftButtonPressed(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Left", true);
			await this._mountMotion.SlewAxisAsync(MountAxis.LeftRight, -10).ConfigureAwait(false);
		}

		public async void OnLeftButtonReleased(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Left", false);
			await this._mountMotion.Stop(MountAxis.UpDown).ConfigureAwait(false);
		}

		private async void OnRightButtonPressed(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Right", true);
			await this._mountMotion.SlewAxisAsync(MountAxis.LeftRight, 10).ConfigureAwait(false);
		}

		public async void OnRightButtonReleased(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Right", false);
			await this._mountMotion.Stop(MountAxis.UpDown).ConfigureAwait(false);
		}
	}
}