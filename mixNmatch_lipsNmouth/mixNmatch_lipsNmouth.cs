using HarmonyLib;
using NeosModLoader;
using FrooxEngine.CommonAvatar;

namespace mixNmatch_lipsNmouth
{
	public class mixNmatch_lipsNmouth : NeosMod
	{
		public override string Name => "mixNmatch_lipsNmouth";
		public override string Author => "dfgHiatus";
		public override string Version => "1.0.1";
		public override string Link => "https://github.com/dfgHiatus/mixNmatch_lipsNmouth";
		public override void OnEngineInit()
		{
			// Harmony.DEBUG = true;
			Harmony harmony = new Harmony("net.dfgHiatus.mixNmatch_lipsNmouth");
			harmony.PatchAll();
		}

		// Fix Issue 3440 (Can't mix and match the Eye Tracker with SRAnipal Lip)
		[HarmonyPatch(typeof(AvatarEyeDataSourceAssigner), "OnEquip")]
		public class AvatarEyeDataSourceAssignerPatch
		{
			public bool Prefix(AvatarEyeDataSourceAssigner __instance, AvatarObjectSlot slot)
			{
				if (__instance.TargetReference.Target == null)
					return false;
				AvatarEyeTrackingInfo rawEyeData = null;
				slot.Slot.ActiveUserRoot.ForeachRegisteredComponent<AvatarEyeTrackingInfo>(val => {
					if (val.EyeDataSource.Target?.IsEyeTrackingActive ?? false)
						rawEyeData = val;
				});
				__instance.TargetReference.Target.Target = rawEyeData?.EyeDataSource.Target;
				return false;
			}
		}
	}
}
