using Atmo2.Entities;
using Indigo;
using Indigo.Content;
using Indigo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Audio;

namespace Atmo2.Worlds.Rooms
{
	public class Room
	{
		public RealRoomMeta RealRoomMeta { get; private set; }
		public Dictionary<string, Door> Doors { get; private set; }
        public List<Enemy> Enemies { get; set; }

		private MapRoom layoutRoom;
		private List<Entity> entities;
		private World parentWorld;

		public Room(List<Entity> entities, RealRoomMeta realRoomMeta, MapRoom layoutRoom, World parentWorld)
		{
			RealRoomMeta = realRoomMeta;
			this.entities = entities;
			this.layoutRoom = layoutRoom;
			this.parentWorld = parentWorld;

            foreach (var e in entities)
                e.RenderStep = -5;

			Doors = new Dictionary<string, Door>();
            foreach (var door in entities.OfType<Door>())
            {
                Doors.Add(door.DoorName, door);
                door.RenderStep = -10;
            }

            Enemies = new List<Enemy>();
            foreach (var enemy in entities.OfType<Enemy>())
            {
                enemy.RenderStep = 0;
            }

			var bgEnt = new Entity();
			var bg = bgEnt.AddComponent<Image>(new Image(Library.Get<Texture>("content/image/IceCaveBg.png")));
			bg.ScrollX = bg.ScrollY = 0;
			bgEnt.RenderStep = -9999999;
			parentWorld.Add(bgEnt);
		}

		public void PopulateWorld()
		{
			parentWorld.AddList(entities);
			AudioManager.LoopMusic(RealRoomMeta.roomMusic);
		}

		public void GenocideWorld()
		{
			parentWorld.RemoveList(entities);
		}
	}
}
