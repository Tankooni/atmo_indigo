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
    class PSIdle : PlayerState
    {
        public PSIdle(Player player)
			: base(player)
		{
            this.player = player;
        }

        public override void OnEnter()
        {
			player.MovementInfo.VelY = 0;
			player.image.Play("stand");
        }

        public override void OnExit()
        {
        }

        public override PlayerState Update(GameTime time)
        {
            player.RefillEnergy(time);

            // See if there's ground below us
            if(player.Collide(KQ.CollisionTypeSolid, player.X, player.Y + 1) == null) {
                return new PSFall(player, KQ.STANDARD_GRAVITY);
            }
            // Check for enemy collision
            Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null && !this.player.IsInvincable)
            {
                return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
            }
			if(Controller.Attack())
			{
				return new PSAttackNormal(player, KQ.STANDARD_GRAVITY);
			}
            //if(Controller.Down())
            //{
            //    return new PSCharge(player);
            //}
            if(Controller.LeftHeld() || Controller.RightHeld())
            {
                return new PSRun(player);
            }
            if(Controller.Jump())
            {
                return new PSJump(player);
            }

            return null;
        }
    }
}
