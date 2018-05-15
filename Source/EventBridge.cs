using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;

namespace Rimchallenge
{
	public static class EventBridge
	{
		private static HarmonyInstance harmony = HarmonyInstance.Create("rimworld.undera4.challenge");

		private static void patch(Type type, string srcName, string preName, string postName, Type[] hint=null)
		{
			MethodBase srcMethod = AccessTools.Method(type, srcName, hint);
			HarmonyMethod preMethod = preName != null ? new HarmonyMethod(typeof(EventBridge).GetMethod(preName)) : null;
			HarmonyMethod postMethod = postName != null ? new HarmonyMethod(typeof(EventBridge).GetMethod(postName)) : null;
			harmony.Patch(srcMethod, preMethod, postMethod);
		}

		public static void Hook()
		{
			patch(typeof(Map), nameof(Map.FinalizeInit), null, nameof(OnMapLoaded));
			patch(typeof(IncidentWorker_NeutralGroup), "SpawnPawns", null, nameof(OnGroupSpawned));
			patch(typeof(PawnRenderer), nameof(PawnRenderer.RenderPawnAt), null, nameof(RenderPawnAt), new[] { typeof(Vector3), typeof(RotDrawMode), typeof(bool) });
			patch(typeof(FloatMenuMakerMap), nameof(FloatMenuMakerMap.ChoicesAtFor), null, nameof(ChoicesAtFor));

			// CHALLENGE HOOKS BELOW
			patch(typeof(Pawn), nameof(Pawn.Kill), null, nameof(OnPawnKilled));
			patch(typeof(Pawn), nameof(Pawn.Destroy), null, nameof(OnPawnDestroyed));
			patch(typeof(Pawn), nameof(Pawn.SetFaction), null, nameof(OnPawnFactionSet));
			// FIXME when escape pod colonist joins, nothing happens

			patch(typeof(Mineable), nameof(Mineable.DestroyMined), null, nameof(OnDestroyMined));
			patch(typeof(Mineable), nameof(Mineable.Destroy), null, nameof(OnDestroyMined));

			patch(typeof(SkillRecord), nameof(SkillRecord.Learn), null, nameof(OnSkillLearned));
		}

		public static void OnMapLoaded()
		{
			ChallengeDef.GenerateNonOverlappingCoordinates();
			// TODO load saved status
		}

		public static void OnGroupSpawned(IncidentWorker_NeutralGroup __instance, List<Pawn> __result)
		{
			Log.Message("Spawned group by " + __instance);
			foreach (Pawn p in __result)
			{
				Log.Message(p + " can trade " + (p.trader == null));
			}
			ChallengeManager.instance.SetQuestOwner(__result.Where(x => x.trader==null).RandomElementWithFallback(null));
		}

		public static void RenderPawnAt(PawnRenderer __instance)
		{
			Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();

			if (ChallengeManager.instance.questOwner == pawn)
			{
				UITools.RenderAsteriskOverlay(pawn);
			}
		}

		public static void ChoicesAtFor(List<FloatMenuOption> __result, Vector3 clickPos, Pawn pawn)
		{
			foreach (LocalTargetInfo current in GenUI.TargetsAt(clickPos, TargetingParameters.ForAttackAny(), true))
			{
				if (current.Thing is Pawn && ChallengeManager.instance.questOwner == current.Thing)
				{
					UITools.AddChallengeOptions(clickPos, pawn, __result);
				}
			}
		}

		// CHALLENGES BELOW
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
			if (ChallengeManager.instance.questOwner == __instance)
			{
				ChallengeManager.instance.SetQuestOwner(null);
			}

			ChallengeManager.instance.currentChallenge.OnPawnDestroyed(__instance);
		}

		public static void OnDestroyMined(Mineable __instance)
		{
			ChallengeManager.instance.currentChallenge.OnDestroyMined(__instance);
		}

		public static void OnSkillLearned(SkillRecord __instance)
		{
			//Log.Message("Skill learned " + __instance);
			ChallengeManager.instance.currentChallenge.OnSkillLearned(__instance);
		}
	}
}