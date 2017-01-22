using Atmo2.Entities;
using Indigo;
using Utility;

namespace Atmo2.Movements
{
	public class MovementInfo
	{
        private Actor entity;
		public bool OnGround { get; set; }
		public int AgainstWall { get; set; }
		public float MoveRefill { get; set; }

		public float Move { get; set; }
		public float VelY { get; set; }

		public MovementInfo(Actor entity)
		{
            this.entity = entity;
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

        public void Update(GameTime time)
        {

            entity.MoveX(Move);
            entity.MoveY(VelY, () => VelY = 0);

            this.OnGround = entity.Collide(KQ.CollisionTypeSolid, entity.X, entity.Y + 1) != null;
            /*this.AgainstWall += (entity.Collide(KQ.CollisionTypeSolid, entity.X + 1, entity.Y) != null) ? 1 : 0;
            this.AgainstWall -= (entity.Collide(KQ.CollisionTypeSolid, entity.X - 1, entity.Y) != null) ? 1 : 0;*/

            Move = 0;
        }
    }
}