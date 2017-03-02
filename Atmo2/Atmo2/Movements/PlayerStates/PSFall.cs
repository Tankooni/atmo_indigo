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
    class PSFall : PlayerState
    {
        private float gravity;
        private float speed;

        public PSFall(Player player, float gravity, float speed = -1)
			: base(player)
		{
            this.player     = player;
            this.gravity    = gravity;
            this.speed      = speed < 0 ? player.RunSpeed : speed;
        }
        public override void OnEnter()
        {
            if(player.MovementInfo.VelY > 0)
                player.image.Play("fall");
        }

        public override void OnExit()
        {
            //player.MovementInfo.VelY = 0;
        }

        public override PlayerState Update(GameTime time)
        {
            player.MovementInfo.VelY += gravity;

            // Animation
            if (player.MovementInfo.VelY > 0)
                player.image.Play("fall");

			if (Controller.Attack())
			{
				return new PSAttackNormal(player, KQ.STANDARD_GRAVITY);
			}

			if (Controller.DownHeld())
			{
				if ((Controller.Jump() || Controller.Dash()) && time.TotalMilliseconds - PSDiveKick.last_bounce > 300)
					return new PSDiveKick(player, KQ.STANDARD_GRAVITY);
			}

			if (Controller.LeftHeld())
            {
                player.MovementInfo.Move -= this.speed;
                player.image.FlippedX = true;
            } else if (Controller.RightHeld())
            {
                player.MovementInfo.Move += this.speed;
                player.image.FlippedX = false;
            }

            Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null && !this.player.IsInvincable)
            {
                return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
            }

            if (player.Abilities.DoubleJump &&
                Controller.Jump() && 
                player.Energy >= 1)
            {
                player.Energy -= 1;
                return new PSJump(player);
            }

            if(player.Abilities.AirDash && 
                Controller.Dash() && 
                player.Energy >= 1)
            {
				if (Controller.LeftHeld() || Controller.RightHeld())
				{
					player.Energy -= 1;
					return new PSDash(player);
				}
            }

           

            if (player.MovementInfo.OnGround)
                if (player.MovementInfo.Move == 0)
                    return new PSIdle(player);
                else // Hit the ground runnin'
                    return new PSRun(player);

            return null;
        }
    }
}
