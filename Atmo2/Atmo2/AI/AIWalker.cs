using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atmo2.Entities;
using Atmo2.Worlds;
using Indigo;
using Indigo.Inputs;
using Indigo.Masks;
using Utility;

namespace Atmo2.AI
{
    class AIWalker : IAI
    {
        Actor entity;
        GameWorld world;
        Grid collision_map;
        bool firstrun;
        float vel_Y;

        // TODO: Move movement related stuff somewhere else
        bool isFacingLeft;
        float speed;

        public AIWalker(GameWorld world, Actor entity, float speed=200)
        {
            this.world = world;
            this.entity = entity;
            this.speed = speed;
            this.firstrun = true;
        }

        public void update()
        {
            if(firstrun)
            {
                this.collision_map = this.world.GetAllEntities()
                .FirstOrDefault((e) => e.Name == "TileCollision")
                .GetComponent<Grid>();
                firstrun = false;
            }

            float move_amount = FP.Elapsed * speed * (isFacingLeft ? -1 : 1);

            // Check if we're at a cliff
            if (!collision_map[
                (int)((entity.X + move_amount) / 16),
                (int)((entity.Y + 0.5)/ 16)])
            {
                this.isFacingLeft = !this.isFacingLeft;
                move_amount = -move_amount;
            }

            entity.MoveX(move_amount, 
                () => isFacingLeft = !isFacingLeft);

            vel_Y += 10 * FP.Elapsed;
            entity.MoveY(vel_Y, () =>
            {
                vel_Y = 0;
            });
        }
    }
}
