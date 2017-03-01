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
    class PSDiveKick : PlayerState
    {
        //TODO: Fix this
        public static int last_bounce;
		private float gravity;

		public PSDiveKick(Player player, float gravity)
			: base(player)
		{
            this.player = player;
			this.gravity = gravity;
		}
        public override void OnEnter()
        {
            player.image.Play("diveKick");
            player.MovementInfo.VelY = 20f;
        }

        public override void OnExit()
        {
        }

        public override PlayerState Update(GameTime time)
        {
			player.MovementInfo.VelY += gravity;

			if (player.MovementInfo.OnGround)
            {
				if (Controller.LeftHeld() || Controller.RightHeld())
					return new PSRun(player);
				else
					return new PSIdle(player);
			}

			if (player.Abilities.DoubleJump &&
					Controller.Jump() &&
					player.Energy >= 1)
			{
				player.Energy -= 1;
				return new PSJump(player);
			}

			if (player.Abilities.AirDash &&
				Controller.Dash() &&
				player.Energy >= 1)
			{
				if (Controller.LeftHeld() || Controller.RightHeld())
				{
					player.Energy -= 1;
					return new PSDash(player);
				}
			}

			if (Controller.LeftHeld())
			{
				player.image.FlippedX = true;
				player.MovementInfo.Move -= player.RunSpeed;
			}
			else if (Controller.RightHeld())
			{
				player.image.FlippedX = false;
				player.MovementInfo.Move += player.RunSpeed;
			}

			// Check for enemy collision
			Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null)
            {
				player.MovementInfo.VelY = 0;
				enemy.World.Remove(enemy);
				player.Energy++;
                last_bounce = time.TotalMilliseconds;
                return new PSJump(player, 1.1f);
            }

            return null;
        }
    }
}
