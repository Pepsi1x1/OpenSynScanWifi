using System;
using System.Net.Sockets;
using System.Threading;
using OpenSynScanWifi.Emulator.Services;

namespace OpenSynScanWifi.Emulator
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			using (UdpClient udpClient = new UdpClient(11880))
			{
				Server server = new Server(udpClient);
				server.Listen(new CancellationTokenSource().Token);
				for (;;)
				{
				}
			}
		}
	}
}