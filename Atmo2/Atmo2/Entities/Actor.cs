using Indigo;
using System;
using Utility;

namespace Atmo2.Entities
{
	public class Actor : Entity
	{
		private float xRemainder = 0;
		private float yRemainder = 0;

		public Actor(float x = 0, float y = 0)
			: base(x, y)
		{
		}

		public virtual bool IsRiding(Solid solid)
		{
			return false;
		}

		public virtual void Squish()
		{
		}

		public void MoveX(float amount, Action onCollide = null)
		{
			xRemainder += amount;
			int move = (int)Math.Round(xRemainder);
			if (move != 0)
			{
				xRemainder -= move;
				int sign = Math.Sign(move);

				while (move != 0)
				{
					if (Collide(KQ.CollisionTypeSolid, X + sign, Y) == null)
					{
						//No solid immediately beside us
						X += sign;
						move -= sign;
					}
					else
					{
						//Hit a solid!
						if(onCollide != null)
							onCollide();
						break;
					}
				}
			}
		}

		public void MoveY(float amount, Action onCollide = null)
		{
			yRemainder += amount;
			int move = (int)Math.Round(yRemainder);

			if (move != 0)
			{
				yRemainder -= move;
				int sign = Math.Sign(move);

				while (move != 0)
				{
					if (Collide(KQ.CollisionTypeSolid, X, Y + sign) == null)
					{
						//No solid immediately beside us
						Y += sign;
						move -= sign;
					}
					else
					{
						//Hit a solid!
						if(onCollide != null)
							onCollide();
						break;
					}
				}
			}
		}
	}
}
