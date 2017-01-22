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
	public enum OrbType:int
	{
		Yellow = 0,
		Green = 1,
		Red = 2,
		Blue = 3,
		Grey = 4
	}

	public class Orb : Entity
	{
		public const int DISTANCE_BEWTWEEN_ORBS = 30;
		public float DistanceToLeader;

		public Spritemap baseOrb;
		private Entity followLeader;
		public OrbType OrbType;
		public bool IsActivated;

		public Orb(float x, float y, OrbType orbType, Entity follow)
			:base(x, y)
		{
			
			baseOrb = new Spritemap(Library.Get<Texture>(GetTexture(OrbType = orbType)), 24, 24);
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

		public static string GetTexture(OrbType orbType)
		{
			string texture;
			switch (orbType)
			{
				case OrbType.Yellow:
					texture = "content/image/OrbYellow.png";
					break;
				case OrbType.Green:
					texture = "content/image/OrbGreen.png";
					break;
				case OrbType.Red:
					texture = "content/image/OrbRed.png";
					break;
				case OrbType.Blue:
					texture = "content/image/OrbBlue.png";
					break;
				case OrbType.Grey:
				default:
					texture = "content/image/OrbGrey.png";
					break;
			}
			return texture;
		}
	}
}
