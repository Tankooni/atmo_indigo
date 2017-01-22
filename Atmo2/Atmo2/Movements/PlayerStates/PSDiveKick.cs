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
    class PSDiveKick : IPlayerState
    {
        private Player player;

        public PSDiveKick(Player player)
        {
            this.player = player;
        }
        public void OnEnter()
        {
            player.image.Play("diveKick");
            player.MovementInfo.VelY = 12f;
        }

        public void OnExit()
        {
        }

        public IPlayerState Update(GameTime time)
        {
            if(player.MovementInfo.OnGround)
            {
                return new PSIdle(player);
            }
            // Check for enemy collision
            Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null)
            {
                enemy.World.Remove(enemy);
                return new PSJump(player);
            }

            return null;
        }
    }
}
