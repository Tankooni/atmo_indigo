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
	public class LayoutRoom : Entity, Indigo.Loaders.IOgmoNodeHandler
	{
		public string Filename;
		public Image RoomImage;

		public LayoutRoom()
		{
			AddComponent<Graphic>(RoomImage = new Image(Library.Get<Texture>("content/image/layoutRoom.png")));
		}

		public void NodeHandler(System.Xml.XmlNode entity)
		{
			RoomImage.ScaleX = Width / (float)RoomImage.Width;
			RoomImage.ScaleY = Height / (float)RoomImage.Height;
		}
	}
}
