using Atmo2.Entities;
using Atmo2.Worlds.Rooms;
using Indigo;
using Indigo.Graphics;
using Indigo.Inputs;
using Indigo.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.Audio;
using System.Xml;
using Indigo.Content;
using Indigo.Core;
using Atmo2.Transitions;
using Glide;
using Indigo.Utils;
using Atmo2.UI;

namespace Atmo2.Worlds
{
	public class GameWorld : World
	{
        public static GameWorld World;
		public static Player player;
		public Room CurrentRoom { get; private set; }

		private Dictionary<string, Room> rooms = new Dictionary<string, Room>();

		public GameWorld()
			: base()
		{
            World = this;
            Engine.Console.InvisibleUntilOpen = true;

            // Fade
            Fade fade = new Fade(new Color(0x000000), 0.15f);
            AddResponse(Door.DoorMessages.StartChangeRoom, (a) =>
            {
                player.Active = false;
                fade.FadeIn(() =>
                {
                    Door callingDoor = (Door)a[0];
                    ActuallyChangeRoom(callingDoor);
                    fade.FadeOut(() => player.Active = true);
                });
            });
            this.Add(fade);

            // Ogmo loading
			OgmoLoader ogmoSenpai = new OgmoLoader();

            // TODO: Move enemy init logic somewhere else
            ogmoSenpai.RegisterClassAlias<Enemy>("EnemyWalker");

			ogmoSenpai.DefineGrid("TileCollision", KQ.CollisionTypeSolid, 24, 24);
			ogmoSenpai.DefineGrid("px4TileCollision", KQ.CollisionTypeSolid, 6, 6);

			ogmoSenpai.DefineTilemap("Blocks", 24, 24, Library.Get<Texture>("content/ogmo/rooms/roomProjectTileset.png"));

            Map map = new Map("content/ogmo/layout/layout.oel");
            this.Add(map);

			foreach(MapRoom layoutRoom in map.mapRooms.OfType<MapRoom>())
			{
				XmlDocument roomXml = Library.Get<XmlDocument>("content/ogmo/rooms/" + layoutRoom.Filename + ".oel");
				//Load level properties

				Room realRoom = new Room
				(
					ogmoSenpai.BuildLevelAsArray(roomXml).ToList(),
					ogmoSenpai.GetLevelProperties<RealRoomMeta>(roomXml),
					layoutRoom,
					this
				);
				realRoom.RealRoomMeta.Init();
				rooms.Add(layoutRoom.Filename, realRoom);
			}

			(CurrentRoom = rooms["start"]).PopulateWorld();
			Add(player);

            HUD hud = new HUD(player);
            Add(hud);
			

			Entity follow = player;
			for (int i = 0; i < 4; i++)
			{
				Orb orb = new Orb(player.X, player.Y, i, follow);
				follow = orb;
				orbs.Add(Add(orb));
			}
		}

		public override void Update(GameTime time)
		{
			base.Update(time);

			//if (Keyboard.Space.Pressed)
			//FP.World = layoutMapWorld;
			if (Keyboard.M.Pressed)
				Log.WriteFormat("Player x:{0}, y:{1}", player.X, player.Y);

			if (MathHelper.DistanceRects(0, 0, CurrentRoom.RealRoomMeta.width, CurrentRoom.RealRoomMeta.height, player.X, player.Y, player.Width, player.Height) != 0)
				player.ResetPlayerPosition();
			
			if(Keyboard.Num1.Pressed)
			{
                //AudioManager.CurrentSong
			}
			
			//path.AddFirst(new Point(/*MouseX, MouseY*/player.X, player.Y - player.Height));
			//FollowHead(orbs, path);
		}

		public void ActuallyChangeRoom(Door callingDoor)
		{
			CurrentRoom.GenocideWorld();
			CurrentRoom = rooms[callingDoor.SceneConnectionName];
			Door spawnDoor = CurrentRoom.Doors[callingDoor.DoorConnectionName];
			spawnDoor.TraveledThrough = true;
			//Add(Player);

			switch (spawnDoor.OutDir)
			{
				case DoorDirection.Left:
					player.X = spawnDoor.X - player.HalfWidth;
					player.Y = spawnDoor.Y + player.Y - callingDoor.Y;
					break;
				case DoorDirection.Right:
					player.X = spawnDoor.X + spawnDoor.Width + player.HalfWidth;
					player.Y = spawnDoor.Y + player.Y - callingDoor.Y;
					break;
				case DoorDirection.Up:
					player.X = spawnDoor.X + player.X - callingDoor.X;
					player.Y = spawnDoor.Y;
					break;
				case DoorDirection.Down:
					player.X = spawnDoor.X + player.X - callingDoor.X;
					player.Y = spawnDoor.Y + spawnDoor.Height + player.Height;
					break;
			}

			player.SetResetPointToCurrentLocation();
			player.UpdateCamera();
			CurrentRoom.PopulateWorld();
		}

		private List<Orb> orbs = new List<Orb>();
		private LinkedList<Point> path = new LinkedList<Point>();

		void FollowHead(List<Orb> orbos, LinkedList<Point> pathos)
		{
			//assuming at least one node in linked list
			var segmentStart = pathos.First.Value;
			var segmentEndNode = pathos.First.Next;
			var lengthToSegmentEnd = 0f;

			foreach (var orb in orbos)
			{
				var segmentEnd = Point.Zero;
				var segmentDiff = Point.Zero;
				var segmentLength = 0f;
				var lengthToSegmentStart = lengthToSegmentEnd;

				//advance to correct segment if needed
				while (orb.DistanceToLeader > lengthToSegmentEnd)
				{
					if (segmentEndNode == null) //path too short, back out early
					{
						return;
						//break;
					}

					segmentEnd = segmentEndNode.Value;
					segmentDiff = segmentEnd - segmentStart;
					segmentLength = segmentDiff.Length;
					lengthToSegmentEnd += segmentLength;

					segmentEndNode = segmentEndNode.Next;
				}

				//interpolate position on segment
				var distanceLeft = orb.DistanceToLeader - lengthToSegmentStart;
				var percentageAlongSegment = distanceLeft / segmentLength;

				var newPos = segmentStart + segmentDiff * percentageAlongSegment;
				orb.X = newPos.X;
				orb.Y = newPos.Y;
			}

			while (segmentEndNode != pathos.Last)
			{
				//Console.WriteLine("END {0}", pathos.Count);
				pathos.RemoveLast();
			}
		}
	}
}
