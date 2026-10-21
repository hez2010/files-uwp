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
			SafetyExtensions.IgnoreExceptions(JumpListCatalog.RefreshAsync, Program.Logger));
	}

	internal static void Stop()
	{
		CatalogWatchers.Dispose();
	}
}
