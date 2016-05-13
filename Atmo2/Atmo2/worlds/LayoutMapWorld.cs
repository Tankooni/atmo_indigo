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
		public List<Entity> mapRooms;
		private GameWorld gameWorld;

		public LayoutMapWorld(List<Entity> mapRooms, GameWorld gameWorld)
			:base()
		{
			this.mapRooms = mapRooms;
			this.gameWorld = gameWorld;

			AddList(mapRooms);
		}

		public override void Update()
		{
			base.Update();
			if (Keyboard.Tab.Pressed)
				FP.World = gameWorld;
		}
	}
}
