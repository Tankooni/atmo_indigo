using Atmo2.Entities;
using Atmo2.Movements;
using Indigo;
using Indigo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements
{
	public class Dash : Movement
	{
		public const float STARTUP_DURATION = 0;
		public const float DASH_DURATION = .1f;
		public const float RECOVERY_DURATION = 0;
		public const float DASH_SPEED = 7f;

		public float timer { get; set; }

		private int dashDir;
		private int phase;
		private Action<MovementInfo>[] phases;
		private float[] timings;

		public Dash(Player player)
			: base(player)
		{
			timer = 0;
			dashDir = 0;
			phase = 0;
			BlockGravity = true;

			phases = new Action<MovementInfo>[]
			{
				NoMove,
				Dashing
			};

			timings = new float[]
			{
				STARTUP_DURATION,
				DASH_DURATION,
				RECOVERY_DURATION
			};
		}

		public void NoMove(MovementInfo movementInfo)
		{
			movementInfo.Move = 0;
			movementInfo.VelY = 0;
		}

		public void Dashing(MovementInfo movementInfo)
		{
			movementInfo.Move = dashDir * DASH_SPEED;
			movementInfo.VelY = 0;
		}

		public override bool Restart(MovementInfo movementInfo)
		{			
			if (!movementInfo.OnGround && player.Energy <= 1)
				return false;

			if (movementInfo.Move == 0)
				return false;

			phase = 0;
			timer = timings[phase];
			if(!movementInfo.OnGround)
				player.Energy -= 1.0f;
			dashDir = Math.Sign(movementInfo.Move);

			return true;
		}

		public override void Update(GameTime time, MovementInfo movementInfo)
		{
			timer -= time.Elapsed;

			phases[phase](movementInfo);

			if(timer <= 0)
			{
				phase++;
				if (phase >= phases.Length)
					Done();
				else
					timer = timings[phase];
			}
		}

		public override string GetAnimation()
		{
			return "dash";
		}
	}
}
