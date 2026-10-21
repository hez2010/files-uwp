// Copyright (c) Files Community
// Licensed under the MIT License.

namespace Files.App.Server;

internal static class StartupServices
{
	internal static async Task InitializeAsync()
	{
		CatalogWatchers.Initialize();
		await Task.WhenAll(
			SafetyExtensions.IgnoreExceptions(InitializationCache.InitializeAsync, Program.Logger),
			SafetyExtensions.IgnoreExceptions(LocationCatalog.RecentFiles.GetAsync, Program.Logger),
			SafetyExtensions.IgnoreExceptions(LocationCatalog.RecentFolders.GetAsync, Program.Logger),
			SafetyExtensions.IgnoreExceptions(LocationCatalog.NetworkShortcuts.GetAsync, Program.Logger));
	}

	internal static void Stop()
	{
		CatalogWatchers.Dispose();
	}
}
