using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Indigo.Graphics;
using Atmo2.AI;
using Atmo2.Sprites;
using Atmo2.Worlds;
using Indigo;
using Utility;

namespace Atmo2.Entities
{
    public class Enemy : Actor, Indigo.Loaders.IOgmoNodeHandler
    {
        public Spritemap spritemap;
        public IAI ai;
        public int touchDamage;

        private float prevX;

        // Only use this contructor with the OgmoLoader
        public Enemy() { }

        public Enemy(Spritemap spritemap, IAI ai, float x, float y)
            : base(x, y)
        {
            this.spritemap = spritemap;
            this.ai = ai;
            this.AddComponent<Image>(this.spritemap);

            this.touchDamage = 15;
        }


        public void NodeHandler(XmlNode entity)
        {
            switch(entity.Name)
            {
				default:
				case "EnemyWalker":
                    {
                        this.spritemap = SpritemapConstructor.makeWalker();
                        this.SetHitbox(spritemap.Width, spritemap.Height, (int)spritemap.OriginX, (int)spritemap.OriginY);
                        this.ai = new AIWalker(GameWorld.World, this, int.Parse(entity.Attributes["speed"].Value));
                        this.Type = KQ.CollisionTypeEnemy;
                        this.AddComponent<Image>(this.spritemap);
                        break;
                    }
                case "EnemyCrawler":
                    {
                        this.spritemap = SpritemapConstructor.makeWalker();
                        this.SetHitbox(spritemap.Width, spritemap.Height, (int)spritemap.OriginX, (int)spritemap.OriginY);
                        this.ai = new AICrawler(GameWorld.World, this, int.Parse(entity.Attributes["speed"].Value));
                        this.Type = KQ.CollisionTypeEnemy;
                        this.AddComponent<Image>(this.spritemap);
                        break;
                    }
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            ai.Update(time);
            this.spritemap.FlippedX = this.X < prevX;
            prevX = this.X;
        }
    }
}
