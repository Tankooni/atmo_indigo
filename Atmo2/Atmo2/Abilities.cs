using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2
{
	public class Abilities
	{
		public bool DoubleJump { get; set; }
		public bool WallJump { get; set; }
		public bool AirDash { get; set; }

		public Abilities()
		{
			DoubleJump = false;
			WallJump = false;
			AirDash = false;
		}
	}
}
