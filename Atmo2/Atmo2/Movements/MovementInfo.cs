using Indigo;

namespace Atmo2.Movements
{
	public class MovementInfo
	{
		public bool OnGround { get; set; }
		public int AngainstWall { get; set; }

		public int MovesRemaining { get; set; }
		public int CurrentMaxMoves { get; set; }
		public float MoveRefill { get; set; }

		public float Move { get; set; }
		public float VelY { get; set; }

		public MovementInfo()
		{
			CurrentMaxMoves = 5;
			MoveRefill = 0;
			MovesRemaining = 0;
			VelY = 0;

			Reset();
		}

		public void Reset()
		{
			OnGround = false;
			AngainstWall = 0;
			Move = 0;
		}
	}
}