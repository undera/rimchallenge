using System.Reflection;
using Harmony;
using Verse;

namespace Rimchallenge
{
	public static class EventBridge
    {
		private static ModLoader modLoader;

        public static void Hook(ModLoader modL)
        {
            modLoader = modL;

			HarmonyInstance harmony = HarmonyInstance.Create("rimworld.undera4.challenge");

			MethodInfo killMethod = AccessTools.Method(typeof(Pawn), "Kill");
			harmony.Patch(killMethod, null, new HarmonyMethod(typeof(EventBridge).GetMethod("OnPawnKilled")));
        }

		public static void OnPawnKilled(Pawn __instance)
		{
			Log.Message("Pawn Killed "+__instance);
			modLoader.currentChallenge.OnPawnKilled(__instance);
		}
	}
}