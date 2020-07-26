using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using OpenSynScanWifi.Commands;
using OpenSynScanWifi.Helpers;
using OpenSynScanWifi.Models;
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

		internal static Queue<int> AvailablePorts = new Queue<int>( Enumerable.Range(50101, 100));

		private static void RegisterServices(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

			containerRegistry.RegisterInstance<IObjectPool<UdpClient>>(new ObjectPool<UdpClient>(() =>
			{
				Random rng = new Random();
				int nextUpdPort = AvailablePorts.Dequeue();
				
				UdpClient client;
				try
				{
					client = new UdpClient(new IPEndPoint(IPAddress.Any, nextUpdPort));
				}
				catch (SocketException)
				{
					nextUpdPort = AvailablePorts.Dequeue();
					client = new UdpClient(new IPEndPoint(IPAddress.Any, nextUpdPort));
				}

				Debug.WriteLine($"Generated new UDP client at port {nextUpdPort}");

				return client;
			}, 50));

			containerRegistry.Register<IMountInitialisationCommandBuilder, MountInitialisationCommandBuilder>();

			containerRegistry.Register<IMountCommonCommandBuilder, MountCommonCommandBuilder>();

			containerRegistry.Register<IMountControlCommandBuilder, MountControlCommandBuilder>();

			containerRegistry.Register<IMountInitialisationCommandParser, MountInitialisationCommandParser>();

			containerRegistry.Register<IMountCommonCommandParser, MountCommonCommandParser>();

			containerRegistry.Register<IMountControlCommandParser, MountControlCommandParser>();

			containerRegistry.Register<IMountDiscovery, MountDiscovery>();

			containerRegistry.RegisterSingleton<IMountInfo, MountInfo>();

			containerRegistry.Register<IMountMotion, MountMotion>();

			containerRegistry.RegisterInstance<IMountOptions>(new MountOptions());

			containerRegistry.Register<IMountControl, MountControl>();
		}
	}
}