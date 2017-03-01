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
    class PSOuch : PlayerState
    {
        private float duration;
        private int damage_taken;
		private float gravity;

        public PSOuch(Player player, int damage, float gravity)
			: base(player)
        {
            this.player = player;
            this.damage_taken = damage;
            this.duration = .4f;
			this.gravity = gravity;
        }
        public override void OnEnter()
        {
            this.player.image.Play("fall");
            this.player.Spice -= damage_taken;
            if (this.player.Spice == 0) return;

            this.player.MovementInfo.VelY = -4f;
            if (this.player.image.FlippedX)
                this.player.MovementInfo.Move += 8f;
            else
                this.player.MovementInfo.Move -= 8f;

            this.player.IsInvincable = true;
            this.player.Tweener.Tween(this.player, new { Alpha = 1 }, .9f)
                .From(new { Alpha = 0})
                .Ease((t) => Math.Abs(player.Alpha - 1))
                .OnComplete(() =>
                {
                    this.player.Alpha = 1;
                    this.player.IsInvincable = false;
                });
        }

        public override void OnExit()
        {
            
        }

        public override PlayerState Update(GameTime time)
        {
            this.duration -= time.Elapsed;
            if(this.duration < 0)
            {
				if (player.MovementInfo.OnGround)
					if (Controller.LeftHeld() || Controller.RightHeld())
						return new PSRun(player);
					else
						return new PSIdle(player);
				else
					return new PSFall(player, KQ.STANDARD_GRAVITY);
			}
			player.MovementInfo.VelY += gravity;
            return null;
        }
    }
}
