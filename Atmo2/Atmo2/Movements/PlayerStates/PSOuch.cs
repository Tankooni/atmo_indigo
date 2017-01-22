using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atmo2.Entities;
using Indigo;

namespace Atmo2.Movements.PlayerStates
{
    class PSOuch : IPlayerState
    {
        private Player player;
        private float duration;
        private int damage_taken;

        public PSOuch(Player player, int damage)
        {
            this.player = player;
            this.damage_taken = damage;
            this.duration = .4f;
        }
        public void OnEnter()
        {
            this.player.image.Play("fall");
            this.player.Spice -= damage_taken;
            this.player.MovementInfo.VelY = -4f;
            if (this.player.image.FlippedX)
                this.player.MovementInfo.Move += 8f;
            else
                this.player.MovementInfo.Move -= 8f;
        }

        public void OnExit()
        {
            
        }

        public IPlayerState Update(GameTime time)
        {
            this.duration -= time.Elapsed;
            if(this.duration < 0)
            {
                return new PSIdle(player);
            }
            player.MovementInfo.VelY += .3f;
            return null;
        }
    }
}
