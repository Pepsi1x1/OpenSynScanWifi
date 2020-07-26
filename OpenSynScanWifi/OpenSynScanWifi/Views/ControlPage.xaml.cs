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

		private void OnStopButtonPressed(object sender, EventArgs e)
		{
			this._mountMotion.Stop(MountAxis.UpDown).GetAwaiter().GetResult();
			this._mountMotion.Stop(MountAxis.LeftRight).GetAwaiter().GetResult();
		}

		private void OnUpButtonPressed(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Up", true);
			this._mountMotion.SlewAxisAsync(MountAxis.UpDown, 10).GetAwaiter().GetResult();
		}

		private void OnUpButtonReleased(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Up", false);
			this._mountMotion.Stop(MountAxis.UpDown).GetAwaiter().GetResult();
		}

		private void OnDownButtonPressed(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Down", true);
			this._mountMotion.SlewAxisAsync(MountAxis.UpDown, -10).GetAwaiter().GetResult();
		}

		private void OnDownButtonReleased(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Down", false);
			this._mountMotion.Stop(MountAxis.UpDown).GetAwaiter().GetResult();
		}

		private void OnLeftButtonPressed(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Left", true);
			this._mountMotion.SlewAxisAsync(MountAxis.LeftRight, -10).GetAwaiter().GetResult();
		}

		private void OnLeftButtonReleased(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Left", false);
			this._mountMotion.Stop(MountAxis.LeftRight).GetAwaiter().GetResult();
		}

		private void OnRightButtonPressed(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Right", true);
			this._mountMotion.SlewAxisAsync(MountAxis.LeftRight, 10).GetAwaiter().GetResult();
		}

		private void OnRightButtonReleased(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "Right", false);
			this._mountMotion.Stop(MountAxis.LeftRight).GetAwaiter().GetResult();
		}
	}
}