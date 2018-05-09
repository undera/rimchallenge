using System;
using Verse;

namespace Rimchallenge
{
	public class Controller: Mod
	{
        public Controller(ModContentPack content) : base(content)
        {
			EventBridge.Hook(this);
        }

   	}   
}
// TODO: save challenge into save file
// TODO: save challenge revealed state globally