// Copyright (c) Files Community
// Licensed under the MIT License.

namespace Files.App.Server;

internal static class StartupServices
{
	internal static async Task InitializeAsync()
	{
		await Task.WhenAll(
			SafetyExtensions.IgnoreExceptions(InitializationCache.InitializeAsync, Program.Logger),
			WarmDrivesAsync());
	}

	private static async Task WarmDrivesAsync()
	{
		var ids = await SafetyExtensions.IgnoreExceptions(DriveCatalog.GetIdsAsync, Program.Logger);
		foreach (var id in ids ?? Array.Empty<string>())
			_ = SafetyExtensions.IgnoreExceptions(() => DriveCatalog.GetAsync(id), Program.Logger);
	}

	internal static void Stop()
	{
		DriveCatalog.Stop();
	}
}
