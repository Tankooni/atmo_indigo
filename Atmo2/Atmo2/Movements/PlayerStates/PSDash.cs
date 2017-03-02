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
    class PSDash : PlayerState
    {
        private float speed;
        private float duration;

        public PSDash(Player player, float speed = -1, float duration = .1f)
			: base(player)
		{
            this.player = player;
            this.speed = speed < 0 ? 7 * player.RunSpeed : speed;
            this.duration = duration;
        }
        public override void OnEnter()
        {
            player.image.Play("dash");
        }

        public override void OnExit()
        {
        }

        public override PlayerState Update(GameTime time)
        {
            Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null /*&& !this.player.IsInvincable*/)
            {
				//player.MovementInfo.Move = Math.Sign(player.MovementInfo.Move) * 100;
				enemy.World.Remove(enemy);
				player.Energy++;
				//return new PSFall(player);
				return new PSBounce(player, KQ.STANDARD_GRAVITY/*, enemy.touchDamage*/);
            }

            if (player.image.FlippedX)
                player.MovementInfo.Move -= this.speed;
            else
                player.MovementInfo.Move += this.speed;

			if (!player.MovementInfo.OnGround)
			{
				if (Controller.DownHeld())
				{
					if ((Controller.Jump() || Controller.Dash()) && time.TotalMilliseconds - PSDiveKick.last_bounce > 300)
						return new PSDiveKick(player, KQ.STANDARD_GRAVITY);
				}

				if (player.Abilities.DoubleJump &&
					Controller.Jump() &&
					player.Energy >= 1)
				{
					player.Energy -= 1;
					return new PSJump(player);
				}
			}

			if (player.MovementInfo.OnGround && Controller.Jump())
				return new PSJump(player);


			this.duration -= time.Elapsed;
            if(duration < 0)
            {
                // We're done here
                if (player.MovementInfo.OnGround)
                    if (Controller.LeftHeld() || Controller.RightHeld())
                        return new PSRun(player);
                    else
                        return new PSIdle(player);
                else
                    return new PSFall(player, KQ.STANDARD_GRAVITY);
            }
            return null;
        }
    }
}
