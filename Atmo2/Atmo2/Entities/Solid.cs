using Indigo;
using Indigo.Graphics;
using Indigo.Masks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Atmo2.Entities
{
	public class Solid : Entity
	{
		private float xRemainder = 0;
		private float yRemainder = 0;
		private List<Actor> allActors;

		public Solid(float x = 0, float y = 0, Graphic graphic = null, Mask mask = null)
			: base(0, 0, graphic, mask)
		{
			Type = KQ.CollisionTypes.Solid.ToString();
		}

		public override void Added()
		{
			base.Added();
			allActors = new List<Actor>();
		}

		public override void Update()
		{
			base.Update();
			allActors = World.GetAllEntities().OfType<Actor>().ToList();
		}

		public void Move(float amountX, float amountY)
		{
			xRemainder += amountX;
			yRemainder += amountY;
			int moveX = (int)Math.Round(xRemainder);
			int moveY = (int)Math.Round(yRemainder);

			if(moveX != 0 || moveY != 0)
			{
				//Loop through every Actor in the Level, add it to
				//a list if actor.IsRiding(this) is true
				var ridingActors = GetAllRidingActors();

				//Make this Solid non-collidable for Actors,
				//so that Actors moved by it do not get stuck on it
				Collidable = false;

				if(moveX != 0)
				{
					xRemainder -= moveX;
					X += moveX;

					if(moveX > 0)
					{
						foreach(Actor actor in allActors)
						{
							if(OverlapCheck(actor))
							{
								//Push right
								actor.MoveX(this.Right - actor.Left, actor.Squish);
							}
							else if(ridingActors.Contains(actor))
							{
								actor.MoveX(moveX, null);
							}
						}
					}
					else
					{
						foreach (Actor actor in allActors)
						{
							if (OverlapCheck(actor))
							{
								actor.MoveX(this.Left - actor.Right, actor.Squish);
							}
							else if (ridingActors.Contains(actor))
							{
								actor.MoveX(moveX, null);
							}
						}
					}
				}

				if (moveY != 0)
				{
					yRemainder -= moveY;
					Y += moveY;

					if (moveY > 0)
					{
						foreach(Actor actor in allActors)
						{
							if (OverlapCheck(actor))
							{
								//Push down
								actor.MoveY(this.Bottom - actor.Top, actor.Squish);
							}
							else if (ridingActors.Contains(actor))
							{
								//Carry down
								actor.MoveY(moveY, null);
							}
						}
					}
					else
					{
						foreach (Actor actor in allActors)
						{
							if (OverlapCheck(actor))
							{
								//Push up
								actor.MoveY(this.Top - actor.Bottom, actor.Squish);
							}
							else if (ridingActors.Contains(actor))
							{
								//Carry up
								actor.MoveY(moveY, null);
							}
						}
					}
				}

				Collidable = true;
			}
		}

		public IEnumerable<Actor> GetAllRidingActors()
		{
			return allActors.Where(x => x.IsRiding(this));
		}

		public bool OverlapCheck(Entity e)
		{
			return e != this &&
				X - OriginX + Width > e.X - e.OriginX &&
				Y - OriginY + Height > e.Y - e.OriginY &&
				X - OriginX < e.X - e.OriginX + e.Width &&
				Y - OriginY < e.Y - e.OriginY + e.Height;
		}
	}
}
