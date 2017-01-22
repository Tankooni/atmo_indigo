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
    class PSFall : IPlayerState
    {
        private Player player;
        private float gravity;
        private float speed;

        public PSFall(Player player, float gravity = .3f, float speed = -1)
        {
            this.player     = player;
            this.gravity    = gravity;
            this.speed      = speed < 0 ? player.RunSpeed : speed;
        }
        public void OnEnter()
        {
            if(player.MovementInfo.VelY > 0)
                player.image.Play("fall");
        }

        public void OnExit()
        {
            player.MovementInfo.VelY = 0;
        }

        public IPlayerState Update(GameTime time)
        {
            player.MovementInfo.VelY += gravity;

            // Animation
            if (player.MovementInfo.VelY > 0)
                player.image.Play("fall");

            if (Controller.Left())
            {
                player.MovementInfo.Move -= this.speed;
                player.image.FlippedX = true;
            } else if (Controller.Right())
            {
                player.MovementInfo.Move += this.speed;
                player.image.FlippedX = false;
            }

            Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null && !this.player.IsInvincable)
            {
                return new PSOuch(player, enemy.touchDamage);
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
                if(Controller.Left())
                {
                    player.Energy -= 1;
                    return new PSDash(player, true);
                } else if(Controller.Right())
                {
                    player.Energy -= 1;
                    return new PSDash(player, false);
                }
            }

            if(Controller.Down() &&
                time.TotalMilliseconds - PSDiveKick.last_bounce > 300)
            {
                return new PSDiveKick(player);
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
