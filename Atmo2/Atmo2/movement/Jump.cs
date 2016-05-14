using Atmo2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements
{
	public class Jump : Movement
	{
		private const float JUMP_STR = 8f;

		public Jump(Player player)
			: base(player)
		{
		}

		public override bool Restart(MovementInfo movementInfo)
		{
			if (movementInfo.OnGround)
			{
				movementInfo.VelY = -JUMP_STR;
			}
			else if (movementInfo.MovesRemaining > 0)
			{
				movementInfo.MovesRemaining--;
				movementInfo.VelY = -JUMP_STR;
				//wings.visible = true;
				//wings.play("wings", true);
			}
			return false;
		}
	}
}
