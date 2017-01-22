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
		public const int DISTANCE_BEWTWEEN_ORBS = 15;
		public float DistanceToLeader;

		public Spritemap baseOrb;
		private Entity followLeader;

		public Orb(float x, float y, int order, Entity follow)
			:base(x, y)
		{
			baseOrb = new Spritemap(Library.Get<Texture>("content/image/OrbBase.png"), 12, 12);
			baseOrb.Color = Engine.Random.Color();
			baseOrb.CenterOrigin();
			baseOrb.RenderStep = -1;
			baseOrb.Add("idle", FP.MakeFrames(0, 3), 3, true);
			baseOrb.Play("idle");

			AddComponent<Image>(baseOrb);

			DistanceToLeader = DISTANCE_BEWTWEEN_ORBS * order;
			Console.WriteLine(DistanceToLeader);

			followLeader = follow;
		}

		public override void Update(GameTime time)
		{
			base.Update(time);
			//FollowTheLeader();
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
