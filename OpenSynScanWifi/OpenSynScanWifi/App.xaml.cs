using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using OpenSynScanWifi.Services;
using OpenSynScanWifi.ViewModels;
using OpenSynScanWifi.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace OpenSynScanWifi
{
	public partial class App
	{
		/* 
		 * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
		 * This imposes a limitation in which the App class must have a default constructor. 
		 * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
		 */
		public App() : this(null)
		{
		}

		public App(IPlatformInitializer initializer) : base(initializer, true)
		{
		}

		protected override async void OnInitialized()
		{
			InitializeComponent();

			await NavigationService.NavigateAsync("NavigationPage/MainPage");
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			RegisterServices(containerRegistry);

			containerRegistry.RegisterForNavigation<NavigationPage>();
			containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
			containerRegistry.RegisterForNavigation<ControlPage, ControlPageViewModel>();
		}

		private static void RegisterServices(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

			Random rand = new Random(50000);

			int port = rand.Next(50000, 65535);

			Debug.WriteLine($"UDP on port {port}");

			containerRegistry.RegisterInstance(typeof(UdpClient), new UdpClient(new IPEndPoint(IPAddress.Any, port)));

			containerRegistry.Register<IMountDiscovery, MountDiscovery>();

			containerRegistry.RegisterInstance<IMountOptions>(new MountOptions());

			containerRegistry.Register<IMountControl, MountControl>();
		}
	}
}