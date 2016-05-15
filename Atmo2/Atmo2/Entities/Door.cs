using Indigo;
using Indigo.Content;
using Indigo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Atmo2.Entities
{
	public class Door : Entity, Indigo.Loaders.IOgmoNodeHandler
	{
		public string DoorName { get; set; }
		public string SceneConnectionName { get; set; }
		public string DoorConnectionName { get; set; }
		public DoorDirection OutDir { get; set; }
		public bool TraveledThrough { get; set; }

		private Image doorImage { get; set; }
		private Entity collision { get; set; }

		public Door()
		{
			AddComponent<Graphic>(doorImage = new Image(Library.Get<Texture>("content/image/door.png")));
		}

		public void NodeHandler(System.Xml.XmlNode entity)
		{
			doorImage.ScaleX = Width / doorImage.Width;
			doorImage.ScaleY = Height / doorImage.Height;
			SetHitbox((int)(doorImage.ScaleX * doorImage.Width), (int)(doorImage.ScaleY * doorImage.Height));
		}

		public override void Update()
		{
			base.Update();

			if((collision = Collide(KQ.CollisionTypePlayer, X, Y)) != null)
			{
				if(!TraveledThrough)
				{
					FP.World.BroadcastMessage(DoorMessages.StartChangeRoom, this);
				}
			}
			else
			{
				TraveledThrough = false;
			}
		}

		public enum DoorMessages
		{
			StartChangeRoom
		}
	}

	public enum DoorDirection
	{
		Left,
		Right,
		Up,
		Down
	}
}
