// Copyright (c) Files Community
// Licensed under the MIT License.

using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;

namespace Files.App.Server;

public sealed partial class AppService : IDisposable
{
	private readonly long clientId;
	private int disposed;

	public AppService(int processId)
	{
		clientId = ServerHost.AddClient(processId);
	}

	~AppService() => Dispose();

	public event EventHandler<string> Changed
	{
		add => ServerEvents.Add(clientId, value);
		remove => ServerEvents.Remove(clientId, value);
	}

	public static int CloudProviderGroupCount => InitializationCache.CloudProviders.Length;

	public void SetWindowHandle(long handle) => ServerHost.SetWindow(clientId, handle);

	public static IAsyncAction InitializeAsync() => ServerAsync.Run(token => StartupServices.InitializeAsync().WaitAsync(token));

	public static IAsyncOperation<IReadOnlyList<ShellNewEntryInfo>> GetShellNewEntriesAsync() => ServerAsync.Run(token => InitializationCache.ShellNew.GetAsync().WaitAsync(token));

	public static IAsyncOperation<ShellNewEntryInfo?> GetShellNewEntryAsync(string extension) => ServerAsync.Run(async token =>
	{
		var entry = await Task.Run(() => ShellNewMenuHelper.GetNewContextMenuEntryForType(extension), token);
		return entry is null ? null : new ShellNewEntryInfo(entry);
	});

	public static IAsyncOperation<IReadOnlyList<string>> GetWslDistributionsAsync() => ServerAsync.Run(token => InitializationCache.Wsl.GetAsync().WaitAsync(token));

	public static IAsyncOperation<IReadOnlyList<LibraryInfo>> GetLibrariesAsync() => ServerAsync.Run(token => InitializationCache.Libraries.GetAsync().WaitAsync(token));

	public static IAsyncOperation<IReadOnlyList<PinnedFolderInfo>> GetPinnedFoldersAsync() => ServerAsync.Run(token => InitializationCache.PinnedFolders.GetAsync().WaitAsync(token));

	public static void InvalidatePinnedFolders()
	{
		InitializationCache.PinnedFolders.Invalidate();
		ServerEvents.Notify(ServerChangeNames.PinnedFolders);
	}

	public static IAsyncOperation<IReadOnlyList<CloudProviderInfo>> GetCloudProvidersAsync(int group)
	{
		if ((uint)group >= InitializationCache.CloudProviders.Length)
			throw new ArgumentOutOfRangeException(nameof(group));
		return ServerAsync.Run(token => InitializationCache.CloudProviders[group].GetAsync().WaitAsync(token));
	}

	public static IAsyncOperation<bool> CheckReleaseNotesAsync() => ServerAsync.Run(token => UpdateCatalog.ReleaseNotes.GetAsync().WaitAsync(token));

	public static IAsyncOperation<bool> CheckSideloadUpdatesAsync() => ServerAsync.Run(token => UpdateCatalog.SideloadUpdate.GetAsync().WaitAsync(token));

	public static IAsyncOperation<IReadOnlyList<Windows.Services.Store.StorePackageUpdate>> GetStoreUpdatesAsync() => ServerAsync.Run(token => UpdateCatalog.StoreUpdates.GetAsync().WaitAsync(token));

	public static IAsyncAction UpdateLauncherAsync() => ServerAsync.Run(token => UpdateCatalog.UpdateLauncherAsync().WaitAsync(token));

	public static void InvalidateUpdates() => UpdateCatalog.Invalidate();

	public static void BeginUpdate() => ServerHost.BeginUpdate();

	public static void EndUpdate() => ServerHost.EndUpdate();

	public void Dispose()
	{
		if (Interlocked.Exchange(ref disposed, 1) == 0)
			ServerHost.RemoveClient(clientId);
		GC.SuppressFinalize(this);
	}
}
