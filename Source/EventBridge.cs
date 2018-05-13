using System;
using System.Reflection;
using Harmony;
using RimWorld;
using Verse;

namespace Rimchallenge
{
	public static class EventBridge
    {
		private static HarmonyInstance harmony = HarmonyInstance.Create("rimworld.undera4.challenge");

		private static void patch(Type type, string srcName, string preName, string postName) {
			MethodBase srcMethod = AccessTools.Method(type, srcName);
			HarmonyMethod preMethod = preName != null ?new HarmonyMethod(typeof(EventBridge).GetMethod(preName)):null;
			HarmonyMethod postMethod = postName != null ? new HarmonyMethod(typeof(EventBridge).GetMethod(postName)) : null;           
			harmony.Patch(srcMethod, preMethod, postMethod);
		}

        public static void Hook(Controller modL)
        {
			patch(typeof(Map), "FinalizeInit", null, "OnMapLoaded");

			patch(typeof(Pawn), "Kill", null, "OnPawnKilled");
			patch(typeof(Pawn), "Destroy", null, "OnPawnDestroyed");
			patch(typeof(Pawn), "SetFaction", null, "OnPawnFactionSet");

			patch(typeof(Mineable), "DestroyMined", null, "OnDestroyMined");
			patch(typeof(Mineable), "Destroy", null, "OnDestroyMined");

			patch(typeof(SkillRecord), "Learn", null, "OnSkillLearned");
		}

		public static void OnMapLoaded()
        {
			ChallengeDef.GenerateNonOverlappingCoordinates();
            // TODO load saved status
        }

		public static void OnPawnKilled(Pawn __instance, DamageInfo dinfo)
		{
			ChallengeManager.instance.currentChallenge.OnPawnKilled(__instance, dinfo);
		}

		public static void OnPawnFactionSet(Pawn __instance)
        {
			ChallengeManager.instance.currentChallenge.OnPawnFactionSet(__instance);
        }

		public static void OnPawnDestroyed(Pawn __instance)
        {
            ChallengeManager.instance.currentChallenge.OnPawnDestroyed(__instance);
        }

		public static void OnDestroyMined(Mineable __instance)
        {
			ChallengeManager.instance.currentChallenge.OnDestroyMined(__instance);
        }

		public static void SkillLearned(SkillRecord __instance)
        {
            Log.Message("Skill learned " + __instance);
            ChallengeManager.instance.currentChallenge.OnSkillLearned(__instance);
        }
}
}