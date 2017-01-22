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
    class PSIdle : IPlayerState
    {
        private Player player;

        public PSIdle(Player player)
        {
            this.player = player;
        }

        public void OnEnter()
        {
            player.image.Play("stand");
        }

        public void OnExit()
        {
        }

        public IPlayerState Update(GameTime time)
        {
            player.RefillEnergy(time);

            // See if there's ground below us
            if(player.Collide(KQ.CollisionTypeSolid, player.X, player.Y + 1) == null) {
                return new PSFall(player);
            }
            // Check for enemy collision
            Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null)
            {
                return new PSOuch(player, enemy.touchDamage);
            }
            if(Controller.Down())
            {
                return new PSCharge(player);
            }
            if(Controller.Left() || Controller.Right())
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
