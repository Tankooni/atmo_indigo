using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atmo2.Entities;
using Indigo;
using Utility;


namespace Atmo2.Movements.PlayerStates
{
	class PSAttackNormal : PlayerState
	{
		private float gravity;
		public PSAttackNormal(Player player, float gravity)
			: base(player)
		{
			this.player = player;
			this.gravity = gravity;
		}

		public override void OnEnter()
		{
			player.image.Play("attackNormal");
			Enemy enemy = player.World.CollideRect(KQ.CollisionTypeEnemy, player.X + player.Width * (player.image.FlippedX ? -1 : 1), player.Y, player.Width, player.Height) as Enemy;
			if(enemy != null)
			{
				enemy.World.Remove(enemy);
			}
		}

		public override void OnExit()
		{
		}

		public override PlayerState Update(GameTime time)
		{
			player.MovementInfo.VelY += gravity;

			return null;
		}

		public override PlayerState OnAnimationComplete()
		{
			if (player.MovementInfo.OnGround)
				if (Controller.LeftHeld() || Controller.RightHeld())
					return new PSRun(player);
				else
					return new PSIdle(player);
			else
				return new PSFall(player, KQ.STANDARD_GRAVITY);
		}
	}
}
