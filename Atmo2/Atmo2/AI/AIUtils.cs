using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atmo2.Worlds;
using Indigo;
using Indigo.Masks;

namespace Atmo2.AI
{
    public static class AIUtils
    {
        static bool isAtEdge(GameWorld world, float x, float y)
        {
            Entity tiles = world.GetAllEntities()
                .SingleOrDefault<Entity>(
                (e) => e.Name == "TileCollision");
            Grid grid = tiles.GetComponent<Grid>();
            

            return false;
        }
    }
}
