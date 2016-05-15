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

namespace Atmo2.Worlds
{
	public class GameWorld : World
	{
		public static Player Player;
		public RealRoom CurrentRoom { get; private set; }

		private Dictionary<string, RealRoom> rooms = new Dictionary<string, RealRoom>();
		private LayoutMapWorld layoutMapWorld;

		//Jank stuff for room changing
		private Entity fadeToBlack;
		private Image fadeToBlackImage;
		private bool changeToNewRoom1 = false;
		private bool changeToNewRoom2 = false;
		private Door callingDoor;
		private int fadeBuffer = 30;
		private float fadeIncrement = .05f;

		public GameWorld()
			: base()
		{
			fadeToBlack = new Entity();
			fadeToBlack.AddComponent(fadeToBlackImage = new Image(Library.Get<Texture>("content/image/white.png")));
			fadeToBlackImage.ScaleX = FP.Width + fadeBuffer;
			fadeToBlackImage.ScaleY = FP.Height + fadeBuffer;
			fadeToBlack.RenderStep = -999999;
			fadeToBlackImage.Color = new Color(0x000000);
			fadeToBlackImage.ScrollX = 0;
			fadeToBlackImage.ScrollY = 0;

			OgmoLoader ogmoSenpai = new OgmoLoader();
			ogmoSenpai.RegisterClassAlias<LayoutRoom>("Room");
			var bob = ogmoSenpai.BuildLevelAsArray(Library.Get<XmlDocument>("content/ogmo/layout/layout.oel"));
			layoutMapWorld = new LayoutMapWorld(
				ogmoSenpai.BuildLevelAsArray(Library.Get<XmlDocument>("content/ogmo/layout/layout.oel")),
				this);
			ogmoSenpai.RegisterGridType("TileCollision", KQ.CollisionTypeSolid, 16, 16);
			ogmoSenpai.RegisterGridType("px4TileCollision", KQ.CollisionTypeSolid, 4, 4);

			ogmoSenpai.RegisterTilemapType("Blocks", 16, 16, Library.Get<Texture>("content/ogmo/rooms/roomProjectTileset.png"));

			foreach(LayoutRoom layoutRoom in layoutMapWorld.MapRooms.OfType<LayoutRoom>())
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

			AddResponse(Door.DoorMessages.StartChangeRoom, StartChangeRoom);
		}

		public override void Update()
		{
			if (changeToNewRoom1)
			{
				if(!changeToNewRoom2 && (fadeToBlackImage.Alpha += fadeIncrement) >= 1)
				{
					ActuallyChangeRoom();
					changeToNewRoom2 = true;
				}
				else if (changeToNewRoom2 && (fadeToBlackImage.Alpha -= fadeIncrement) <= 0)
				{
					changeToNewRoom1 = changeToNewRoom2 = false;
					Remove(fadeToBlack);
				}
				return;
			}

			base.Update();

			if (Keyboard.Space.Pressed)
				FP.World = layoutMapWorld;
			if (Keyboard.M.Pressed)
				FP.LogFormat("Player x:{0}, y:{1}", Player.X, Player.Y);

			if (FP.DistanceRects(0, 0, CurrentRoom.RealRoomMeta.width, CurrentRoom.RealRoomMeta.height, Player.X, Player.Y, Player.Width, Player.Height) != 0)
			{
				Player.ResetPlayerPosition();
			}
		}

		public void StartChangeRoom(object[] args)
		{
			changeToNewRoom1 = true;
			fadeToBlackImage.Alpha = 0;
			Add(fadeToBlack);
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
	}
}
