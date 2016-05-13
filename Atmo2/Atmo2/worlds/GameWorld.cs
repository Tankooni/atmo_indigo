using Indigo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Atmo2.Worlds
{
	public class GameWorld : World
	{
		public override void Update()
		{
			base.Update();
			Console.WriteLine(AudioManager.CurrentSong.Position);
		}
	}
}
