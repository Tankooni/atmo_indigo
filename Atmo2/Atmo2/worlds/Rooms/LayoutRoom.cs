using Indigo;
using Indigo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Worlds.Rooms
{
	public class LayoutRoom : Entity
	{
		public string Filename;
		public Image RoomImage;

		public LayoutRoom()
		{
			AddComponent<Graphic>(RoomImage = new Image(Library.Get<Image>("content/image/layoutRoom.png")));
			RoomImage.ScaleX = Width / RoomImage.Width;
			RoomImage.ScaleY = Height / RoomImage.Height;
		}
	}
}
