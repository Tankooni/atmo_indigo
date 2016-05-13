﻿using Indigo;
using System;

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

		public bool IsRiding(Solid solid)
		{
			return false;
		}

		public void Squish()
		{
		}

		public void MoveX(float amount, Action onCollide)
		{
			xRemainder += amount;
			int move = (int)Math.Round(xRemainder);
			if (move != 0)
			{
				xRemainder -= move;
				int sign = FP.Sign(move);

				while (move != 0)
				{
					if (Collide("Solid", X + sign, Y) == null)
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

		public void MoveY(float amount, Action onCollide)
		{
			yRemainder += amount;
			int move = (int)Math.Round(yRemainder);
			if (move != 0)
			{
				yRemainder -= move;
				int sign = FP.Sign(move);

				while (move != 0)
				{
					if (Collide("Solid", X, Y + sign) == null)
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
