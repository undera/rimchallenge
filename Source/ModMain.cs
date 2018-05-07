using System;
namespace rimchallenge
{
	public class ModMain: HugsLib.ModBase
	{
        public ModMain()
        {
			
        }

		public override string ModIdentifier
        {
            get { return "RimChallenges"; }
        }

        public override void DefsLoaded()
		{
			base.DefsLoaded();
			Logger.Message("Logger Test");
			// TODO: inject harmony patches here
		}
	}
}
