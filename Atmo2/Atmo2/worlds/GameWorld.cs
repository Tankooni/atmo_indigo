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

namespace Atmo2.Worlds
{
	public class GameWorld : World
	{
        public static GameWorld World;
		public static Player Player;
		public RealRoom CurrentRoom { get; private set; }

		private Dictionary<string, RealRoom> rooms = new Dictionary<string, RealRoom>();
        private Map map;
		//private LayoutMapWorld layoutMapWorld;

		//Jank stuff for room changing
		private bool changeToNewRoom1 = false;
		private bool changeToNewRoom2 = false;
		private Door callingDoor;

		public GameWorld()
			: base()
		{
            World = this;

            // Fade
            Fade fade = new Fade(new Color(0x0000), 30, 0.05f);
            this.Add(fade);

            // Ogmo loading
			OgmoLoader ogmoSenpai = new OgmoLoader();

            // TODO: Move enemy init logic somewhere else
            ogmoSenpai.RegisterClassAlias<Enemy>("EnemyWalker");

			ogmoSenpai.DefineGrid("TileCollision", KQ.CollisionTypeSolid, 16, 16);
			ogmoSenpai.DefineGrid("px4TileCollision", KQ.CollisionTypeSolid, 4, 4);

			ogmoSenpai.DefineTilemap("Blocks", 16, 16, Library.Get<Texture>("content/ogmo/rooms/roomProjectTileset.png"));

            Map map = new Map("content/ogmo/layout/layout.oel");
            this.Add(map);

			foreach(MapRoom layoutRoom in map.mapRooms.OfType<MapRoom>())
			{
				XmlDocument roomXml = Library.Get<XmlDocument>("content/ogmo/rooms/" + layoutRoom.Filename + ".oel");
				//Load level properties

				RealRoom realRoom = new RealRoom
				(
					ogmoSenpai.BuildLevelAsArray(roomXml).ToList(),
					ogmoSenpai.GetLevelProperties<RealRoomMeta>(roomXml),
					layoutRoom,
					this
				);
				realRoom.RealRoomMeta.Init();
				rooms.Add(layoutRoom.Filename, realRoom);
			}

			(CurrentRoom = rooms["surface01"]).PopulateWorld();
			Add(Player);

			

			Entity follow = Player;
			for (int i = 1; i <= 2; i++)
			{
				Orb orb = new Orb(Player.X, Player.Y, i, follow);
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
				Log.WriteFormat("Player x:{0}, y:{1}", Player.X, Player.Y);

			if (MathHelper.DistanceRects(0, 0, CurrentRoom.RealRoomMeta.width, CurrentRoom.RealRoomMeta.height, Player.X, Player.Y, Player.Width, Player.Height) != 0)
				Player.ResetPlayerPosition();
			
			if(Keyboard.Num1.Pressed)
			{
                //AudioManager.CurrentSong
			}
			
			path.AddFirst(new Point(/*MouseX, MouseY*/Player.X, Player.Y - Player.Height));
			FollowHead(orbs, path);
		}

		public void StartChangeRoom(object[] args)
		{
			changeToNewRoom1 = true;
			//fadeToBlackImage.Alpha = 0;
			//Add(fadeToBlack);
			callingDoor = (Door)args[0];
		}

		public void ActuallyChangeRoom()
		{
			CurrentRoom.GenocideWorld();
			CurrentRoom = rooms[callingDoor.SceneConnectionName];
			Door spawnDoor = CurrentRoom.Doors[callingDoor.DoorConnectionName];
			spawnDoor.TraveledThrough = true;
			//Add(Player);

			switch (spawnDoor.OutDir)
			{
				case DoorDirection.Left:
					Player.X = spawnDoor.X - Player.HalfWidth;
					Player.Y = spawnDoor.Y + Player.Y - callingDoor.Y;
					break;
				case DoorDirection.Right:
					Player.X = spawnDoor.X + spawnDoor.Width + Player.HalfWidth;
					Player.Y = spawnDoor.Y + Player.Y - callingDoor.Y;
					break;
				case DoorDirection.Up:
					Player.X = spawnDoor.X + Player.X - callingDoor.X;
					Player.Y = spawnDoor.Y;
					break;
				case DoorDirection.Down:
					Player.X = spawnDoor.X + Player.X - callingDoor.X;
					Player.Y = spawnDoor.Y + spawnDoor.Height + Player.Height;
					break;
			}

			Player.SetResetPointToCurrentLocation();
			Player.UpdateCamera();
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
