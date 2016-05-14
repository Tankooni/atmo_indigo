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
		private string roomName;
		private Door callingDoor;
		private string doorConnectionName;
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
			ogmoSenpai.RegisterGridType("TileCollision", KQ.CollisionTypes.Solid.ToString(), 16, 16);
			ogmoSenpai.RegisterGridType("px4TileCollision", KQ.CollisionTypes.Solid.ToString(), 4, 4);

			ogmoSenpai.RegisterTilemapType("Blocks", 16, 16, Library.Get<Texture>("content/ogmo/rooms/roomProjectTileset.png"));

			foreach(LayoutRoom layoutRoom in layoutMapWorld.MapRooms.OfType<LayoutRoom>())
			{
				XmlDocument roomXml = Library.Get<XmlDocument>("content/ogmo/rooms/" + layoutRoom.Filename + ".oel");
				//Load level properties
				RealRoomMeta realRoomMeta = new RealRoomMeta
				{
					Width = 320,
					Height = 1200,
					RoomMusic = "B_piano_mid;1"
				};
				RealRoom realRoom = new RealRoom
				(
					ogmoSenpai.BuildLevelAsArray(roomXml).ToList(),
					realRoomMeta,
					layoutRoom,
					this
				);

				rooms.Add(layoutRoom.Filename, realRoom);
			}

			(CurrentRoom = rooms["surface01"]).PopulateWorld();
			Add(Player);
		}

		public override void Update()
		{
			base.Update();
			Console.WriteLine(AudioManager.CurrentSong.Position);
			if (Keyboard.Space.Pressed)
			{
				FP.World = layoutMapWorld;
			}
		}
	}
}
