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
    class PSDash : IPlayerState
    {
        private Entities.Player player;
        private bool isFacingLeft;
        private float speed;
        private float duration;

        public PSDash(Player player, bool isFacingLeft, float speed = -1, float duration = .1f)
        {
            this.player = player;
            this.isFacingLeft = isFacingLeft;
            this.speed = speed < 0 ? 7 * player.RunSpeed : speed;
            this.duration = duration;
        }
        public void OnEnter()
        {
            player.image.Play("dash");
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

            if (this.isFacingLeft)
                player.MovementInfo.Move -= this.speed;
            else
                player.MovementInfo.Move += this.speed;

            this.duration -= time.Elapsed;
            if(duration < 0)
            {
                // We're done here
                if (player.MovementInfo.OnGround)
                    if (Controller.Left() || Controller.Right())
                        return new PSRun(player);
                    else
                        return new PSIdle(player);
                else
                    return new PSFall(player);
            }
            return null;
        }
    }
}
