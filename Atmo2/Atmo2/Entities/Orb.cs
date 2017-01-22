using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indigo;
using Indigo.Content;
using Indigo.Graphics;
using Utility;
using Indigo.Core;

namespace Atmo2.Entities
{
	public class Orb : Entity
	{
		public const int DISTANCE_BEWTWEEN_ORBS = 30;
		public float DistanceToLeader;

		public Spritemap baseOrb;
		private Entity followLeader;
		public int OrbType;

		public Orb(float x, float y, int orbType, Entity follow)
			:base(x, y)
		{
			string texture;
			switch (OrbType = orbType)
			{
				case 0:
					texture = "content/image/OrbYellow.png";
					break;
				case 1:
					texture = "content/image/OrbGreen.png";
					break;
				case 2:
					texture = "content/image/OrbRed.png";
					break;
				case 3:
					texture = "content/image/OrbBlue.png";
					break;
				default:
					texture = "content/image/OrbGrey.png";
					break;
			}
			baseOrb = new Spritemap(Library.Get<Texture>(texture), 24, 24);
			//baseOrb.Color = Engine.Random.Color();
			baseOrb.CenterOrigin();
			baseOrb.RenderStep = -1;
			baseOrb.Add("idle", FP.MakeFrames(0, 0), 3, true);
			baseOrb.Play("idle");

			AddComponent<Image>(baseOrb);

			DistanceToLeader = DISTANCE_BEWTWEEN_ORBS * (int)OrbType;
			//Console.WriteLine(DistanceToLeader);

			followLeader = follow;
		}

		public override void Update(GameTime time)
		{
			base.Update(time);
			FollowTheLeader();
		}

		public void FollowTheLeader()
		{
			if (followLeader == null)
				return;
			var differenceToParent = new Point(followLeader.X - X, followLeader.Y - Y);
			var distanceToParent = differenceToParent.Length;

			if (distanceToParent > DISTANCE_BEWTWEEN_ORBS)
			{
				var tooFar = distanceToParent - DISTANCE_BEWTWEEN_ORBS;
				var translation = differenceToParent.Normalized() * tooFar;

				X += translation.X;
				Y += translation.Y;
			}
		}
	}
}
