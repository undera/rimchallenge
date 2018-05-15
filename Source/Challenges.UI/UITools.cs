using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Rimchallenge
{
	[StaticConstructorOnStartup]
	public static class UITools
	{
		private static Material asterisk;

		public static void RenderAsteriskOverlay(Pawn pawn)
		{
			if (!pawn.Spawned) return;
			var drawPos = pawn.DrawPos;
			drawPos.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays) + 0.28125f;
			if (pawn is Pawn)
			{
				drawPos.x += (float)pawn.def.size.x - 0.52f;
				drawPos.z += (float)pawn.def.size.z - 0.45f;
			}

			if (asterisk == null)
			{
				asterisk = MaterialPool.MatFrom("UI/Overlays/asterisk", ShaderDatabase.MetaOverlay);
			}

			RenderPulsing(pawn, asterisk, drawPos, MeshPool.plane05);
		}

		private static void RenderPulsing(Thing thing, Material mat, Vector3 drawPos, Mesh mesh)
		{
			var num = (Time.realtimeSinceStartup + 397f * (float)(thing.thingIDNumber % 571)) * 4f;
			var num2 = ((float)Math.Sin((double)num) + 1f) * 0.5f;
			num2 = 0.3f + num2 * 0.7f;
			var material = FadedMaterialPool.FadedVersionOf(mat, num2);
			Graphics.DrawMesh(mesh, drawPos, Quaternion.identity, material, 0);
		}

		public static void AddChallengeOptions(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			foreach (LocalTargetInfo current6 in GenUI.TargetsAt(clickPos, ForQuest(), true))
			{
				LocalTargetInfo dest = current6;
				if (!pawn.CanReach(dest, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					opts.Add(new FloatMenuOption("CantTalk".Translate()+" (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else if (pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
				{
					opts.Add(new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(new object[]
					{
						SkillDefOf.Social.LabelCap
					}), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else
				{
					Pawn pTarg = (Pawn)dest.Thing;
					Action action4 = delegate
					{
						Job job = new Job(ChallengeJobDefOf.ChallengeGiverJob, pTarg);
						job.playerForced = true;
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InteractingWithTraders, KnowledgeAmount.Total);
					};
					string str = string.Empty;
					if (pTarg.Faction != null)
					{
						str = " (" + pTarg.Faction.Name + ")";
					}

					string label = "TalkTo".Translate(new object[]
					{
						pTarg.LabelShort
					}) + str;
					MenuOptionPriority priority2 = MenuOptionPriority.InitiateSocial;
					Thing thing = dest.Thing;
					FloatMenuOption opt = new FloatMenuOption(label, action4, priority2, null, thing, 0f, null, null);
					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(opt, pawn, pTarg, "ReservedBy"));
				}
			}
		}

		public static TargetingParameters ForQuest()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = delegate (TargetInfo x)
			{
				return x.Thing is Pawn && x.Thing == ChallengeManager.instance.questOwner;
			};
			return targetingParameters;
		}
	}
}
