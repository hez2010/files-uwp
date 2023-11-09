using Windows.Foundation;

namespace Files.App.Server.Interfaces;

public sealed class ProgressData
{
	public ProgressData(int progress)
	{
		Progress = progress;
	}

	public int Progress { get; }
}

public enum Result
{
	Success, Failed, Cancelled
}

public interface IFileOperations
{
	IAsyncOperation<Result> RunAsync(int a, int b, TypedEventHandler<IFileOperations, ProgressData> progress);
}
