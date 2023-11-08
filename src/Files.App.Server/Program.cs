using Files.App.Server.Interfaces;
using Shmuelie.WinRTServer;
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Files.App.Server;

class Program
{
	static async Task Main(string[] args)
	{
		await using (WinRtServer server = new WinRtServer())
		{
			server.RegisterClass<RemoteThing>();
			server.Start();
			await server.WaitForFirstObjectAsync();
		}
	}
}

public sealed class RemoteThing : IRemoteThing
{
	public IAsyncOperation<int> RemAsync(int a, int b)
	{
		return AsyncInfo.Run(async c =>
		{
			await Task.Delay(1000);
			return Math.DivRem(a, b, out var _);
		});
	}
}