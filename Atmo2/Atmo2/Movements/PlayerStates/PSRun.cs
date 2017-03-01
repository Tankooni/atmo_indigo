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
    class PSRun : PlayerState
    {
        public PSRun(Player player)
			:base(player)
        {
            this.player = player;
        }
        public override void OnEnter()
        {
			player.MovementInfo.VelY = 0;
			player.image.Play("walk");
        }

        public override void OnExit()
        {
        }

        public override PlayerState Update(GameTime time)
        {
            player.RefillEnergy(time);

			if (Controller.Attack())
			{
				return new PSAttackNormal(player, KQ.STANDARD_GRAVITY);
			}
			if (Controller.LeftHeld())
            {
                player.image.FlippedX = true;
                player.MovementInfo.Move -= player.RunSpeed;
            } else if (Controller.RightHeld())
            {
                player.image.FlippedX = false;
                player.MovementInfo.Move += player.RunSpeed;
            } else
            {
                return new PSIdle(player);
            }

            Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null && !this.player.IsInvincable)
            {
                return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
            }

            if (Controller.Jump())
            {
                return new PSJump(player);
            }
            if(player.Abilities.GroundDash && 
                Controller.Dash())
            {
                if (Controller.LeftHeld() || Controller.RightHeld())
                    return new PSDash(player);
            }
            if (!player.MovementInfo.OnGround)
            {
                return new PSFall(player, KQ.STANDARD_GRAVITY);
            }

            return null;
        }
    }
}
