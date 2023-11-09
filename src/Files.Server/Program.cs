using Files.Server.Interfaces;
using Shmuelie.WinRTServer;
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Files.Server;

class Program
{
	static async Task Main(string[] args)
	{
		await using (WinRtServer server = new WinRtServer())
		{
			server.RegisterClass<FileOperations>();
			server.Start();
			await server.WaitForFirstObjectAsync();
		}
	}
}

public sealed class FileOperations : IFileOperations
{
	public IAsyncOperation<Result> RunAsync(int a, int b, TypedEventHandler<IFileOperations, ProgressData> progress)
	{
		return AsyncInfo.Run(async c =>
		{
			for (var i = a; i < b; i++)
			{
				progress(this, new(i));
			}
			return Result.Success;
		});
	}
}