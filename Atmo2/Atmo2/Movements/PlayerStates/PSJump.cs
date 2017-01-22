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
    class PSJump : IPlayerState
    {
        private Player player;

        public PSJump(Player player)
        {
            this.player = player;
        }
        public void OnEnter()
        {
            player.image.Play("jump");
        }

        public void OnExit()
        {
        }

        public IPlayerState Update(GameTime time)
        {
            Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null && !this.player.IsInvincable)
            {
                return new PSOuch(player, enemy.touchDamage);
            }

            player.MovementInfo.VelY = -player.JumpStrenth;
            return new PSFall(player);
        }
    }
}
