// Copyright FirePlume, All Rights Reserved. Email: fireplume@126.com

using UnrealBuildTool;

public class FPSpawnerSystemEditor : ModuleRules
{
	public FPSpawnerSystemEditor(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

		bUsePrecompiled = true;
		PrecompileForTargets = PrecompileTargetsType.None;

		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
			}
			);

		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				"CoreUObject",
				"Engine",
				"Slate",
				"SlateCore",
				"InputCore",
				"UnrealEd",
				"PhysicsCore",
				"NavigationSystem",
				"PropertyEditor",
				"FPSpawnerSystem",
			}
			);

	}
}
