using System;
using System.Reflection;
using Harmony;
using Verse;

namespace Rimchallenge
{
	public static class EventBridge
    {
		private static HarmonyInstance harmony = HarmonyInstance.Create("rimworld.undera4.challenge");
		private static ModLoader modLoader;

		private static void patch(Type type, string srcName, string preName, string postName) {
			MethodBase srcMethod = AccessTools.Method(type, srcName);
			HarmonyMethod preMethod = preName != null ?new HarmonyMethod(typeof(EventBridge).GetMethod(preName)):null;
			HarmonyMethod postMethod = postName != null ? new HarmonyMethod(typeof(EventBridge).GetMethod(postName)) : null;           
			harmony.Patch(srcMethod, preMethod, postMethod);
		}

        public static void Hook(ModLoader modL)
        {
            modLoader = modL;

			patch(typeof(Map), "FinalizeInit", null, "OnMapLoaded");

			patch(typeof(Pawn), "Kill", null, "OnPawnKilled");
			patch(typeof(Pawn), "SetFaction", null, "OnPawnFactionSet");
		}

		public static void OnMapLoaded()
        {
			ChallengeDef.GenerateNonOverlappingCoordinates();
            // TODO load saved status
        }

		public static void OnPawnKilled(Pawn __instance)
		{
			Log.Message("Pawn Killed "+__instance);
			modLoader.currentChallenge.OnPawnKilled(__instance);
		}

		public static void OnPawnFactionSet(Pawn __instance)
        {
            Log.Message("Pawn Faction Set " + __instance);
            modLoader.currentChallenge.OnPawnFactionSet(__instance);
        }
	}
}