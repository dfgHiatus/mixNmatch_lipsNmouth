using HarmonyLib;
using NeosModLoader;
using FrooxEngine.CommonAvatar;

namespace mixNmatch_lipsNmouth
{
	public class mixNmatch_lipsNmouth : NeosMod
	{
		public override string Name => "mixNmatch_lipsNmouth";
		public override string Author => "dfgHiatus";
		public override string Version => "1.0.0";
		public override string Link => "https://github.com/dfgHiatus/https://github.com/dfgHiatus/mixNmatch_lipsNmouth/";
		public override void OnEngineInit()
		{
			// Harmony.DEBUG = true;
			Harmony harmony = new Harmony("net.dfgHiatus.mixNmatch_lipsNmouth");
			harmony.PatchAll();
		}

		// Fix Issue 3440 (Can't mix and match the Eye Tracker with SRAnipal Lip)
		[HarmonyPatch(typeof(AvatarEyeDataSourceAssigner))]
		public class AvatarEyeDataSourceAssignerPatch
		{
			public void Postfix(AvatarEyeDataSourceAssigner _aedsa, AvatarObjectSlot slot)
			{
				if (_aedsa.TargetReference.Target == null)
					return;
				AvatarEyeTrackingInfo rawEyeData = null;
				slot.Slot.ActiveUserRoot.ForeachRegisteredComponent<AvatarEyeTrackingInfo>(val => {
					if (val.EyeDataSource.Target?.IsEyeTrackingActive ?? false)
						rawEyeData = val;
				});
				_aedsa.TargetReference.Target.Target = rawEyeData?.EyeDataSource.Target;
			}
		}
	}
}