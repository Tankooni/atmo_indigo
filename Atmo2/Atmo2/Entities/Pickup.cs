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
	public enum PickupType
	{
		Jump,
		AirJump,
		Dash,
		AirDash
	}

	public class Pickup : Entity, Indigo.Loaders.IOgmoNodeHandler
	{
		public PickupType PickupType { get; set; }

		private Image pickupImage { get; set; }
		private Entity collision { get; set; }

		public Pickup()
		{

		}

		public void NodeHandler(System.Xml.XmlNode entity)
		{
			Texture texture;
			switch (PickupType)
			{
				case PickupType.Jump:
					texture = Library.Get<Texture>(Orb.GetTexture(OrbType.Yellow));
					break;
				case PickupType.AirJump:
					texture = Library.Get<Texture>(Orb.GetTexture(OrbType.Yellow));
					break;
				case PickupType.Dash:
					texture = Library.Get<Texture>(Orb.GetTexture(OrbType.Blue));
					break;
				case PickupType.AirDash:
					texture = Library.Get<Texture>(Orb.GetTexture(OrbType.Blue));
					break;
				default:
					texture = Library.Get<Texture>(Orb.GetTexture(OrbType.Grey));
					break;
			}
			pickupImage = AddComponent<Image>(new Image(texture));
			SetHitbox((int)pickupImage.ScaledWidth, (int)pickupImage.ScaledHeight);
		}

		public override void Update(GameTime time)
		{
			base.Update(time);
			
			if ((collision = Collide(KQ.CollisionTypePlayer, X, Y)) != null)
			{
				Engine.World.BroadcastMessage(PickupType);
				World.Remove(this);
			}
		}
	}
}
