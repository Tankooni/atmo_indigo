using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atmo2.Entities;
using Glide;
using Indigo;
using Indigo.Utils;

namespace Atmo2.Movements.PlayerStates
{
    class PSDeath : IPlayerState
    {
        private Player player;
        private bool animation_finished;

        public PSDeath(Player player)
        {
            this.player = player;
        }
        public void OnEnter()
        {
            player.Collidable = false;
            player.image.Play("fall");
            player.Tweener.Tween(player, new { Alpha = player.Alpha }, 1)
                .From(new { Alpha = 0 })
                .Ease((t) => Engine.Random.Float(0, 1))
                .OnComplete(() => player.Alpha = 1);
            player.Tweener.Tween(player, new { X = player.resetPointX, Y = player.resetPointY}, 1)
                .Ease((t) => t)
                .OnComplete(() => this.animation_finished = true);
            
        }

        public void OnExit()
        {
            player.Collidable = true;
        }

        public IPlayerState Update(GameTime time)
        {
            if(animation_finished)
            {
                player.ResetPlayerPosition();
                player.Spice = 100;
                return new PSJump(player);
            }
            return null;
        }
    }
}
