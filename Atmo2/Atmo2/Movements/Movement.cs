using Atmo2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements
{
	public class Movement
	{
		public Action Done { get; set; }
		public bool IsRunning { get; protected set; }
		public bool BlockGravity { get; protected set; }

		protected Player player;

		public Movement(Player player)
		{
			this.player = player;
		}

		public virtual void Update(MovementInfo movementInfo)
		{
		}

		public virtual bool Restart(MovementInfo movementInfo)
		{
			//	return true if the ability can be activated
			//	otherwise false
			return true;
		}

		public virtual string GetAnimation()
		{
			return null;
		}
	}
}
