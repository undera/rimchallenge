using System;
using UnityEngine;
using Verse;

namespace Rimchallenge
{
	public class Dialog_GameEnd : Dialog_NodeTree
	{
		private Texture2D image;
        
		public Dialog_GameEnd(DiaNode nodeRoot, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(nodeRoot, delayInteractivity, radioMode, title)
		{
		}

		public override void DoWindowContents(Rect inRect)
		{
			if (image == null) {
				image = ContentFinder<Texture2D>.Get("UI/camp3", true);
			}

			base.DoWindowContents(inRect);
			Rect rect = new Rect(inRect.width - image.width, inRect.height - image.height, image.width, image.height);
			GUI.DrawTexture(rect, image);
		}
	}
}
