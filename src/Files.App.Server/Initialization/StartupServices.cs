// Copyright (c) Files Community
// Licensed under the MIT License.

namespace Files.App.Server;

internal static class StartupServices
{
	private static readonly Lazy<Task> menuInitialization = new(() => SafetyExtensions.IgnoreExceptions(ContextMenu.WarmUpQueryContextMenuAsync, Program.Logger));

	internal static async Task InitializeAsync()
	{
		await Task.WhenAll(
			SafetyExtensions.IgnoreExceptions(InitializationCache.InitializeAsync, Program.Logger),
			menuInitialization.Value);
	}
}
