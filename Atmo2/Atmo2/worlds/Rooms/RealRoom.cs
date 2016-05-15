using Atmo2.Entities;
using Indigo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Audio;

namespace Atmo2.Worlds.Rooms
{
	public class RealRoom
	{
		public RealRoomMeta RealRoomMeta { get; private set; }
		public Dictionary<string, Door> Doors { get; private set; }

		private LayoutRoom layoutRoom;
		private List<Entity> entities;
		private World parentWorld;

		public RealRoom(List<Entity> entities, RealRoomMeta realRoomMeta, LayoutRoom layoutRoom, World parentWorld)
		{
			RealRoomMeta = realRoomMeta;
			this.entities = entities;
			this.layoutRoom = layoutRoom;
			this.parentWorld = parentWorld;

			Doors = new Dictionary<string, Door>();
			foreach (var door in entities.OfType<Door>())
				Doors.Add(door.DoorName, door);
		}

		public void PopulateWorld()
		{
			parentWorld.AddList(entities);
			AudioManager.SetLayersPlaying(RealRoomMeta.MusicList);
		}

		public void GenocideWorld()
		{
			parentWorld.RemoveList(entities);
		}
	}
}
