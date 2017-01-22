using Indigo;
using Indigo.Content;
using Indigo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Worlds.Rooms
{
	public class MapRoom : Entity, Indigo.Loaders.IOgmoNodeHandler
	{
		public string Filename;
		private Image RoomImage;

        public float Scale
        {
            get { return RoomImage.Scale; }
            set { RoomImage.Scale = value; }
        } 

		public MapRoom()
		{
			AddComponent<Graphic>(RoomImage = new Image(Library.Get<Texture>("content/image/layoutRoom.png")));
            RoomImage.ScrollX = 0;
            RoomImage.ScrollY = 0;
            RoomImage.Alpha = 0.5f;
        }

		public void NodeHandler(System.Xml.XmlNode entity)
		{
			RoomImage.ScaleX = Width / (float)RoomImage.Width;
			RoomImage.ScaleY = Height / (float)RoomImage.Height;
            //RoomImage.OriginX = (FP.Screen.Width / 2) - this.X;
            //RoomImage.OriginY = (FP.Screen.Height / 2) - this.Y;
        }
	}
}
