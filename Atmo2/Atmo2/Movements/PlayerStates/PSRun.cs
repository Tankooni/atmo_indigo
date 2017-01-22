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
    class PSRun : IPlayerState
    {
        private Player player;
        public PSRun(Player player)
        {
            this.player = player;
        }
        public void OnEnter()
        {
            player.image.Play("walk");
        }

        public void OnExit()
        {
        }

        public IPlayerState Update(GameTime time)
        {
            player.RefillEnergy(time);

            if(Controller.Left())
            {
                player.image.FlippedX = true;
                player.MovementInfo.Move -= player.RunSpeed;
            } else if (Controller.Right())
            {
                player.image.FlippedX = false;
                player.MovementInfo.Move += player.RunSpeed;
            } else
            {
                return new PSIdle(player);
            }

            Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null)
            {
                return new PSOuch(player, enemy.touchDamage);
            }

            if (Controller.Jump())
            {
                return new PSJump(player);
            }
            if(Controller.Dash())
            {
                if (Controller.Left())
                    return new PSDash(player, true);
                else if (Controller.Right())
                    return new PSDash(player, false);
            }
            if (!player.MovementInfo.OnGround)
            {
                return new PSFall(player);
            }

            return null;
        }
    }
}
