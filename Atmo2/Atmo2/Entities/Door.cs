using Indigo;
using Indigo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Entities
{
	public class Door : Entity
	{
		public string DoorName { get; set; }
		public string SceneConnectionName { get; set; }
		public string DoorConnectionName { get; set; }
		public string OutDir { get; set; }
		public bool TraveledThrough { get; set; }

		private Image doorImage { get; set; }
		private Entity collision { get; set; }

		public Door()
		{
			AddComponent<Graphic>(doorImage = new Image(Library.Get<Image>("content/image/layoutRoom.png")));
			doorImage.ScaleX = Width / doorImage.Width;
			doorImage.ScaleY = Height / doorImage.Height;
			SetHitbox((int)(doorImage.ScaleX * doorImage.Width), (int)(doorImage.ScaleY * doorImage.Height));
		}

		public override void Update()
		{
			base.Update();

			if((collision = Collide("player", X, Y)) != null)
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
}
