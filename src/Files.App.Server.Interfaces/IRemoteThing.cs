using Windows.Foundation;

namespace Files.App.Server.Interfaces;

public interface IRemoteThing
{
	IAsyncOperation<int> RemAsync(int a, int b);
}
