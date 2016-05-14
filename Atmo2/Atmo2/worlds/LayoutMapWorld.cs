using Indigo;
using Indigo.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Worlds
{
	public class LayoutMapWorld : World
	{
		public Entity[] MapRooms;
		private GameWorld gameWorld;

		public LayoutMapWorld(Entity[] mapRooms, GameWorld gameWorld)
			:base()
		{
			MapRooms = mapRooms;
			this.gameWorld = gameWorld;

			AddList(mapRooms);
		}

		public override void Update()
		{
			base.Update();
			if (Keyboard.Space.Pressed)
				FP.World = gameWorld;
		}
	}
}
