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
	class PSBounce : PlayerState
	{
		private float duration;
		private float strX;
		private float strY;
		private float gravity;

		public PSBounce(Player player, float gravity, float strX = 4, float strY = -2, float duration = .1f)
			: base(player)
		{
			this.player = player;
			this.gravity = gravity;
			this.strX = strX;
			this.strY = strY;
			this.duration = duration;
		}
		public override void OnEnter()
		{
			this.player.image.Play("fall");

			//this.player.MovementInfo.VelY = strY;
		}

		public override void OnExit()
		{

		}

		public override PlayerState Update(GameTime time)
		{
			duration -= time.Elapsed;

			if (!player.MovementInfo.OnGround)
			{
				if (Controller.DownHeld())
				{
					if ((Controller.Jump() || Controller.Dash()) && time.TotalMilliseconds - PSDiveKick.last_bounce > 300)
					{
						return new PSDiveKick(player, KQ.STANDARD_GRAVITY);
					}
				}

				if (player.Abilities.DoubleJump &&
					Controller.Jump() &&
					player.Energy >= 1)
				{
					player.Energy -= 1;
					return new PSJump(player);
				}

				if (player.Abilities.AirDash &&
					Controller.Dash() &&
					player.Energy >= 1)
				{
					if (Controller.LeftHeld() || Controller.RightHeld())
					{
						player.Energy -= 1;
						return new PSDash(player);
					}
				}
			}
			else
			{
				if(Controller.Jump())
					return new PSJump(player);
				if (player.Abilities.GroundDash &&
					Controller.Dash())
					return new PSDash(player);
			}
			

			if (player.image.FlippedX)
				player.MovementInfo.Move += strX;
			else
				player.MovementInfo.Move -= strX;
			if (duration < 0)
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
