using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2
{
	public class Abilities
	{
		public bool GoodJump { get; set; }
		public bool DoubleJump { get; set; }
		public bool WallSlide { get; set; }
		public bool WallJump { get; set; }
		public bool GroundDash { get; set; }
		public bool AirDash { get; set; }

		public Abilities()
		{
			GoodJump = false;
			DoubleJump = false;
			WallSlide = false;
			WallJump = false;
			GroundDash = false;
			AirDash = false;
		}
	}
}
