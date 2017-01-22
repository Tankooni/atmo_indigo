using Indigo;

namespace Atmo2.Movements
{
	public class MovementInfo
	{
		public bool OnGround { get; set; }
		public int AgainstWall { get; set; }
		public float MoveRefill { get; set; }

		public float Move { get; set; }
		public float VelY { get; set; }

		public MovementInfo()
		{
			MoveRefill = 0;
			VelY = 0;

			Reset();
		}

		public void Reset()
		{
			OnGround = false;
			AgainstWall = 0;
			Move = 0;
		}
	}
}